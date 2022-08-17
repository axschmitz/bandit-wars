using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public float attackRange = 8f;
    public int attackDamage = 50;
    public float attackSpeed = 0.25f;
    public int maxHealth = 200;

    protected int currentHealth;
    protected float nextAttackTime = 0f;

    public LayerMask enemyLayers;
    public GameObject arrow;

    public ParticleSystem particles;
    public TraumaInducer cameraShake;

    public Canvas UI;
    public Canvas endScreen;

    public abstract void Attack(Vector3 enemyPosition);

    public abstract void TakeDamage(int damage);

    public abstract void Die();
}
