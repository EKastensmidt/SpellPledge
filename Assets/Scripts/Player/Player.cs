using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

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

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public PhotonView PV { get => pv; set => pv = value; }
    public TextMeshPro Tmp { get => tmp; set => tmp = value; }
    public bool IsOnLava { get => isOnLava; set => isOnLava = value; }
    public GameManager GameManager { get => gameManager; set => gameManager = value; }

    private int currentHealth;
    [SerializeField] private Animator animator;
    [SerializeField] private PhotonView pv;
    [SerializeField] private TextMeshPro tmp;

    private GameManager gameManager;
    private bool isOnLava = false;


    public virtual void Start()
    {
        playerStats.Execute();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        currentHealth = playerStats.Health;
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public virtual void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        pv.RPC("DamageTaken", RpcTarget.All, damage);
        ChangePlayerHealth();
        if (currentHealth <= 0)
        {
            pv.RPC("Die", pv.Owner);
        }
    }

    [PunRPC]
    public void Die()
    {
        Debug.Log("DEAD");
        PhotonNetwork.Destroy(this.gameObject);
        gameManager.SetLoser(this);
    }

    public void ApplyKnockBack(Vector2 direction)
    {
        pv.RPC("AddKnockBack", RpcTarget.All, direction);
    }

    public void ChangePlayerHealth()
    {
        pv.RPC("UpdatePlayerHealth", RpcTarget.All);
    }


    // PUNRPCs
    [PunRPC]
    public void AddKnockBack(Vector2 direction)
    {
        rb.AddForce(-direction * playerStats.KnockbackForce, ForceMode2D.Impulse);
    }

    [PunRPC]
    public void DamageTaken(int damage)
    {
        currentHealth -= damage;
    }

    [PunRPC]
    public void UpdatePlayerHealth()
    {
        Tmp.text = "HP: " + currentHealth;
    }
}
