using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameScene;
using BehaviorDesigner.Runtime;
using System.Threading.Tasks;
using Core.AI;

public class BossHealth : MonoBehaviour,IDamageable
{
    public float maxHealth;
    public float currentHealth;
    public LastCut Cut;

    
    [SerializeField]private Player player;
    private Animator animator;
    private AudioSource armorAudio;

    public GameObject BodyDamage;
    private AudioSource bodyDamageSound;

    public ParticleSystem armorSparks;
    private ParticleSystem armorSparksInstaniate;

    public ParticleSystem blood;
    private ParticleSystem bloodInstaniate;

    private int backAttackCounter;
    private float AttackDistance;

    private Vector3 addVector;
    private void Start()
    {
        currentHealth = maxHealth;
        backAttackCounter = 0;
        AttackDistance = 4f;
        animator = GetComponent<Animator>();
        armorAudio = GetComponent<AudioSource>();
        bodyDamageSound = BodyDamage.GetComponent<AudioSource>();
        addVector = new Vector3(0, 2f,0);
    }
    public void Damage(float damage, bool direction, AudioSource damageSound)
    {
        if (animator.GetBool("IsRight") == direction)
        {
            currentHealth -= damage;
            backAttackCounter++;

            if(backAttackCounter >= 3 && Mathf.Abs(transform.position.x - player.transform.position.x) <= AttackDistance)
            {
               animator.SetBool("TurnAttackTrigger", true);
               backAttackCounter = 0;
            }

            bodyDamageSound.Play();
            Vector3 addPosHigh = new Vector3(0f, 2.5f, 0f);
            bloodInstaniate = Instantiate(blood, transform.position + addPosHigh, transform.rotation);
            ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = blood.velocityOverLifetime;

            if (direction == true)
            {
                velocityOverLifetime.x = 3;
            }
            else
            {
                velocityOverLifetime.x = -3;
            }

            bloodInstaniate.Play();

        }
        else
        {
            armorSparksInstaniate = Instantiate(armorSparks, transform.position+addVector, transform.rotation);
            armorAudio.Play();
            armorSparks.Play();
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Cut.StartEnding();
        Destroy(gameObject);
    }


}
