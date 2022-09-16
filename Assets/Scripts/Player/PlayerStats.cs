using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Scriptable Object/Player", order = 0)]

public class PlayerStats : ScriptableObject
{
    [SerializeField] private int maxHealth = 100;

    [SerializeField] private float speed = 4;
    [SerializeField] private int damage = 10;
    [SerializeField] private float knockbackForce = 2;
    private int currentHealth;

    public float Speed { get => speed; set => speed = value; }
    public int Damage { get => damage; set => damage = value; }
    public float KnockbackForce { get => knockbackForce; set => knockbackForce = value; }
    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

    public void Execute()
    {
        currentHealth = maxHealth;
    }
}
