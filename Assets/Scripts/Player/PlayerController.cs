using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : Player
{
    private Vector3 movement;
    private float shootCD;

    private Vector3 pos;
    private float angle;
    [SerializeField] private float emitterDistance = 5f;

    public override void Start()
    {
        if (!photonView.IsMine)
            Destroy(this);
        base.Start();
        shootCD = 0f;
    }

    public override void Update()
    {
        //if (!photonView.IsMine) return;
        Move();
        UpdateEmitterPosition();
        Shoot();
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        movement = new Vector3(x, y, 0);
        SetAnimation(x, y);
        transform.position += movement * Time.deltaTime * PlayerStats.Speed;
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

    private void Shoot()
    {
        if(Input.GetKey(KeyCode.Mouse0) && shootCD < 0f)
        {
            Vector3 shootDirection;
            shootDirection = Input.mousePosition;
            shootDirection.z = 0.0f;
            shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
            shootDirection = shootDirection - transform.position;

            GameObject shotProjectile = PhotonNetwork.Instantiate("PlayerProjectile", Emitter.position, Quaternion.identity);
            Rigidbody2D projectileRb = shotProjectile.GetComponent<Rigidbody2D>();

            projectileRb.velocity = new Vector2(shootDirection.x, shootDirection.y).normalized * PlayerStats.ProjectileSpeed;
            shootCD = PlayerStats.AttackSpeed;
        }
        shootCD -= Time.deltaTime;
    }

    private void SetAnimation(float x, float y)
    {
        PV.RPC("UpdateAnimations", RpcTarget.All, "HSpeed", x);   
        PV.RPC("UpdateAnimations", RpcTarget.All, "VSpeed", y);
    }

    [PunRPC]
    void UpdateAnimations(string animationName, float movementDir)
    {
        Animator.SetFloat(animationName, movementDir);
    } 
}
