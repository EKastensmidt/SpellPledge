using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : Player
{
    private Vector3 movement;
    private float emitterOffset = 1f;
    private float shootCD;
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
        Shoot();
    }

    private void Move()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += movement * Time.deltaTime * PlayerStats.Speed;
    }

    private void Shoot()
    {
        Vector3 shootDirection = new Vector3(Input.GetAxis("Fire1"), Input.GetAxis("Fire2"), 0);
        if ((shootDirection.x != 0 || shootDirection.y != 0 || (shootDirection.x != 0 && shootDirection.y != 0)) && shootCD <= 0f)
        {
            Emitter.position += shootDirection * emitterOffset;
            GameObject shotProjectile = PhotonNetwork.Instantiate("PlayerProjectile", Emitter.position, Quaternion.identity);
            Rigidbody2D projectileRb = shotProjectile.GetComponent<Rigidbody2D>();
            projectileRb.velocity = shootDirection * PlayerStats.ProjectileSpeed;
            shootCD = PlayerStats.AttackSpeed;
        }
        Emitter.position = transform.position;
        shootCD -= Time.deltaTime;
    }
}
