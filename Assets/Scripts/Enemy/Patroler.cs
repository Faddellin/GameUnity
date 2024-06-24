using GameScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Patroler : MonoBehaviour
{
    [Header("EnemyBehavior")]
    public float Speed;
    public int RadiusOfPatrol;
    public float stoppingTime;
    public bool wait;

    public Transform Point;
    private Animator animator;

    private bool MovingRight;
    public Transform Player;
    private Rigidbody2D rb; 

    public float StoppingDistance;

    private float prevX;
    private float curX;

    public bool faceRight;
    public bool isAttacking;
    public int attackCounter;

    public float AttackEpsilon;

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
        Physics2D.IgnoreLayerCollision(7, 10, true);
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        prevX = transform.position.x;
        curX = transform.position.x;
        attackCounter = 1;
        faceRight = true;
        isAttacking = false;
        wait = false;
        MovingRight = true;
    }

    void Update()
    {
        prevX = curX;
        curX = transform.position.x;
        Reflect();
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
    }

    public void Chill()
    {
        if (transform.position.x > Point.position.x + RadiusOfPatrol)
        {
            if (MovingRight)
            {
                StartCoroutine(Wait());
                MovingRight = false;
            }

        }
        else if (transform.position.x < Point.position.x - RadiusOfPatrol)
        {
            if (!MovingRight)
            {
                StartCoroutine(Wait());
                MovingRight = true;
            }
        }

        if (MovingRight && !wait)
        {
           rb.velocity = new Vector2(Speed, rb.velocity.y);
            animator.SetFloat("Speed", Speed);
        }
        else if (!MovingRight && !wait)
        {
           rb.velocity = new Vector2(- Speed, rb.velocity.y);
            animator.SetFloat("Speed", Speed);
        }
    }

    private IEnumerator Wait()
    {
        wait = true;
        rb.velocity = new Vector2(0,rb.velocity.y);
        animator.SetFloat("Speed", 0);
        yield return new WaitForSeconds(stoppingTime);
        wait= false;
    }

    public void Angry()
    {
        if (isAttacking && canAttack)
        {
            StartCoroutine(Attack());
        }
        else if (Player.position.x > transform.position.x && canAttack)
        {
            rb.velocity = new Vector2 (Speed, rb.velocity.y);
            animator.SetFloat("Speed", Speed);
        }
        else if(Player.position.x < transform.position.x && canAttack)
        {
            rb.velocity = new Vector2 (-Speed, rb.velocity.y);
            animator.SetFloat("Speed", Speed);
        }
    }

    void GoBack()
    {
        if (Point.position.x > transform.position.x && canAttack)
        {
            rb.velocity = new Vector2(Speed, rb.velocity.y);
            animator.SetFloat("Speed", Speed);
        }
        else if (Point.position.x < transform.position.x && canAttack)
        {
            rb.velocity = new Vector2(-Speed, rb.velocity.y);
            animator.SetFloat("Speed", Speed);
        }
    }

    void Reflect()
    {
        if ((curX - prevX < 0 && faceRight) || (curX - prevX > 0 && !faceRight))
        {
            faceRight = !faceRight;
            animator.SetBool("IsRight", faceRight);

            attackTransform.transform.localPosition = new Vector3(-attackTransform.transform.localPosition.x,
                attackTransform.transform.localPosition.y,attackTransform.transform.localPosition.z);
        }
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        rb.velocity = new Vector2 (0, rb.velocity.y);
        animator.SetFloat("Speed", 0);
        animator.SetBool("IsAttacking", !canAttack);

        yield return new WaitForSeconds(attackingTime);

        if (attackCounter >= 4)
        {
            attackCounter = 0;
            
        }
        canAttack = true;
        animator.SetBool("IsAttacking", !canAttack);
        attackCounter++;
        animator.SetInteger("attackCount",attackCounter);
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
