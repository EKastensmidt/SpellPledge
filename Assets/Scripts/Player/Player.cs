using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPun
{
    [SerializeField] private PlayerStats playerStats;
    private Rigidbody2D rb;
    private Collider2D col;
    public PlayerStats PlayerStats { get => playerStats; set => playerStats = value; }
    public Rigidbody2D Rb { get => rb; set => rb = value; }
    public Collider2D Col { get => col; set => col = value; }

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public virtual void Update()
    {
        
    }
}
