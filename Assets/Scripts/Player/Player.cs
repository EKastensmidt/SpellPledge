using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPun
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Transform emitter;
    private Rigidbody2D rb;
    private Collider2D col;
    public PlayerStats PlayerStats { get => playerStats; set => playerStats = value; }
    public Rigidbody2D Rb { get => rb; set => rb = value; }
    public Collider2D Col { get => col; set => col = value; }
    public Transform Emitter { get => emitter; set => emitter = value; }

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public virtual void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        playerStats.CurrentHealth -= damage;
        if (playerStats.CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("DEAD");
    }
}
