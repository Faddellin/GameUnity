using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour,IDamageable
{
    [SerializeField] private float maxHealth;
    [SerializeField] private ParticleSystem getDamaged;
    private float currentHealth;
    private Patroler enemyScript;
    private bool IsStunned;
    public bool IsDied = false;

    private void Start()
    {
        currentHealth = maxHealth;
        enemyScript = gameObject.GetComponent<Patroler>();
        IsStunned = false;
    }

    public void Damage(float damage, bool direction)
    {
        if (!IsStunned)
        {
            StartStun();
            if (direction == enemyScript.faceRight)
            {
                enemyScript.Reflect();
            }
            Vector3 addPosHigh = new Vector3(0f, 2.5f, 0f);
            ParticleSystem blood = Instantiate(getDamaged, transform.position + addPosHigh, transform.rotation);
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
        }

        if(currentHealth <= 0f) {
            Die();
        }
    }

    public void  StartStun()
    {
        IsStunned = true;
        enemyScript.animator.SetBool("IsStunned", IsStunned);
        enemyScript.enabled = false; 
    }

    public void StopStun()
    {
        IsStunned = false;
        enemyScript.animator.SetBool("IsStunned", IsStunned);
        enemyScript.enabled = true;
    }

    private void Die()
    {
        IsDied = true;
        Destroy(gameObject);
    }
}
