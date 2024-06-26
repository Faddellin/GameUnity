using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour,IDamageable
{
    [SerializeField] private float maxHealth;
    [SerializeField] private ParticleSystem getDamaged;
    private float currentHealth;
    private Patroler enemyScript;
    private float stunTime;
    private bool IsStunned;
    public bool IsDied = false;

    private void Start()
    {
        currentHealth = maxHealth;
        enemyScript = gameObject.GetComponent<Patroler>();
        stunTime = 0.3f;
        IsStunned = false;
    }

    public void Damage(float damage, bool direction)
    {
        if (direction == enemyScript.faceRight)
        {
            enemyScript.Reflect();
        }
        Vector3 addPosHigh = new Vector3(0f, 2.5f, 0f);
        ParticleSystem blood = Instantiate(getDamaged, transform.position+addPosHigh, transform.rotation);
        ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = blood.velocityOverLifetime;

        if (direction == true)
        {
            velocityOverLifetime.x = 3;
        }
        else
        {
            velocityOverLifetime.x = -3;
        }
        blood.Play();

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
        IsStunned = true;
        enemyScript.enabled = false;
        yield return new WaitForSeconds(stunTime);
        IsStunned = false;
        enemyScript.enabled = true;
    }

    private void Die()
    {
        IsDied = true;
        Destroy(gameObject);
    }
}
