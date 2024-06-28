using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Core.AI
{
    public class FacePlayer: EnemyAction
    {

        public override void OnAwake()
        {
            base.OnAwake();
        }

        public override TaskStatus OnUpdate()
        {
            Reflect();
            return TaskStatus.Success;
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