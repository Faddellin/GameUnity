using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using GameScene;
using System;

public class PlayerAttack : MonoBehaviour
{
    private RaycastHit2D[] hits;
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private float damage = 1f;

    [SerializeField] private Transform _playerTransform;
    private Player _player;
    private Animator playerAnimator;
    public bool isNextAttack;

    public float attackingTime;
    public bool canAttack;

    public void Start()
    {
        isNextAttack = false;
        canAttack = true;
    }
    private void Awake()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        _player = _playerTransform.gameObject.GetComponent<Player>();
    }

    private void Update()
    {
        if (!_player.isJumping && (Input.GetMouseButtonDown(0) || isNextAttack))
        {
            if (canAttack)
            {
                Attack();
            }
            else
            {
                AttackDelay();
            }
            
        }
    }

    private void AttackDelay()
    {
        if (!isNextAttack && playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f)
        {
            isNextAttack = true;
        }
    }

    private void Attack()
    {

        _player.IsMoving = false;
        canAttack = false;
        _player.animator.SetBool("IsAttacking", !canAttack);
        _player.rb.velocity = new Vector2((Convert.ToInt32(_player.IsFacingRight) * 2 - 1) * _player.speed*0.1f,_player.rb.velocity.y);
        isNextAttack = false;
        
    }
    public void stopAttack()
    {
        if (_player.attackCounter >= 4)
        {
            _player.attackCounter = 0;
        }

        _player.IsMoving = true;
        canAttack = true;
        _player.animator.SetBool("IsAttacking", !canAttack);

        _player.attackCounter++;
        _player.animator.SetInteger("AttackCounter", _player.attackCounter);
    }

    public void check()
    {
        hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            IDamageable IDamageable = hits[i].collider.gameObject.GetComponent<IDamageable>();

            if (IDamageable != null)
            {
                IDamageable.Damage(damage);
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackTransform.position,attackRange);
    }
    public void startAttack2_Trail()
    {
        _player.swordTrail.SetBool("Attack2_Trail", true);
    }
    public void stopAttack2_Trail()
    {
        _player.swordTrail.SetBool("Attack2_Trail", false);
    }
    
}
