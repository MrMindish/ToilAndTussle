using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
namespace AB
{
    public class PlayerMovement : MonoBehaviour
    {
        PlayerAttackManager playerAttackManager;
        PlayerHurtboxManager hurtboxManager;
        PlayerShield playerShield;
        PlayerInput playerInput;
        PlayerHealth playerHealth;

        RoundManager roundManager;


        //Handles all of the movement speed and such
        public Vector2 horizontal;

        public float moveSpeed;
        public float slowedMoveSpeed;
        public float jumpingPower;

        public float pushForce;

        //Once the player has landed after being juggled, they can recover/jump backwards
        public bool canRecover;
        public float recoveryTime = 2;
        public float recoveryForce = 3;
        public bool isRecovering;

        //Used for the movement code to understand who the target is
        public GameObject playerOneX;
        public GameObject playerTwoX;

        //Makes sure the player is facing towards the enemy
        public bool isFacingRight = true;

        //Allows the jump animation to play
        public bool isJumping;
        public float jumpTimer = 0.4f;

        //Allows the Dash to play
        public bool canDash;
        public bool isDashing;
        private float dashingPower;
        private float dashingTime = 0.2f;
        private float dashingCooldown = 0.05f;


        public bool isBlocking;
        public bool canParry;

        [SerializeField] private Rigidbody rb;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform orientation;

        private void Awake()
        {
            playerInput = GetComponentInChildren<PlayerInput>();

            playerAttackManager = GetComponentInChildren<PlayerAttackManager>();
            hurtboxManager = GetComponentInChildren<PlayerHurtboxManager>();

            playerShield = GetComponent<PlayerShield>();
            playerHealth = GetComponent<PlayerHealth>();

            roundManager = GetComponentInParent<RoundManager>();
        } //Includes the Component Getting

        private void Start()
        {
            canRecover = false;
            isRecovering = false;
            isJumping = false;
            canDash = true;
        } //Sets various bools to false

        void Update()
        {

            if ((!playerHealth.p1Dead && !playerHealth.p2Dead) && roundManager.roundStarted)
            {
                if (gameObject.tag == "Player1")
                {
                    horizontal = playerInput.actions["Move"].ReadValue<Vector2>();
                }

                else if(gameObject.tag == "Player2")
                {
                    horizontal = playerInput.actions["Move"].ReadValue<Vector2>();
                }


                if (playerInput.actions["Jump"].WasPressedThisFrame() && IsGrounded() && playerAttackManager.isAttacking == false && playerAttackManager.isCrouching == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && playerShield.shieldBreak == false && !isDashing)
                {
                    //if the jump is inputed while the fighter is grounded, not attacking or being attacked, or crouching, then the jump is performed
                    rb.velocity = new Vector3(rb.velocity.x, jumpingPower, 0);
                    jumpTimer = 0.2f;
                }

                if (playerInput.actions["Jump"].WasReleasedThisFrame() && rb.velocity.y > 0f)
                {
                    //Allows the jump to be cancelled halfway, allowing short hops to happen
                    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, 0f);
                }

                if (isDashing)
                {
                    return;
                }


            }
            IsGrounded();
            Flip();
            BlockingRange();

            if (jumpTimer > 0 && !IsGrounded())
            {
                jumpTimer -= Time.deltaTime;
                isJumping = true;
            }
            else if(jumpTimer <= 0 && IsGrounded())
            {
                jumpTimer = 0.4f;
                isJumping = false;
            }
            else if (IsGrounded())
            {
                isJumping = false;
            }

        } //Includes the Jumping Code

