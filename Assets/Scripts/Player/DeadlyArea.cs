using GameScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyArea : MonoBehaviour
{
    public Player Player;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.Damage(Player.health,Player.IsFacingRight);
        }
    }
}
