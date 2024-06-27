using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameScene;
public class Box : MonoBehaviour,IDamageable
{
    [SerializeField]
    private Player player;

    public ParticleSystem destroyEffect;
    public GameObject audioSource;
    private AudioSource audioS;

    private void Awake()
    {
        audioS = audioSource.GetComponent<AudioSource>();
    }
    public void Damage(float damage,bool direction)
    {
        audioS.Play();
        ParticleSystem destroyEffectPrefab = Instantiate(destroyEffect, transform.position, transform.rotation);
        destroyEffectPrefab.Play();
        player.FillShurikens();
        Destroy(gameObject);
    }
}
