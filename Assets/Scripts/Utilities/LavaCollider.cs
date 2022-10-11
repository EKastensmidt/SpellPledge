using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;

public class LavaCollider : MonoBehaviourPun
{
    [SerializeField] private PhotonView pv;
    [SerializeField] private int lavaDmg = 2;
    [SerializeField] private float lavaDmgInterval = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!pv.IsMine) return;

        Player playerHit = collision.GetComponent<Player>();
        if (playerHit)
        {
            playerHit.IsOnLava = true;
            StartCoroutine(TakingDamage(playerHit));
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!pv.IsMine) return;

        Player playerHit = collision.GetComponent<Player>();
        if (playerHit)
        {
            playerHit.IsOnLava = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!pv.IsMine) return;

        Player playerHit = collision.GetComponent<Player>();
        if (playerHit)
        {
            playerHit.IsOnLava = false;
        }
    }

    public IEnumerator TakingDamage(Player player)
    {
        while (player.IsOnLava)
        {
            player.TakeDamage(lavaDmg);
            yield return new WaitForSeconds(lavaDmgInterval);
        }
        yield return null;
    }
}
