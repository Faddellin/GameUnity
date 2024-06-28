using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameScene;
using BehaviorDesigner.Runtime;
using System.Threading.Tasks;
using Core.AI;


public class BossDependencies : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;


    [SerializeField] private Player player;
    private Animator animator;
    public BehaviorTree behaviorTree;
    private DefaultAttack defAttack;
    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        behaviorTree = GetComponent<BehaviorTree>();
        defAttack = behaviorTree.FindTask<DefaultAttack>();
    }

    public void StopAttack()
    {
       
        defAttack.StopAttack();
    }
    
    public void check()
    {
        defAttack.check();
    }

    public void StopTurnAttack()
    {
        animator.SetBool("TurnAttackTrigger", false);
    }

    public void checkTurnAttack()
    {
        defAttack.checkTurnAttack();
    }

    public void PlaySound()
    {
        defAttack.PlaySound();
    }
}
