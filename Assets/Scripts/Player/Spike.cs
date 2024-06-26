using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameScene;

public class Spike : MonoBehaviour
{
    private float CoolDown = 2f;
    public Player Player;
    public bool CanDamage;

    void Start()
    {
        CanDamage = true;
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && CanDamage)
        {
            StartCoroutine(Wait());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        CanDamage = true;
    }

    private IEnumerator Wait()
    {
        Player.Damage(1f);
        CanDamage = false;
        yield return new WaitForSeconds(CoolDown);
        CanDamage = true;
    }
}
