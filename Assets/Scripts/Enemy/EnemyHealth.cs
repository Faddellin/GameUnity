using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour,IDamageable
{
    [SerializeField] private float maxHealth = 3f;

    private float currentHealth;
    private Patroler enemyScript;
    private float stunTime;
    private bool IsStunned;

    private void Start()
    {
        currentHealth = maxHealth;
        enemyScript = gameObject.GetComponent<Patroler>();
        stunTime = 0.3f;
        IsStunned = false;
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;

        if (!IsStunned)
        {
            StartCoroutine(Stun());
        }

        if(currentHealth <= 0f) {
            Die();
        }
    }

    private IEnumerator Stun()
    {
        Debug.Log("EnemyStunned");
        IsStunned = true;
        enemyScript.enabled = false;
        yield return new WaitForSeconds(stunTime);
        IsStunned = false;
        enemyScript.enabled = true;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
