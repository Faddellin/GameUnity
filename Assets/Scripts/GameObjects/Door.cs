using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameScene;
public class Door : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Player player;
    private float maxHealth;
    private float currenthealth;
    public ParticleSystem destroyEffect;
    public GameObject audioSource;
    private AudioSource audioS;

    public GameObject damageSource;
    private AudioSource damageSound;
    private void Awake()
    {
        audioS = audioSource.GetComponent<AudioSource>();
        damageSound = damageSource.GetComponent<AudioSource>();
        maxHealth = 5f;
        currenthealth = maxHealth;
    }
    public void Damage(float damage, bool direction, AudioSource destroySound)
    {
        currenthealth -= damage;
        damageSound.Play();
        if (currenthealth <= 0)
        {
            audioS.Play();
            ParticleSystem destroyEffectPrefab = Instantiate(destroyEffect, transform.position, transform.rotation);
            destroyEffectPrefab.Play();
            Destroy(gameObject);
        }
    }
}
