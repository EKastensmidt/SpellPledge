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
        if (playerHit != null)
        {
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            playerHit.ApplyKnockBack(direction);
            playerHit.TakeDamage(playerStats.Damage);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
