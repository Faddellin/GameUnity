using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using GameScene;

public class PlayerAnimations : MonoBehaviour
{
    public Transform playerTransform;
    private Player player;
    private Animator swordTrail;

    private bool IsFacingRight;
    private bool IsFalling;
    private int attackCounter;

    private SpriteRenderer sr;

    private void Start()
    {
        player = playerTransform.GetComponent<Player>();
        sr = player.GetComponent<SpriteRenderer>();
    }

    [Header("Materials Left")]
    public Material running_left;
    public Material idle_left;
    public Material jump_left;
    public Material falling_left;
    public Material playerAttack_left;
    public Material second_Attack_left;
    public Material forth_Attack_left;
    public Material third_Attack_left;
    public Material slide_left;
    public Material throw_left;
    public Material damaged_left;
    public Material death_left;

    [Header("Materials Right")]
    public Material running_right;
    public Material idle_right;
    public Material jump_right;
    public Material falling_right;
    public Material playerAttack_right;
    public Material second_Attack_right;
    public Material forth_Attack_right;
    public Material third_Attack_right;
    public Material slide_right;
    public Material throw_right;
    public Material damaged_right;
    public Material death_right;
    private void Update()
    {
        IsFacingRight = player.IsFacingRight;
        IsFalling = player.IsFalling;
        attackCounter = player.attackCounter;
        swordTrail = player.swordTrail;
    }
    public void IdleAnim()
    {
        if (IsFacingRight)
        {
            sr.sharedMaterial = idle_right;
        }
        else
        {
            sr.sharedMaterial = idle_left;
        }
    }
    public void RunningAnim()
    {
        if (IsFacingRight)
        {
            sr.sharedMaterial = running_right;
        }
        else
        {
            sr.sharedMaterial = running_left;
        }
    }

    public void JumpAnimation()
    {
        if (IsFacingRight)
        {
            sr.sharedMaterial = jump_right;
        }
        else
        {
            sr.sharedMaterial = jump_left;
        }
    }
    public void FallingAnimation()
    {
        if (IsFalling)
        {
            if (IsFacingRight)
            {
                sr.sharedMaterial = falling_right;
            }

            else
            {
                sr.sharedMaterial = falling_left;
            }
        }
    }

    public void AttackAnimation()
    {
        if (IsFacingRight)
        {
            switch (attackCounter)
            {
                case 1:
                    sr.sharedMaterial = second_Attack_right;
                    break;
                case 2:
                    sr.sharedMaterial = third_Attack_right;
                    break;
                case 3:
                    sr.sharedMaterial = playerAttack_right;
                    break;
                case 4:
                    sr.sharedMaterial = forth_Attack_right;
                    break;
            }
        }
        else
        {
            switch (attackCounter)
            {
                case 1:
                    sr.sharedMaterial = second_Attack_left;
                    break;
                case 2:
                    sr.sharedMaterial = third_Attack_left;
                    break;
                case 3:
                    sr.sharedMaterial = playerAttack_left;
                    break;
                case 4:
                    sr.sharedMaterial = forth_Attack_left;
                    break;
            }
        }
    }

    public void ThrowAnimation()
    {
        if (IsFacingRight)
        {
            sr.sharedMaterial = throw_right;
        }
        else
        {
            sr.sharedMaterial = throw_left;
        }
    }

    public void GetDamagedAnimation()
    {
        if (IsFacingRight)
        {
            sr.sharedMaterial = damaged_right;
        }
        else
        {
            sr.sharedMaterial = damaged_left;
        }
    }

    public void DeathAnimation()
    {
        if (IsFacingRight)
        {
            sr.sharedMaterial = death_right;
        }
        else
        {
            sr.sharedMaterial = death_left;
        }
    }

    public void StopDamagingAnimation()
    {
        player.damaged = false;
        player.animator.SetBool("IsDamaged",player.damaged);
    }

    public void SlidingAnimation()
    {
        if (!IsFacingRight)
        {
            sr.sharedMaterial = slide_right;
        }
        else
        {
            sr.sharedMaterial = slide_left;
        }
    }

    public void startAttack2Right_Trail()
    {
        swordTrail.SetBool("Attack2Right_Trail", true);
    }
    public void startAttack2Left_Trail()
    {
        swordTrail.SetBool("Attack2Left_Trail", true);
    }
    public void startAttack1Right_Trail()
    {
        swordTrail.SetBool("Attack1Right_Trail", true);
    }
    public void startAttack1Left_Trail()
    {
        swordTrail.SetBool("Attack1Left_Trail", true);
    }
    public void startAttack3Left_Trail()
    {
        swordTrail.SetBool("Attack3Left_Trail", true);
    }
    public void startAttack3Right_Trail()
    {
        swordTrail.SetBool("Attack3Right_Trail", true);
    }
    public void startAttack4Right_Trail()
    {
        swordTrail.SetBool("Attack4Right_Trail", true);
    }
    public void startAttack4Left_Trail()
    {
        swordTrail.SetBool("Attack4Left_Trail", true);
    }
}
