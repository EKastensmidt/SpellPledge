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
    [SerializeField] private float attackSpeed = 0.6f, shotgunAttackSpeed = 3f;
    [SerializeField] private float projectileSpeed = 6;
    private int health;

    public float Speed { get => speed; set => speed = value; }
    public int Damage { get => damage; set => damage = value; }
    public float KnockbackForce { get => knockbackForce; set => knockbackForce = value; }
    public int Health { get => health; set => health = value; }
    public float ProjectileSpeed { get => projectileSpeed; set => projectileSpeed = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float ShotgunAttackSpeed { get => shotgunAttackSpeed; set => shotgunAttackSpeed = value; }

    public void Execute()
    {
        Health = maxHealth;
    }
}
