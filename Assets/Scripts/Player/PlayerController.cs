using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : Player
{
    private Vector3 movement;
    public override void Start()
    {
        if (!photonView.IsMine)
            Destroy(this);
        base.Start();
    }

    public override void Update()
    {
        //if (!photonView.IsMine) return;
        Move();
    }

    private void Move()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += movement * Time.deltaTime * PlayerStats.Speed;
    }
}
