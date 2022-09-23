using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviourPun
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PhotonView pv;
    private void Start()
    {
        //if (!photonView.IsMine)
        //    Destroy(this);
    }

    private void Update()
    {
        pv.RPC("DestroyObject", RpcTarget.All, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player playerHit = collision.GetComponent<Player>();
        if (playerHit)
        {
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            playerHit.Rb.AddForce(-direction * playerStats.KnockbackForce, ForceMode2D.Impulse);
            playerHit.TakeDamage(playerStats.Damage);
            pv.RPC("DestroyObject", RpcTarget.All, 0f);
        }
    }

    [PunRPC]
    void DestroyObject(float secondsToDestroy)
    {
        Destroy(gameObject, secondsToDestroy);
    }
}
