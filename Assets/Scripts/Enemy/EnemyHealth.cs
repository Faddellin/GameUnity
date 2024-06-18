using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour,IDamageable
{
    [SerializeField] private float maxHealth = 3f;

    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0f) {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
