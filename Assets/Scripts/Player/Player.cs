using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;


namespace GameScene
{
    public class Player : MonoBehaviour
    {
        [Header("Moving")]
        public float speed = 1f;
        public float jumpForce = 5f;
        public bool IsFacingRight;
        public bool IsFalling;
        public bool IsMoving;
        public int extraJumps;
        public float fallingGravityScale;
        public float commonGravityScale;

        [Header("Player Interface")]
        public float health;
        public int heartsNum;
        public Image[] hearts;
        public Sprite fullHeart;
        public Sprite emptyHeart;

       [Header("Materials")]
        public Material running_left;
        public Material running_right;
        public Material idle_right;
        public Material idle_left;
        public Material jump_right;
        public Material jump_left;
        public Material falling_right;
        public Material falling_left;

        [Header("Attack")]
        public Animator swordTrail;
        public int attackCounter;
        public Material playerAttack_right;
        public Material second_Attack_right;
        public Material playerAttack_left;
        public Material second_Attack_left;


        public Animator animator;
 
        public GameObject cameraFollowGo;

        [Header("Main Settings")]
        public Rigidbody2D rb;
        public SpriteRenderer sr;

        [Header("Cameras")]
        private FollowingCameraObject followingCamera;
        private float _fallSpeedYDampingChangeThreshold;

        [Header("Arms")]
        public Transform _playerArms;
        public Transform _attackArea;

        [Header("Dash")]
        public bool canDash;
        public float dashPower = 24f;
        public float dashingTime = 0.2f;
        public float dashCooldown = 1f;

        [Header("WallClimbing")]
        public bool onWall;
        public Transform wallCheckUp;
        public Transform wallCheckDown;
        public Vector2 boxSize = new Vector2(2.0f, 1.0f);



        void Start()
        {
            IsFacingRight = true;

            IsFalling = false;

            IsMoving = true;

            canDash = true;

            animator = GetComponent<Animator>();

            rb = GetComponent<Rigidbody2D>();

            sr = GetComponent<SpriteRenderer>();

            followingCamera = cameraFollowGo.GetComponent<FollowingCameraObject>();

            _fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;
        }

        private void FixedUpdate()
        {
            CheckingGround();
            CheckingWall();
        }
        void Update()
        {
            Strafe();
            Jump();
            testDamage();

            boxSize = GetComponent<BoxCollider2D>().size;

            if (rb.velocity.y < _fallSpeedYDampingChangeThreshold && !CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpedFromPlayerFalling)
            {
                CameraManager.instance.LerpYDamping(true);
            }

            if(rb.velocity.y >= 0f && !CameraManager.instance.IsLerpingYDamping && CameraManager.instance.LerpedFromPlayerFalling)
            {
                CameraManager.instance.LerpedFromPlayerFalling = false;

                CameraManager.instance.LerpYDamping(false);
            }
        }

        public void Strafe()
        {
            float movement = Input.GetAxis("Horizontal");
            if (IsMoving)
            {
                rb.velocity = new Vector2(movement * speed, rb.velocity.y);
                DashActivation();
                animator.SetFloat("Speed", Mathf.Abs(movement));
                swordTrail.SetFloat("Speed", Mathf.Abs(movement));
                if (movement < 0 && IsFacingRight)
                {
                    _playerArms.transform.localPosition = new Vector3(-_playerArms.transform.localPosition.x, _playerArms.transform.localPosition.y, _playerArms.transform.localPosition.z);
                    _attackArea.transform.localPosition = new Vector3(-_attackArea.transform.localPosition.x, _attackArea.transform.localPosition.y, _attackArea.transform.localPosition.z);
                    IsFacingRight = false;
                    animator.SetBool("IsRight", false);
                    swordTrail.SetBool("IsRight", false);
                    followingCamera.CallTurn();

                }

                if (movement > 0 && !IsFacingRight)
                {
                    _playerArms.transform.localPosition = new Vector3(-_playerArms.transform.localPosition.x, _playerArms.transform.localPosition.y, _playerArms.transform.localPosition.z);
                    _attackArea.transform.localPosition = new Vector3(-_attackArea.transform.localPosition.x, _attackArea.transform.localPosition.y, _attackArea.transform.localPosition.z);
                    IsFacingRight = true;
                    animator.SetBool("IsRight", true);
                    swordTrail.SetBool("IsRight", true);
                    followingCamera.CallTurn();
                }
            }
        }

        public void Idle()
        {
            if(IsFacingRight)
            {
                sr.sharedMaterial = idle_right;
            }
            else
            {
                sr.sharedMaterial = idle_left;
            }
        }

