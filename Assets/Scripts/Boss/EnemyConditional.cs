using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using GameScene;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;

namespace Core.AI
{
    public class EnemyConditional : Conditional
    {
        protected Rigidbody2D rb;
        protected SpriteRenderer sr;
        protected Animator animator;

        protected float Speed = 7f;

        [SerializeField]
        protected Transform attackArea;

        protected bool IsFacingRight;
        protected bool IsAttacking;
        public Player player;

        public override void OnAwake()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            IsFacingRight = true;
            IsAttacking = false;
        }
    }

}