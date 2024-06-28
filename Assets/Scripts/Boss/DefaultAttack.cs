using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using GameScene;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;

namespace Core.AI
{
    public class DefaultAttack : EnemyAction
    {
        public GameObject BossHit;
        private AudioSource hitSound;
        public GameObject airSwing;
        private AudioSource airSwingSound;
        public override void OnStart()
        {

            StartAttack();
            hitSound = BossHit.GetComponent<AudioSource>();
            airSwingSound = airSwing.GetComponent<AudioSource>();
        }


        public void check()
        {
            hits = Physics2D.CircleCastAll(attackArea.position, attackRange, transform.right, 0f, attackableLayer);


            for (int i = 0; i < hits.Length; i++)
            {
                IDamageable IDamageable = hits[i].collider.gameObject.GetComponent<IDamageable>();

                if (IDamageable != null)
                {
                    IDamageable.Damage(damage,IsFacingRight,hitSound);
                }
            }
        }

        public void PlaySound()
        {
            airSwingSound.Play();
        }

        public void checkTurnAttack()
        {
            hits = Physics2D.CircleCastAll(attackArea.position, attackRange, transform.right, 0f, attackableLayer);


            for (int i = 0; i < hits.Length; i++)
            {
                IDamageable IDamageable = hits[i].collider.gameObject.GetComponent<IDamageable>();

                if (IDamageable != null)
                {
                    IDamageable.Damage(damage, !IsFacingRight, hitSound);
                }
            }
        }

        public void StartAttack()
        {
            IsAttacking = true;
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetFloat("Speed", 0);
            animator.SetBool("IsAttacking", IsAttacking);
        }

        public TaskStatus StopAttack()
        { 
            IsAttacking = false;
            animator.SetBool("IsAttacking", IsAttacking);
            return TaskStatus.Success;
        }

        public override void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(attackArea.position, attackRange);
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