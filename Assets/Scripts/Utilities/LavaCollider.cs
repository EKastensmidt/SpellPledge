using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LavaCollider : MonoBehaviour
{
    [SerializeField] private int lavaDmg = 2;
    [SerializeField] private float lavaDmgInterval = 0.5f;
    private float lavaCd = 0;

    private bool isOnLava = false;
    private Player player;
    private void Update()
    {
        if (isOnLava == true && lavaCd <= 0f && player != null)
        {
            player.TakeDamage(lavaDmg);
            lavaCd = lavaDmgInterval;
        }
        lavaCd -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player playerHit = collision.GetComponent<Player>();
        if (playerHit != null)
        {
            isOnLava = true;
            player = playerHit;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player playerHit = collision.GetComponent<Player>();
        if (playerHit != null)
        {
            isOnLava = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player playerHit = collision.GetComponent<Player>();
        if (playerHit != null)
        {
            isOnLava = false;
        }
    }
}
