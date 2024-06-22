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
    public class Player : MonoBehaviour,IDamageable
    {
        [Header("Moving")]
        public float speed;
        public float jumpForce;
        public bool IsFacingRight;
        public bool IsFalling;
        public bool IsMoving;
        public int extraJumps;
        private bool jumpDelay;
        public float fallingGravityScale;
        public float commonGravityScale;

        [Header("Player Interface")]
        public float health;
        public int heartsNum;
        public Image[] hearts;
        public Sprite fullHeart;
        public Sprite emptyHeart;


        [Header("Attack")]
        public PlayerAttack playeratak;
        public Animator swordTrail;
        public int attackCounter;


        public Animator animator;
 
        public GameObject cameraFollowGo;

        [Header("Main Settings")]
        public  Rigidbody2D rb;
        public  SpriteRenderer sr;

        [Header("Cameras")]
        private FollowingCameraObject followingCamera;
        private float _fallSpeedYDampingChangeThreshold;

        [Header("Arms")]
        public Transform _playerArms;
        public Transform _attackArea;
        private Transform _shurikenSpawnPoint;

        [Header("Dash")]
        public bool canDash;
        public float dashPower = 24f;
        public float dashingTime = 0.2f;
        public float dashCooldown = 1f;

        [Header("WallJump")]
        public bool onWall;
        public Transform wallCheckUp;
        public Transform wallCheckDown;
        public Vector2 boxSize;

        [Header("OnGround/Wall")]
        public bool onGround;
        public Transform GroundCheck;
        public Vector2 size = new Vector2(2.0f, 1.0f);
        public float angle = 0.0f;
        public LayerMask Ground;
        public LayerMask Wall;


        void Start()
        {
            boxSize = wallCheckUp.GetComponent<BoxCollider2D>().size;

            playeratak = gameObject.GetComponent<PlayerAttack>();

            _shurikenSpawnPoint = transform.Find("WeaponAxis");

            IsFacingRight = true;

            IsFalling = false;

            IsMoving = true;

            canDash = true;

            jumpDelay = false;

            animator = GetComponent<Animator>();

            rb = GetComponent<Rigidbody2D>();

            sr = GetComponent<SpriteRenderer>();

            followingCamera = cameraFollowGo.GetComponent<FollowingCameraObject>();

            _fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;
        }

        private void FixedUpdate()
        {
            CheckingWall();
        }
        void Update()
        {
            WallJumpActivation();
            CheckingGround();
            Strafe();
            Jump();
            testDamage();
            GoDown();
            ChangeCameraPos();
           
        }

        public void ChangeCameraPos() {

            if (rb.velocity.y < _fallSpeedYDampingChangeThreshold && !CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpedFromPlayerFalling)
            {
                CameraManager.instance.LerpYDamping(true);
            }

            if (rb.velocity.y >= 0f && !CameraManager.instance.IsLerpingYDamping && CameraManager.instance.LerpedFromPlayerFalling)
            {
                CameraManager.instance.LerpedFromPlayerFalling = false;

                CameraManager.instance.LerpYDamping(false);
            }
        }


        public void Strafe()
        {
            float movement;
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && !playeratak.IsAttacking)
            {
                movement = Input.GetAxis("Horizontal");
            }
            else
            {
                movement = 0.0f;
            }

            if (IsMoving)
            {
                rb.velocity = new Vector2(movement * speed, rb.velocity.y);
                DashActivation();
                animator.SetFloat("Speed", Mathf.Abs(movement));
                
                if (movement < 0 && IsFacingRight)
                {
                    Flip();
                }

                if (movement > 0 && !IsFacingRight)
                {
                    Flip();
                }
            }
        }

        public void Flip()
        {
            
            wallCheckUp.transform.localPosition = new Vector3(-wallCheckUp.transform.localPosition.x, wallCheckUp.transform.localPosition.y, wallCheckUp.transform.localPosition.z);
            wallCheckDown.transform.localPosition = new Vector3(-wallCheckDown.transform.localPosition.x, wallCheckDown.transform.localPosition.y, wallCheckDown.transform.localPosition.z);

            _shurikenSpawnPoint.transform.localPosition = new Vector3(-_shurikenSpawnPoint.transform.localPosition.x, _shurikenSpawnPoint.transform.localPosition.y, _shurikenSpawnPoint.transform.localPosition.z);
            _playerArms.transform.localPosition = new Vector3(-_playerArms.transform.localPosition.x, _playerArms.transform.localPosition.y, _playerArms.transform.localPosition.z);
            _attackArea.transform.localPosition = new Vector3(-_attackArea.transform.localPosition.x, _attackArea.transform.localPosition.y, _attackArea.transform.localPosition.z);
            IsFacingRight = !IsFacingRight;
            animator.SetBool("IsRight", IsFacingRight);
            followingCamera.CallTurn();
        }


        public void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0 && !onGround && !onWall)
            {
                rb.AddForce(new Vector2(0, jumpForce - rb.velocity.y*rb.mass), ForceMode2D.Impulse);
                animator.SetBool("Jump", true);
                extraJumps--;
            }
            else if((Input.GetKeyDown(KeyCode.Space)||jumpDelay) && onGround)
            {
                rb.AddForce(new Vector2(0, jumpForce - rb.velocity.y * rb.mass), ForceMode2D.Impulse);
                animator.SetBool("Jump", true);
                jumpDelay = false;
            }
            else if(Input.GetKeyDown(KeyCode.Space) && !jumpDelay)
            {
                StartCoroutine(JumpDelay());
            } 
        }

        private IEnumerator JumpDelay()
        {
            jumpDelay = true;
            yield return new WaitForSeconds(0.2f);
            jumpDelay = false;
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
        }


        public void CheckingGround()
        {
            //onGround = Physics2D.OverlapBox(GroundCheck.position, size, angle, Ground);
            //StartCoroutine(OnGroundDelay());
            if (!onGround)
            {
                Falling();
            }
            else if (onGround)
            {
                IsFalling = false;
                extraJumps = 1;
                animator.SetBool("IsFalling", IsFalling);
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
            Gizmos.DrawWireCube(Vector3.zero, boxSize);

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
            animator.SetBool("IsDashing", true);

            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;

            Physics2D.IgnoreLayerCollision(7, 10, true);

            rb.velocity = new Vector2((Convert.ToInt32(IsFacingRight) * 2 - 1) * transform.localScale.x * dashPower, 0f);

            yield return new WaitForSeconds(dashingTime);

            rb.gravityScale = originalGravity;
            IsMoving = true;
            animator.SetBool("IsDashing", false);

            Physics2D.IgnoreLayerCollision(7, 10, false);

            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }

        public void CheckingWall()
        {
            onWall = (Physics2D.OverlapBox(wallCheckUp.position, boxSize, angle, Wall) && Physics2D.OverlapBox(wallCheckDown.position, boxSize, angle, Wall));
            animator.SetBool("OnWall", onWall);
           
        }

        public void WallJumpActivation()
        {
            if (!onGround && onWall && Input.GetKeyDown(KeyCode.Space) && canWallJump)
            {

                StartCoroutine(WallJump());
            }
        }

        public bool canWallJump;
        public float wallJumpCooldown;

        private IEnumerator WallJump()
        {
            canWallJump = false;
            IsMoving = false;

            rb.AddForce(new Vector2(-(Convert.ToInt32(IsFacingRight) * 2 - 1)*jumpForce*0.9f, jumpForce - rb.velocity.y * rb.mass),
            ForceMode2D.Impulse);

            Flip();
            animator.SetBool("IsRight", IsFacingRight);

            yield return new WaitForSeconds(0.18f);
            IsMoving = true;
            yield return new WaitForSeconds(wallJumpCooldown-0.1f);
            canWallJump = true;
            extraJumps = 1;
        }

        public void GoDown()
        {
            if(Input.GetKey(KeyCode.S))
            {
                Physics2D.IgnoreLayerCollision(10, 13, true);
                Invoke("IgnoreLayerCollisionOff",0.5f);
            }
        }
        private void IgnoreLayerCollisionOff()
        {
            Physics2D.IgnoreLayerCollision(10, 13, false);
        }
    }
}

