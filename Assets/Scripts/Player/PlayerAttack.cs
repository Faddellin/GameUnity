using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using GameScene;

public class PlayerAttack : MonoBehaviour
{
    private RaycastHit2D[] hits;
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private float damage = 1f;

    [SerializeField] private Transform _playerTransform;
    private Player _player;
    public bool IsFacingRight;
    public bool IsAttacking;

    public float attackingTime;
    public bool canAttack;

    public void Start()
    {
        IsAttacking = false;
        canAttack = true;
    }
    private void Awake()
    {
        _player = _playerTransform.gameObject.GetComponent<Player>();
        IsFacingRight = _player.IsFacingRight;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
           StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        IsAttacking = true;
        _player.animator.SetBool("IsAttacking", IsAttacking);

        if (_player.attackCounter > 2)
        {
            _player.attackCounter = 0;
        }

        yield return new WaitForSeconds(attackingTime);
        IsAttacking = false;
        canAttack = true;
        _player.animator.SetBool("IsAttacking", IsAttacking);


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