        public void RunningAnim()
        {
            if (IsFacingRight)
            {
                sr.sharedMaterial = running_right;
            }
            else
            {
                sr.sharedMaterial = running_left;
            }
        }

        public void JumpAnimation()
        {
            if (IsFacingRight)
            {
                sr.sharedMaterial = jump_right;
            }
            else
            {
                sr.sharedMaterial= jump_left;
            }
        }

        public void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
            {
                rb.AddForce(new Vector2(0, jumpForce - rb.velocity.y*rb.mass), ForceMode2D.Impulse);
                extraJumps--;
            }
            else if(Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && onGround)
            {
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }

        public void Falling()
        {
            if (rb.velocity.y < 0) {
                IsFalling = true;
                rb.gravityScale = fallingGravityScale;
            }
            else
            {
                IsFalling = false;
                rb.gravityScale = commonGravityScale;
            }
            animator.SetBool("IsFalling", IsFalling);
            swordTrail.SetBool("IsFalling", IsFalling);
        }

        public void FallingAnimation()
        {
            if (IsFalling)
            {
                if (IsFacingRight)
                {
                    sr.sharedMaterial = falling_right;
                }

                else
                {
                    sr.sharedMaterial = falling_left;
                }
            }
        }

        public bool onGround;
        public Transform GroundCheck;
        public Vector2 size = new Vector2(2.0f, 1.0f);
        public float angle = 0.0f;
        public LayerMask Ground;

        void CheckingGround()
        {
            onGround = Physics2D.OverlapBox(GroundCheck.position,size,angle,Ground);
            if (!onGround)
            {
                Falling();
                animator.SetBool("Jump", !onGround);
                swordTrail.SetBool("Jump", !onGround);
            }
            else
            {
                IsFalling = false;
                extraJumps = 1;
                animator.SetBool("IsFalling", IsFalling);
                animator.SetBool("Jump", !onGround);
                swordTrail.SetBool("Jump", !onGround);
                swordTrail.SetBool("IsFalling", IsFalling);
            }
            
        }

        void OnDrawGizmos()
        {
            DrawOverlapBox();
        }

        void DrawOverlapBox()
        {
            // Устанавливаем цвет Gizmos
            Gizmos.color = Color.red;

            // Создаем матрицу трансформации для поворота и перемещения бокса
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(GroundCheck.position, Quaternion.Euler(0, 0, angle), Vector3.one);
            Gizmos.matrix = rotationMatrix;

            // Рисуем проволочный куб (бокс)
            Gizmos.DrawWireCube(Vector3.zero, size);

            // Сбрасываем матрицу трансформации Gizmos
            Gizmos.matrix = Matrix4x4.identity;
        }
        void testDamage()
        {
            if(Input.GetKeyDown(KeyCode.F)) {
                Damage(1f);
            }
        }
        public void Damage(float damage)
        {
            health -= damage;
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < Mathf.RoundToInt(health))
                {
                    hearts[i].sprite = fullHeart;
                }
                else
                {
                    hearts[i].sprite = emptyHeart;
                }

                if (i < heartsNum)
                {
                    hearts[i].enabled = true;
                }
                else
                {
                    hearts[i].enabled = false;
                }
            }
            if (health <= 0f)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        public void DashActivation()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                StartCoroutine(Dash());
            }
        }

        private IEnumerator Dash()
        {
            canDash = false;
            IsMoving = false;
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;
            rb.velocity = new Vector2((Convert.ToInt32(IsFacingRight) * 2 - 1)*transform.localScale.x * dashPower, 0f);
            yield return new WaitForSeconds(dashingTime);
            rb.gravityScale = originalGravity;
            IsMoving = true;
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }

        public void A()
        {
            if (IsFacingRight)
            {
                switch (attackCounter)
                {
                    case 1:
                        sr.sharedMaterial = playerAttack_right;
                        break;
                    case 2:
                        sr.sharedMaterial = second_Attack_right;
                        attackCounter = 0;
                        break;
                }
            }
            else
            {
                switch (attackCounter)
                {
                    case 1:
                        sr.sharedMaterial = playerAttack_left;
                        break;
                    case 2:
                        sr.sharedMaterial = second_Attack_left;
                        attackCounter = 0;
                        break;
                }
            }
        }

        public void CheckingWall()
        {
            onWall = (Physics2D.OverlapBox(wallCheckUp.position, boxSize, angle, Ground)&& Physics2D.OverlapBox(wallCheckDown.position, boxSize, angle, Ground));
            animator.SetBool("OnWall", onWall);
        }


    }
}

