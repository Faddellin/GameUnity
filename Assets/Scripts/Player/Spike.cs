using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameScene;

public class Spike : MonoBehaviour
{
    private float CoolDown = 0f;
    public Player Player;
    private bool CanDamage;

    void Start()
    {
        CanDamage = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && CanDamage)
        {
            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        Player.Damage(1f);
        CanDamage = false;
        yield return new WaitForSeconds(CoolDown);
        CanDamage = true;
    }
}