        private void FixedUpdate()
        {
            if (!playerHealth.p1Dead && !playerHealth.p2Dead && roundManager.roundStarted)
            {
                if (isDashing)
                {
                    return;
                }

                horizontal = playerInput.actions["Move"].ReadValue<Vector2>();
                //if the player's on the ground, isn't attacking, isn't crouching, isn't stunned, isn't dashing and isn't blocking, they move normally
                if (IsGrounded() && playerAttackManager.isAttacking == false && !playerAttackManager.isCrouching && horizontal.y >= -0.4 && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && !isBlocking && !isDashing && !playerShield.shieldBreak && (!playerHealth.p1Dead || !playerHealth.p2Dead))
                {
                    //movement part
                    rb.velocity = new Vector3(horizontal.x * moveSpeed, rb.velocity.y, 0f);
                    playerAttackManager.isWalking = true;

                }

                //Holds the player in place if they're blocking
                else if (IsGrounded() && playerAttackManager.isAttacking == false && playerAttackManager.isCrouching == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isBlocking && !isDashing)
                {
                    //Blocking movement
                    rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                }

                else if (hurtboxManager.isStunned && hurtboxManager.isKnockback && IsGrounded())
                {
                    //Allows the player to get knocked backwards
                    Debug.Log("is stunned");
                    rb.velocity = new Vector3(hurtboxManager.hKnockback, hurtboxManager.vKnockback, 0f);
                }
                else if (hurtboxManager.isStunned && !hurtboxManager.isKnockback && IsGrounded())
                {
                    //Prevents the knockback from lasting throughout the entirety of the hitstun
                    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0f);
                }

                else if (hurtboxManager.isStunned && hurtboxManager.isKnockback && hurtboxManager.isLaunched)
                {
                    //Uses different knockback floats while in the air
                    Debug.Log("is stunned");
                    rb.velocity = new Vector3(hurtboxManager.hAirKnockback, hurtboxManager.vAirKnockback, 0f);

                    //Since gravity will guide the juggled fighter, an else if for knockback duration is not necessary
                }

                if (IsGrounded() && playerInput.actions["Dash"].WasPerformedThisFrame() && !playerAttackManager.isAttacking && !hurtboxManager.isStunned && !canRecover && !playerAttackManager.isCrouching && canDash)
                {
                    StartCoroutine(Dash());

                    /*
                    if (Time.time - lastMovePressTime <= doublePressTimeThreshold)
                    {
                        // Double press detected
                        Debug.Log("Double press detected!");

                        // Reset variables for next detection
                        movePressedOnce = false;
                        lastMovePressTime = 1f;
                    }
                    else
                    {
                        // First press detected
                        movePressedOnce = true;
                        lastMovePressTime = Time.time;
                    }
                    */
                }


                if (canRecover)
                {
                    //If the player has been knocked down, they're made invincible until they get back up
                    hurtboxManager.isInvincible = true;
                    recoveryTime -= Time.deltaTime;

                    if (playerInput.actions["Jump"].WasPressedThisFrame() && recoveryTime <= 0.4 && recoveryTime > 0 && isRecovering == false)
                    {
                        //Allows the player to "Recover" by rolling away from the enemy
                        isRecovering = true;
                        recoveryTime = 0.4f;
                    }

                    if (recoveryTime <= 0)
                    {
                        hurtboxManager.isLaunched = false;
                        hurtboxManager.isInvincible = false;
                        canRecover = false;
                        isRecovering = false;
                    }
                }
                else if (!canRecover)
                {
                    recoveryTime = 0.5f;
                }
                if (isRecovering)
                {
                    rb.velocity = new Vector3(recoveryForce, 0f, 0f);
                }

                if (hurtboxManager.canReset && !IsGrounded())
                {
                    hurtboxManager.isInvincible = true;
                    hurtboxManager.isLaunched = false;
                    rb.velocity = new Vector3(hurtboxManager.hAirKnockback, -3, 0f);

                }
                if (hurtboxManager.canReset && IsGrounded())
                {
                    hurtboxManager.canReset = false;
                    hurtboxManager.isInvincible = false;
                    hurtboxManager.isStunned = false;
                    hurtboxManager.isShieldStunned = false;
                }
            }
        }


        public bool IsGrounded()
        {
            return Physics.Raycast(orientation.position, Vector3.down, 0.01f);
        }

        private void Flip()
        {
            //Flips the player to face the enemy
            if (gameObject.tag == "Player1" && IsGrounded() && transform.position.x > playerTwoX.transform.position.x && isFacingRight == false && playerAttackManager.isAttacking == false)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            else if (gameObject.CompareTag("Player1") && IsGrounded() && transform.position.x < playerTwoX.transform.position.x && isFacingRight == true && playerAttackManager.isAttacking == false)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            if (gameObject.tag == "Player2" && IsGrounded() && transform.position.x > playerOneX.transform.position.x && isFacingRight == false && playerAttackManager.isAttacking == false)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            else if (gameObject.tag == "Player2" && IsGrounded() && transform.position.x < playerOneX.transform.position.x && isFacingRight == true && playerAttackManager.isAttacking == false)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            //Makes sure the knockback sends the player away from the attacker
            if(gameObject.tag == "Player1" && transform.position.x < playerTwoX.transform.position.x && hurtboxManager.hKnockback > 0f)
            {
                hurtboxManager.hKnockback *= -1;
                hurtboxManager.hAirKnockback *= -1;
            }
            else if (gameObject.tag == "Player1" && transform.position.x > playerTwoX.transform.position.x && hurtboxManager.hKnockback < 0f)
            {
                hurtboxManager.hKnockback *= -1;
                hurtboxManager.hAirKnockback *= -1;
            }

            if (gameObject.tag == "Player2" && transform.position.x < playerOneX.transform.position.x && hurtboxManager.hKnockback > 0f)
            {
                hurtboxManager.hKnockback *= -1;
                hurtboxManager.hAirKnockback *= -1;
            }
            else if (gameObject.tag == "Player2" && transform.position.x > playerOneX.transform.position.x && hurtboxManager.hKnockback < 0f)
            {
                hurtboxManager.hKnockback *= -1;
                hurtboxManager.hAirKnockback *= -1;
            }

            //Makes the recovery travel away from the other player
            if (gameObject.tag == "Player1" && transform.position.x < playerTwoX.transform.position.x && isRecovering && recoveryForce > 0)
            {
                recoveryForce *= -1;
            }
            else if (gameObject.tag == "Player1" && transform.position.x > playerTwoX.transform.position.x && isRecovering && recoveryForce < 0)
            {
                recoveryForce *= -1;
            }

            if (gameObject.tag == "Player2" && transform.position.x < playerOneX.transform.position.x && isRecovering && recoveryForce > 0)
            {
                recoveryForce *= -1;
            }
            else if (gameObject.tag == "Player2" && transform.position.x > playerOneX.transform.position.x && isRecovering && recoveryForce < 0)
            {
                recoveryForce *= -1;
            }

            //Makes the dash go in the inputed direction
            if (gameObject.tag == "Player1" && transform.position.x < playerTwoX.transform.position.x && horizontal.x < 0)
            {
                //Sends Player 1 Right when on the Left side
                dashingPower = 15f;
            }
            else if (gameObject.tag == "Player1" && transform.position.x < playerTwoX.transform.position.x && horizontal.x > 0)
            {
                //Sends Player 1 Left when on the Left side
                dashingPower = -30f;
            }
            else if (gameObject.tag == "Player1" && transform.position.x > playerTwoX.transform.position.x && horizontal.x < 0)
            {
                //Sends Player 1 Left when on the Right side
                dashingPower = -30f;
            }
            else if (gameObject.tag == "Player1" && transform.position.x > playerTwoX.transform.position.x && horizontal.x > 0)
            {
                //Sends Player 1 Right when on the Right side
                dashingPower = 15f;
            }

            if (gameObject.tag == "Player2" && transform.position.x < playerOneX.transform.position.x && horizontal.x < 0)
            {
                //Sends Player 2 Right when on the Left side
                dashingPower = 15f;
            }
            else if (gameObject.tag == "Player2" && transform.position.x < playerOneX.transform.position.x && horizontal.x > 0)
            {
                //Sends Player 2 Left when on the Left side
                dashingPower = -30f;
            }
            else if (gameObject.tag == "Player2" && transform.position.x > playerOneX.transform.position.x && horizontal.x < 0)
            {
                //Sends Player 2 Left when on the Right side
                dashingPower = -30f;
            }
            else if (gameObject.tag == "Player2" && transform.position.x > playerOneX.transform.position.x && horizontal.x > 0)
            {
                //Sends Player 2 Right when on the Right side
                dashingPower = 15f;
            }
        }

        private void BlockingRange()
        {
            //Controls the Block Function for Player 1
            if (gameObject.tag == "Player1" && IsGrounded() && transform.position.x > playerTwoX.transform.position.x && playerAttackManager.isAttacking == false && !hurtboxManager.isStunned && hurtboxManager.isBlockable && (Input.GetKey(KeyCode.RightArrow) || horizontal.x > 0))
            {
                isBlocking = true;
            }
            else if (gameObject.tag == "Player1" && IsGrounded() && transform.position.x < playerTwoX.transform.position.x && playerAttackManager.isAttacking == false && !hurtboxManager.isStunned && hurtboxManager.isBlockable && (Input.GetKey(KeyCode.LeftArrow) || horizontal.x < 0))
            {
                isBlocking = true;
            }
            else
            {
                isBlocking = false;
            }

            //Controls the Parry for Player 1
            if (gameObject.tag == "Player1" && IsGrounded() && transform.position.x > playerTwoX.transform.position.x && playerAttackManager.isAttacking == false && !hurtboxManager.isStunned && hurtboxManager.isBlockable && (Input.GetKeyDown(KeyCode.RightArrow) || (horizontal.x > 0 && playerInput.actions["Parry"].WasPressedThisFrame())))
            {
                canParry = true;
            }
            else if (gameObject.tag == "Player1" && IsGrounded() && transform.position.x < playerTwoX.transform.position.x && playerAttackManager.isAttacking == false && !hurtboxManager.isStunned && hurtboxManager.isBlockable && (Input.GetKeyDown(KeyCode.LeftArrow) || (horizontal.x < 0 && playerInput.actions["Parry"].WasPressedThisFrame())))
            {
                canParry = true;
            }
            else
            {
                canParry = false;
            }

            //Controls the Block function for Player 2
            if (gameObject.tag == "Player2" && IsGrounded() && transform.position.x > playerOneX.transform.position.x && playerAttackManager.isAttacking == false && !hurtboxManager.isStunned && hurtboxManager.isBlockable && (Input.GetKey(KeyCode.D) || horizontal.x > 0))
            {
                isBlocking = true;
            }
            else if (gameObject.tag == "Player2" && IsGrounded() && transform.position.x < playerOneX.transform.position.x && playerAttackManager.isAttacking == false && !hurtboxManager.isStunned && hurtboxManager.isBlockable && (Input.GetKey(KeyCode.A) || horizontal.x < 0))
            {
                isBlocking = true;
            }

            //Controls the parry for Player 2
            if (gameObject.tag == "Player2" && IsGrounded() && transform.position.x > playerOneX.transform.position.x && playerAttackManager.isAttacking == false && !hurtboxManager.isStunned && hurtboxManager.isBlockable && (Input.GetKeyDown(KeyCode.D) || (horizontal.x > 0 && playerInput.actions["Parry"].WasPressedThisFrame())))
            {
                canParry = true;
            }
            else if (gameObject.tag == "Player2" && IsGrounded() && transform.position.x < playerOneX.transform.position.x && playerAttackManager.isAttacking == false && !hurtboxManager.isStunned && hurtboxManager.isBlockable && (Input.GetKeyDown(KeyCode.A) || (horizontal.x < 0 && playerInput.actions["Parry"].WasPressedThisFrame())))
            {
                canParry = true;
            }
            else
            {
                canParry = false;
            }
        }

        private IEnumerator Dash()
        {
            canDash = false;
            isDashing = true;
            rb.velocity = new Vector3(transform.localScale.x * dashingPower, 0f, 0f);
            rb.useGravity = false;
            yield return new WaitForSeconds(dashingTime);
            isDashing = false;
            rb.useGravity = true;
            yield return new WaitForSeconds(dashingCooldown);
            canDash = true;
        }
    }
}