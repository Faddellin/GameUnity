using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;



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
        public bool isJumping;
        public int extraJumps;
        private bool jumpDelay;
        public float fallingGravityScale;
        public float commonGravityScale;

        [Header("Player Interface")]
        public float health;
        public int heartsNum;
        public bool damaged;

        public Image[] hearts;
        public Sprite fullHeart;
        public Sprite emptyHeart;

        public int maxShurikenAmount;
        public int currentShurikenAmount;

        public Image[] shurikens;
        public Sprite fullShuriken;
        public Sprite emptyShuriken;



        private bool isDeath;


        [Header("Attack")]
        public PlayerAttack playeratak;
        public Animator swordTrail;
        public int attackCounter;
        public bool isThrowing;

        public Animator animator;
 
        public GameObject cameraFollowGo;

        [Header("Main Settings")]
        public  Rigidbody2D rb;
        public  SpriteRenderer sr;

        [Header("Cameras")]
        private FollowingCameraObject followingCamera;
        private float _fallSpeedYDampingChangeThreshold;
        public CinemachineImpulseSource BossimpulseSource;
        public CinemachineImpulseSource DefaultimpulseSource;

        [Header("Arms")]
        public Transform _playerArms;
        public Transform _attackArea;
        private Transform _shurikenSpawnPoint;

        [Header("Dash")]
        private float lastImageXpos;
        public bool canDash;
        public float dashPower = 24f;
        public float dashingTime = 0.2f;
        public float dashCooldown = 1f;
        public int dashesAmount = 1;
        public float distanceBetweenImages;

        [Header("WallJump")]
        public bool onWall;
        public bool canWallJump;
        public float wallJumpCooldown;
        public Transform wallCheckUp;
        public Transform wallCheckDown;
        public Vector2 boxSize;

        [Header("OnGround/Wall")]
        public bool onGround;
        public float slideSpeed;
        public Transform GroundCheck;

        public Vector2 size = new Vector2(2.0f, 1.0f);
        public float angle = 0.0f;

        public LayerMask Ground;
        public LayerMask Wall;

        [Header("Particles")]
        public ParticleSystem wallSlideParticle;
        public ParticleSystem extraJumpParticle;
        public ParticleSystem blood;

        [Header("Sounds")]
        public GameObject dashSound;
        private AudioSource dashAudio;

        public GameObject throwShuriken;
        public AudioSource shurikenSound;

        public GameObject shurikenFill;
        private AudioSource shurikenFillSound;

        public GameObject katana;
        public AudioSource katanaSwingAir;

        public GameObject extraJumpSound;
        private AudioSource extraJumpAudio;

        public AudioSource playerAudio;

        void Start()
        {
            playerAudio = GetComponent<AudioSource>();

            dashAudio = dashSound.GetComponent<AudioSource>();

            extraJumpAudio = extraJumpSound.GetComponent<AudioSource>();

            shurikenSound = throwShuriken.GetComponent<AudioSource>();

            shurikenFillSound = shurikenFill.GetComponent<AudioSource>();

            katanaSwingAir = katana.GetComponent<AudioSource>(); 

            boxSize = wallCheckUp.GetComponent<BoxCollider2D>().size;

            playeratak = gameObject.GetComponent<PlayerAttack>();

            _shurikenSpawnPoint = transform.Find("WeaponAxis");

            IsFacingRight = true;

            IsFalling = false;

            isJumping = false;

            IsMoving = true;

            canDash = true;

            jumpDelay = false;

            isThrowing = false;

            damaged = false;

            isDeath = false;

            animator = GetComponent<Animator>();

            rb = GetComponent<Rigidbody2D>();

            sr = GetComponent<SpriteRenderer>();

            followingCamera = cameraFollowGo.GetComponent<FollowingCameraObject>();

            _fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;

            wallSlideParticle.Stop();
            extraJumpParticle.Stop();

            currentShurikenAmount = maxShurikenAmount;

        }

        private void FixedUpdate()
        {
            CheckingWall();
        }
        void Update()
        {
            CheckingGround();
            DashActivation();
            CheckDash();
            Strafe();
            Jump();
            GoDown();
            ChangeCameraPos();
            WallSlide();


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
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && playeratak.canAttack && !isThrowing && !damaged)
            {
                movement = Input.GetAxis("Horizontal");
            }
            else
            {
                movement = 0.0f;
            }

            if (IsMoving && !damaged)
            {
                
                rb.velocity = new Vector2(movement * speed, rb.velocity.y);
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

            wallSlideParticle.transform.localPosition = new Vector3(-wallSlideParticle.transform.localPosition.x, wallSlideParticle.transform.localPosition.y, wallSlideParticle.transform.localPosition.z);

            _shurikenSpawnPoint.transform.localPosition = new Vector3(-_shurikenSpawnPoint.transform.localPosition.x, _shurikenSpawnPoint.transform.localPosition.y, _shurikenSpawnPoint.transform.localPosition.z);
            _playerArms.transform.localPosition = new Vector3(-_playerArms.transform.localPosition.x, _playerArms.transform.localPosition.y, _playerArms.transform.localPosition.z);
            _attackArea.transform.localPosition = new Vector3(-_attackArea.transform.localPosition.x, _attackArea.transform.localPosition.y, _attackArea.transform.localPosition.z);

            IsFacingRight = !IsFacingRight;
            animator.SetBool("IsRight", IsFacingRight);
            followingCamera.CallTurn();
        }


        public void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0 && !onGround && !onWall && !damaged)
            {
                rb.AddForce(new Vector2(0, jumpForce - rb.velocity.y*rb.mass), ForceMode2D.Impulse);
                extraJumpAudio.Play();

                extraJumpParticle.Play();

                isJumping = true;
                animator.SetBool("Jump", isJumping);
                extraJumps--;
            }
            else if((Input.GetKeyDown(KeyCode.Space)||jumpDelay) && onGround && !damaged)
            {
                rb.AddForce(new Vector2(0, jumpForce - rb.velocity.y * rb.mass), ForceMode2D.Impulse);
                isJumping = true;
                animator.SetBool("Jump", isJumping);
                jumpDelay = false;
            }

            else if (!onGround && onWall && Input.GetKeyDown(KeyCode.Space) && canWallJump && !damaged)
            {

                    StartCoroutine(WallJump());
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
            if (!onGround)
            {
                Falling();
            }
            else if (onGround)
            {
                IsFalling = false;
                extraJumps = 1;
                dashesAmount = 1;
                animator.SetBool("IsFalling", IsFalling);
            }
        }

        public void Damage(float damage, bool direction, AudioSource damageSound)
        {
            Vector3 addPosHigh = new Vector3(0f, 2.5f, 0f);
            ParticleSystem bloodParticles = Instantiate(blood, transform.position + addPosHigh, transform.rotation);
            ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = blood.velocityOverLifetime;

            if (!damaged)
            {
                damaged = true;

                damageSound.Play();

                if (direction == IsFacingRight) {
                    Flip();
                }
                animator.SetBool("IsDamaged", damaged);

                if(direction == true)
                {
                    velocityOverLifetime.x = 3;
                }
                else
                {
                    velocityOverLifetime.x = -3;
                }

                bloodParticles.Play();

                if(damage == 2)
                {
                    BossimpulseSource.GenerateImpulse();
                }
                else
                {
                    DefaultimpulseSource.GenerateImpulse();
                }

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
                    isDeath = true;
                    animator.SetBool("IsDeath", isDeath);
                }
            }
        }
        public void FillShurikens()
        {
            currentShurikenAmount = maxShurikenAmount;
            shurikenFillSound.Play();
            for (int i = 0; i < shurikens.Length; i++)
            {
               shurikens[i].enabled = true;
               shurikens[i].sprite = fullShuriken;
            }
        }

        public void Die()
        {
            StartCoroutine(CallDieMenu());
        }

        private IEnumerator CallDieMenu()
        {
            yield return new WaitForSeconds(0.5f);
            LevelManager.instance.GameOver();
            gameObject.SetActive(false);

        }

        public void DashActivation()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && dashesAmount > 0 && !damaged)
            {
                StartCoroutine(Dash());
            }
        }

        public Vector2 FindDirection()
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return mousePos;
        }

        private IEnumerator Dash()
        {
            canDash = false;
            IsMoving = false;
            animator.SetBool("IsDashing", true);

            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;


            Vector2 direction = FindDirection();
            Vector2 nonNormilizeDir = direction;
            direction -= (Vector2)transform.position;
            direction.Normalize();

            if (nonNormilizeDir.x - transform.position.x >= 0 && !IsFacingRight)
            {
                Flip();
            }
            else if (nonNormilizeDir.x - transform.position.x < 0 && IsFacingRight)
            {
                Flip();
            }

            float absolutePower = dashPower  * (0.5f + Math.Abs(direction.x) * 0.5f);
            dashAudio.Play();

            AfterImagePool.instance.GetFromPool();
            lastImageXpos = transform.position.x;

            dashesAmount--;

            rb.velocity = direction * absolutePower;

            yield return new WaitForSeconds(dashingTime);

            rb.gravityScale = originalGravity;
            IsMoving = true;
            animator.SetBool("IsDashing", false);

            Physics2D.IgnoreLayerCollision(7, 10, false);

            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }

        private void CheckDash()
        {
            if (!canDash)
            {
                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    AfterImagePool.instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }
        }

        public void CheckingWall()
        {
            onWall = (Physics2D.OverlapBox(wallCheckUp.position, boxSize, angle, Wall) && Physics2D.OverlapBox(wallCheckDown.position, boxSize, angle, Wall));
            animator.SetBool("OnWall", onWall);
        }

        private IEnumerator WallJump()
        {
            canWallJump = false;
            IsMoving = false;
            onWall = false;

            float KirillKoef = 1.5f;

            Vector2 direction = FindDirection();
            direction -= (Vector2)transform.position;
            direction.Normalize();

            rb.velocity = new Vector2(0, 0);
            rb.AddForce(new Vector2(jumpForce * KirillKoef, jumpForce) * direction,
               ForceMode2D.Impulse);

            Flip();

            canWallJump = true;
            extraJumps = 1;
            dashesAmount = 1;
            yield return new WaitForSeconds(0.18f);
            IsMoving = true;

        }

        public void WallSlide()
        {
            if(onWall && !onGround && !damaged)
            {
                if (extraJumps == 0)
                {
                    extraJumps = 1;
                }
                wallSlideParticle.Play();
                rb.gravityScale = 0;
                rb.velocity = new Vector2(rb.velocity.x,-slideSpeed);
            }
            else
            {
                wallSlideParticle.Stop();
                rb.gravityScale = commonGravityScale;
            }
        }

        public void GoDown()
        {
            if(Input.GetKey(KeyCode.S) && !damaged)
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

