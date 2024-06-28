using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using GameScene;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;

namespace Core.AI
{
    public class EnemyAction : Action
    {
        protected Rigidbody2D rb;
        protected SpriteRenderer sr;
        protected Animator animator;

        protected float Speed = 5f;


        [SerializeField]  static public bool IsFacingRight = true;
        [SerializeField]  static public bool IsAttacking = false;

        [SerializeField]
        protected Transform attackArea;
        [SerializeField]
        protected Transform backwardAttackArea;
        public Player player;

        [Header("Attack Parameters")]
        protected float AttackDistance = 4f;
        protected RaycastHit2D[] hits;
        [SerializeField] protected float attackRange;
        [SerializeField] protected LayerMask attackableLayer;
        [SerializeField] protected float damage =2f;

        public override void OnAwake()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }
    }

}