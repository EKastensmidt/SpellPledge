using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : Player
{
    private Vector3 movement;
    private float normalProjectileCD;
    private float shotgunProjectileCD;
    private float blinkCD;
    private float shieldCD;

    private Vector3 pos;
    private float angle;
    [SerializeField] private float emitterDistance = 5f;

    private float shieldDuration = 1f;

    public override void Start()
    {
        base.Start();
        normalProjectileCD = 0f;
    }

    public override void Update()
    {
        if (!photonView.IsMine) return;
        if (!GameManager.IsGameStarted) return;
        Move();
        UpdateEmitterPosition();
        Skills();
        ui.UpdateSkillUI(normalProjectileCD, shotgunProjectileCD, blinkCD, shieldCD);
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        movement = new Vector3(x, y, 0);
        transform.position += movement * Time.deltaTime * PlayerStats.Speed;
        SetAnimation(x,y);
    }

    private void UpdateEmitterPosition()
    {
        pos = Input.mousePosition;
        pos.z = (transform.position.z - Camera.main.transform.position.z);
        pos = Camera.main.ScreenToWorldPoint(pos);
        pos = pos - transform.position;
        angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        if (angle < 0.0f) angle += 360.0f;
        Emitter.transform.localEulerAngles = new Vector3(0, 0, angle);
        float xPos = Mathf.Cos(Mathf.Deg2Rad * angle) * emitterDistance;
        float yPos = Mathf.Sin(Mathf.Deg2Rad * angle) * emitterDistance;
        Emitter.transform.position = new Vector3(transform.position.x + xPos, transform.position.y + yPos, 0);

        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Emitter.transform.LookAt(new Vector3(0, 0, mousepos.z));
    }

    private void Skills()
    {
        NormalProjectile();
        ShotgunProjectile();
        Blink();
        Shield();
    }

    private void NormalProjectile()
    {
        if (normalProjectileCD < 0)
        {
            ui.IsSkillOnCD("Projectile", false);

            if (Input.GetKey(KeyCode.Mouse0))
            {
                Vector3 shootDirection = GetShootDirection();

                GameObject shotProjectile = PhotonNetwork.Instantiate("PlayerProjectile", Emitter.position, Quaternion.identity);
                Rigidbody2D projectileRb = shotProjectile.GetComponent<Rigidbody2D>();

                projectileRb.velocity = new Vector2(shootDirection.x, shootDirection.y).normalized * PlayerStats.ProjectileSpeed;
                normalProjectileCD = PlayerStats.AttackSpeed;

                StartCoroutine(DestroyObject(5f, shotProjectile));
                ui.IsSkillOnCD("Projectile", true);
            }
        }
        normalProjectileCD -= Time.deltaTime;
    }

    private void ShotgunProjectile()
    {
        if (shotgunProjectileCD < 0f)
        {
            ui.IsSkillOnCD("Shotgun", false);

            if (Input.GetKey(KeyCode.Mouse1))
            {
                Vector3 shootDirection = GetShootDirection();
                float spreadAngle = 0;

                for (int i = 0; i <= 2; i++)
                {
                    GameObject shotProjectile = PhotonNetwork.Instantiate("PlayerProjectile", Emitter.position, Quaternion.identity);
                    Rigidbody2D projectileRb = shotProjectile.GetComponent<Rigidbody2D>();

                    switch (i)
                    {
                        case 0:
                            spreadAngle = 15f;
                            break;
                        case 1:
                            spreadAngle = 0f;
                            break;
                        case 2:
                            spreadAngle = -15f;
                            break;
                    }

                    float rotateAngle = spreadAngle + (Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg);
                    Vector2 MovementDirection = new Vector2(Mathf.Cos(rotateAngle * Mathf.Deg2Rad), Mathf.Sin(rotateAngle * Mathf.Deg2Rad)).normalized;
                    projectileRb.velocity = MovementDirection * (PlayerStats.ProjectileSpeed * 2f);

                    StartCoroutine(DestroyObject(0.3f, shotProjectile));
                }
                ui.IsSkillOnCD("Shotgun", true);
                shotgunProjectileCD = PlayerStats.ShotgunAttackSpeed;
            }
        }
        shotgunProjectileCD -= Time.deltaTime;
    }

    private void Blink()
    {
        if(blinkCD <= 0)
        {
            ui.IsSkillOnCD("Blink", false);

            Vector3 blinkTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                GameObject blink = PhotonNetwork.Instantiate("BlinkEffect", transform.position, Quaternion.identity);
                StartCoroutine(DestroyObject(1.5f, blink));

                transform.position = Vector3.MoveTowards(transform.position, blinkTo, PlayerStats.BlinkMaxDistance);
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

                GameObject blink2 = PhotonNetwork.Instantiate("BlinkEffect", transform.position, Quaternion.identity);
                StartCoroutine(DestroyObject(1.5f, blink2));

                blinkCD = PlayerStats.BlinkSpeed;
                ui.IsSkillOnCD("Blink", true);
            }
        }
        blinkCD -= Time.deltaTime;
    }

    private void Shield()
    {
        if(shieldCD <= 0f)
        {
            ui.IsSkillOnCD("Shield", false);

            if (Input.GetKey(KeyCode.E))
            {
                StartCoroutine(SetShield());
                shieldCD = PlayerStats.ShieldSpeed;
                ui.IsSkillOnCD("Shield", true);

            }
        }
        shieldCD -= Time.deltaTime;
    }

    private void SetAnimation(float x, float y)
    {
        UpdateAnimations("HSpeed", x);
        UpdateAnimations("VSpeed", y);

    }

    public void UpdateAnimations(string animationName, float movementDir)
    {
        Animator.SetFloat(animationName, movementDir);
    } 

    private Vector3 GetShootDirection()
    {
        Vector3 shootDirection;
        shootDirection = Input.mousePosition;
        shootDirection.z = 0.0f;
        shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
        shootDirection = shootDirection - transform.position;
        return shootDirection;
    }

    IEnumerator DestroyObject(float secondsToDestroy, GameObject projectile)
    {
        yield return new WaitForSeconds(secondsToDestroy);
        if(projectile != null)
        {
            PhotonNetwork.Destroy(projectile);
        }
    }

    IEnumerator SetShield()
    {
        PV.RPC("SetShield", RpcTarget.All, true);
        GameObject shield = PhotonNetwork.Instantiate("Shield", transform.position, Quaternion.identity);
        shield.GetComponent<Shield>().Player = gameObject;
        yield return new WaitForSeconds(shieldDuration);
        PV.RPC("SetShield", RpcTarget.All, false);
        StartCoroutine(DestroyObject(0f, shield));
    }
}
