using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameScene;

public class GroundCheck : MonoBehaviour
{
    public Player player;
    public LayerMask Ground;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((Ground.value & (1 << collision.gameObject.layer)) > 0)
        {
            player.onGround = true;
            if (player.rb.velocity.y == 0)
            {
                player.isJumping = false;
                player.animator.SetBool("Jump", false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    { 
        StartCoroutine(JumpDelay());   
    }

    private  IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(0.15f);
        player.onGround = false;
    }

}
