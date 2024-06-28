using BehaviorDesigner.Runtime.Tasks;
using GameScene;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Core.AI
{
    public class MovingTowards : EnemyAction
    {
        public override TaskStatus OnUpdate()
        {
            Reflect();
            if (player.transform.position.x > transform.position.x && !IsAttacking)
            {
                rb.velocity = new Vector2(Speed, rb.velocity.y);
                animator.SetFloat("Speed", Speed);
            }
            else if (player.transform.position.x < transform.position.x && !IsAttacking)
            {
                rb.velocity = new Vector2(-Speed, rb.velocity.y);
                animator.SetFloat("Speed", Speed);
                
            }
 
            if(Mathf.Abs(transform.position.x - player.transform.position.x) <= AttackDistance)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                animator.SetFloat("Speed", 0);
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }
        public void Reflect()
        {
            if ((player.transform.position.x < transform.position.x && IsFacingRight && !IsAttacking)
                || (player.transform.position.x > transform.position.x && !IsFacingRight && !IsAttacking))
            {
                IsFacingRight = !IsFacingRight;
                animator.SetBool("IsRight", IsFacingRight);

                attackArea.transform.localPosition = new Vector3(-attackArea.transform.localPosition.x,
                    attackArea.transform.localPosition.y, attackArea.transform.localPosition.z);

                backwardAttackArea.transform.localPosition = new Vector3(-backwardAttackArea.transform.localPosition.x,
                    backwardAttackArea.transform.localPosition.y, backwardAttackArea.transform.localPosition.z);
            }
        }
    }

}
