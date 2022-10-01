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
        //if (!pv.IsMine)
        //    Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!pv.IsMine) return;
        Player playerHit = collision.GetComponent<Player>();
        if (playerHit!=null)
        {
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            //LLamar rpc dentro de player al owner, para el knockback.
            playerHit.Rb.AddForce(-direction * playerStats.KnockbackForce, ForceMode2D.Impulse);
            playerHit.TakeDamage(playerStats.Damage);
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    void DestroyObject(float secondsToDestroy)
    {
        Destroy(gameObject, secondsToDestroy);
    }
}
