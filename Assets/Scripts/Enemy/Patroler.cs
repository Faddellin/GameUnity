using GameScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Patroler : MonoBehaviour
{
    [Header("EnemyMoving")]
    public float Speed;
    public int RadiusOfPatrol;
    public Transform Point;
    private Animator animator;

    bool MovingRight;

    Transform Player;
    public float StoppingDistance;
    private float prevX;
    private float curX;
    public bool faceRight;
    public bool isAttacking;
    public float AttackEpsilon = 7f;

    bool chill = false;
    bool angry = false;
    bool goBack = false;

    [Header("EnemyMeleeAttack")]
    private RaycastHit2D[] hits;
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private float damage = 1f;

    public float attackingTime;
    public bool canAttack;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        prevX = transform.position.x;
        curX = transform.position.x;

        faceRight = true;
        isAttacking = false;
    }

    void Update()
    {
        curX = transform.position.x;
        Reflect();
        prevX = curX;
        if (Vector2.Distance(transform.position, Point.position) < RadiusOfPatrol && !angry)
        {
            chill = true;
        }

        if (Vector2.Distance(transform.position, Player.position) < StoppingDistance)
        {
            angry = true;
            chill = false;
            goBack = false;
        }

        if (Vector2.Distance(transform.position, Player.position) > StoppingDistance)
        {
            goBack = true;
            angry = false;
        }

        if (Vector2.Distance(transform.position, Player.position) < AttackEpsilon)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        if (isAttacking && canAttack)
        {
           StartCoroutine(Attack());
        }

        if (chill)
        {
            Chill();
        }
        else if (angry)
        {
            Angry();
        }
        else if (goBack)
        {
            GoBack();
        }
        if (isAttacking)
        {
            //animator.SetBool("isAttacking", isAttacking);
        }
        else
        {
            //animator.SetBool("isChasing", angry);
        }
    }

    void Chill()
    {
        Speed = 0f;
        if (transform.position.x > Point.position.x + RadiusOfPatrol)
        {
            MovingRight = false;
        }
        else if (transform.position.x < Point.position.x - RadiusOfPatrol)
        {
            MovingRight = true;
        }

        if (MovingRight)
        {
            transform.position = new Vector2(transform.position.x + Speed * Time.deltaTime, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - Speed * Time.deltaTime, transform.position.y);
        }
    }

    void Angry()
    {
        Speed = 0f;
        transform.position = Vector2.MoveTowards(transform.position, Player.position, Speed * Time.deltaTime);
    }

    void GoBack()
    {
        transform.position = Vector2.MoveTowards(transform.position, Point.position, Speed * Time.deltaTime);
    }

    void Reflect()
    {
        if ((curX - prevX < 0 && !faceRight) || (curX - prevX > 0 && faceRight))
        {
            faceRight = !faceRight;
            animator.SetBool("IsRight", faceRight);
        }
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        Speed = 0f;
        check();
        //_player.animator.SetBool("IsAttacking", IsAttacking);

        /* if (_player.attackCounter >= 2)
         {
             _player.attackCounter = 0;
         }*/

        yield return new WaitForSeconds(attackingTime);
        isAttacking = false;
        canAttack = true;
        Speed = 6f;
        //_player.animator.SetBool("IsAttacking", IsAttacking);
        //_player.attackCounter++;
        //_player.animator.SetInteger("AttackCounter", _player.attackCounter);
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
        Gizmos.DrawWireSphere(attackTransform.position, attackRange);
    }
}
