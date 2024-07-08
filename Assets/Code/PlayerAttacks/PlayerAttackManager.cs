using AB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AB
{
    public class PlayerAttackManager : MonoBehaviour
    {
        public KeyCode lightInput;
        public KeyCode heavyInput;
        public KeyCode specialInput;
        public KeyCode blockInput;

        //Calculates how long the player attacks for
        public bool isAttacking;
        public bool isAirAttacking;
        public float attackCooldown;

        //Calculates what direction the attacker moves if they land the attack (This is to prevent corner trapping)
        public float startPosition;
        public float endPosition;

        //Calls upon the animation and animator windows
        private Animation anim;
        private Animator animator;

        PlayerMovement playerMovement;
        PlayerHurtboxManager hurtboxManager;
        PlayerAttackingHitboxes playerAttackingHitboxes;
        TimeManager timeManager;
        PlayerShield playerShield;
        PlayerInput playerInput;
        MaterialManager materialManager;
        PlayerHealth playerHealth;
        RoundManager roundManager;

        //Registers the directional inputs
        public bool isRightPressed;
        public bool isLeftPressed;
        public bool isDownPressed;
        public bool isNothingPressed;

        //Used for the crouch input
        public bool isCrouching = false;

        //Used for Cancelling Moves into Specials
        public bool isCancel = false;

        //Used for the Walk Animation
        public bool isWalking;


        private void Awake()
        {
            anim = GetComponent<Animation>();
            animator = GetComponent<Animator>();
            playerInput = GetComponent<PlayerInput>();

            playerMovement = GetComponentInParent<PlayerMovement>();
            hurtboxManager = GetComponentInChildren<PlayerHurtboxManager>();
            playerAttackingHitboxes = GetComponentInChildren<PlayerAttackingHitboxes>();

            timeManager = GetComponentInParent<TimeManager>();
            materialManager = GetComponentInParent<MaterialManager>();
            playerShield = GetComponentInParent<PlayerShield>();
            playerHealth = GetComponentInParent<PlayerHealth>();

            roundManager = GetComponentInParent<RoundManager>();

            isAttacking = false;
            isAirAttacking = false;
            isWalking = false;
        }

        private void Update()
        {

            if (!playerHealth.p1Dead && !playerHealth.p2Dead)
            {
                animator.SetBool("isDead", false);
                animator.ResetTrigger("isWin");

                //Cancels the Walk animation
                if (animator.GetBool("isWalkingF") == true || animator.GetBool("isWalkingB") == true)
                {
                    if ((isAttacking == true || playerMovement.isBlocking || playerMovement.isJumping || hurtboxManager.isStunned))
                    {
                        animator.SetTrigger("actionPerformed");
                    }
                    else
                    {
                        animator.ResetTrigger("actionPerformed");
                    }
                }


                //Performs the Jump Animation
                if (playerMovement.isJumping && !hurtboxManager.isStunned)
                {
                    animator.SetTrigger("jumpStart");
                    animator.ResetTrigger("jumpEnd");
                }
                if (!playerMovement.isJumping && !hurtboxManager.isStunned)
                {
                    animator.ResetTrigger("jumpStart");
                    animator.SetTrigger("jumpEnd");
                }
                else if (playerMovement.isJumping && playerMovement.IsGrounded())
                {
                    animator.ResetTrigger("jumpStart");
                    animator.SetTrigger("jumpEnd");
                }

                //Performs the Walk Animation
                if (isWalking && playerMovement.horizontal.x > 0 && playerMovement.horizontal.y > -0.4 && playerMovement.isFacingRight)
                {
                    animator.SetBool("isWalkingF", false);
                    animator.SetBool("isWalkingB", true);
                }
                else if (isWalking && playerMovement.horizontal.x < 0 && playerMovement.horizontal.y > -0.4 && playerMovement.isFacingRight)
                {
                    animator.SetBool("isWalkingF", true);
                    animator.SetBool("isWalkingB", false);
                }
                else if (isWalking && playerMovement.horizontal.x > 0 && playerMovement.horizontal.y > -0.4 && !playerMovement.isFacingRight)
                {
                    animator.SetBool("isWalkingF", true);
                    animator.SetBool("isWalkingB", false);
                }
                else if (isWalking && playerMovement.horizontal.x < 0 && playerMovement.horizontal.y > -0.4 && !playerMovement.isFacingRight)
                {
                    animator.SetBool("isWalkingF", false);
                    animator.SetBool("isWalkingB", true);
                }
                else
                {
                    animator.SetBool("isWalkingF", false);
                    animator.SetBool("isWalkingB", false);
                    isWalking = false;
                }

                //Performs the Dash Animation


                //Performs the Blocking Animation
                if (playerMovement.isBlocking && !hurtboxManager.isStunned)
                {
                    animator.SetBool("isBlocking", true);
                }
                else
                {
                    animator.SetBool("isBlocking", false);
                }

                //Performs the Parry Animation
                if (hurtboxManager.parried)
                {
                    animator.SetBool("isParried", true);
                }

                //Performs the Shield Break and Stun Animation
                if (playerShield.shieldBreak)
                {
                    animator.SetBool("shieldBreakStun", true);
                }
                else if (!playerShield.shieldBreak)
                {
                    animator.SetBool("shieldBreakStun", false);
                }

                //Performs the Light Null Attack            LN
                if (playerInput.actions["Light Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isNothingPressed && !playerMovement.isDashing)
                {
                    animator.SetTrigger("isLNAttacking");
                    isAttacking = true;
                    attackCooldown = 0.26f;
                }

                //Performs the Light Forward Attack         LF
                if (playerInput.actions["Light Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x > playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isLeftPressed && !isCrouching && !playerMovement.isDashing)
                {
                    //On the Left Side of the Opponent
                    animator.SetTrigger("isLFAttacking");
                    isAttacking = true;
                    attackCooldown = 0.32f;
                }
                else if (playerInput.actions["Light Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x < playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isRightPressed && !isCrouching && !playerMovement.isDashing )
                {
                    //On the Right Side of the Opponent
                    animator.SetTrigger("isLFAttacking");
                    isAttacking = true;
                    attackCooldown = 0.32f;
                }

                //Performs the Light Backwards Attack       LB
                if (playerInput.actions["Light Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x < playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isLeftPressed && !isCrouching && !playerMovement.isDashing)
                {
                    //On the Right Side of the Opponent
                    animator.SetTrigger("isLBAttacking");
                    isAttacking = true;
                    attackCooldown = 0.36f;
                }
                else if (playerInput.actions["Light Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x > playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isRightPressed && !isCrouching && !playerMovement.isDashing)
                {
                    //On the Left Side of the Opponent
                    animator.SetTrigger("isLBAttacking");
                    isAttacking = true;
                    attackCooldown = 0.36f;
                }

                //Performs the Light Crouch Attack          LC
                if (playerInput.actions["Light Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isDownPressed && isCrouching && !playerMovement.isDashing)
                {
                    animator.SetTrigger("isLCAttacking");
                    isAttacking = true;
                    attackCooldown = 0.5f;
                    animator.SetBool("canCancel", true);
                }

                //Performs the Light Aerial Attack          LA
                if (playerInput.actions["Light Attack"].WasPressedThisFrame() && !playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && !playerMovement.isDashing)
                {
                    animator.SetTrigger("isLAAttacking");
                    isAttacking = true;
                    isAirAttacking = true;
                    attackCooldown = 0.1f;
                }




                //Performs the Heavy Null Attack            HN
                if (playerInput.actions["Heavy Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isNothingPressed && !playerMovement.isDashing)
                {
                    animator.SetTrigger("isHNAttacking");
                    isAttacking = true;
                    attackCooldown = 0.4f;
                    animator.SetBool("canCancel", true);
                }

                //Performs the Heavy Forward Attack         HF
                if (playerInput.actions["Heavy Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x > playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isLeftPressed && !isCrouching && !playerMovement.isDashing)
                {
                    //On the Left Side of the Opponent
                    animator.SetTrigger("isHFAttacking");
                    isAttacking = true;
                    attackCooldown = 0.8f;
                }
                else if (playerInput.actions["Heavy Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x < playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isRightPressed && !isCrouching && !playerMovement.isDashing)
                {
                    //On the Right Side of the Opponent
                    animator.SetTrigger("isHFAttacking");
                    isAttacking = true;
                    attackCooldown = 0.8f;
                }

                //Performs the Heavy Backwards Attack       HB
                if (playerInput.actions["Heavy Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x < playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isLeftPressed && !isCrouching && !playerMovement.isDashing)
                {
                    //On the Right Side of the Opponent
                    animator.SetTrigger("isHBAttacking");
                    isAttacking = true;
                    attackCooldown = 1f;
                    animator.SetBool("canCancel", true);
                }
                else if (playerInput.actions["Heavy Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x > playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isRightPressed && !isCrouching && !playerMovement.isDashing)
                {
                    //On the Left Side of the Opponent
                    animator.SetTrigger("isHBAttacking");
                    isAttacking = true;
                    attackCooldown = 1f;
                    animator.SetBool("canCancel", true);
                }

                //Performs the Heavy Crouch Attack          HC
                if (playerInput.actions["Heavy Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isDownPressed && isCrouching && !playerMovement.isDashing)
                {
                    animator.SetTrigger("isHCAttacking");
                    isAttacking = true;
                    attackCooldown = 0.7f;
                }

                //Performs the Heavy Aerial Attack
                if (playerInput.actions["Heavy Attack"].WasPressedThisFrame() && !playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && !playerMovement.isDashing)
                {
                    Debug.Log("HA");
                    animator.SetTrigger("isHAAttacking");
                    isAttacking = true;
                    isAirAttacking = true;
                    attackCooldown = 0.1f;
                }


                //Performs the Special Null Attack
                if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isNothingPressed && !playerMovement.isDashing)
                {
                    animator.SetTrigger("isSN1Attacking");
                    isAttacking = true;
                    attackCooldown = 0.4f;
                }
                else if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && isAttacking == true && hurtboxManager.isStunned == false && isNothingPressed && hurtboxManager.isShieldStunned == false && isCancel && !playerMovement.isDashing)
                {
                    animator.SetTrigger("isSN1Attacking");
                    isAttacking = true;
                    attackCooldown = 0.4f;
                    animator.ResetTrigger("isCrouchingEnd");
                }

                //Performs the Special Forward Attack
                if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x > playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isLeftPressed && !playerMovement.isDashing)
                {
                    //On the Left Side of the opponent
                    animator.SetTrigger("isSF1Attacking");
                    isAttacking = true;
                    attackCooldown = 0.7f;
                }
                else if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x > playerMovement.playerTwoX.transform.position.x && isAttacking == true && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isLeftPressed && isCancel && !playerMovement.isDashing)
                {
                    //On Left Side + Cancelled the move
                    animator.SetTrigger("isSF1Attacking");
                    isAttacking = true;
                    attackCooldown = 0.7f;
                    animator.ResetTrigger("isCrouchingEnd");
                }
                else if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x < playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isRightPressed && !playerMovement.isDashing)
                {
                    //On the Right Side of the opponent
                    animator.SetTrigger("isSF1Attacking");
                    isAttacking = true;
                    attackCooldown = 0.7f;
                }
                else if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x < playerMovement.playerTwoX.transform.position.x && isAttacking == true && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isRightPressed && isCancel && !playerMovement.isDashing)
                {
                    //On Right Side + Cancelled the move
                    animator.SetTrigger("isSF1Attacking");
                    isAttacking = true;
                    attackCooldown = 0.7f;
                    animator.ResetTrigger("isCrouchingEnd");
                }

                //Performs the Special Backward Attack
                if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x < playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isLeftPressed && !playerMovement.isDashing)
                {
                    //On the Left Side of the opponent
                    animator.SetTrigger("isSB1Attacking");
                    isAttacking = true;
                    attackCooldown = 0.26f;
                }
                else if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x < playerMovement.playerTwoX.transform.position.x && isAttacking == true && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isLeftPressed && isCancel && !playerMovement.isDashing)
                {
                    //On Left Side + Cancelled the move
                    animator.SetTrigger("isSB1Attacking");
                    isAttacking = true;
                    attackCooldown = 0.26f;
                    animator.ResetTrigger("isCrouchingEnd");
                }
                else if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x > playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isRightPressed && !playerMovement.isDashing)
                {
                    //On the Right Side of the opponent
                    animator.SetTrigger("isSB1Attacking");
                    isAttacking = true;
                    attackCooldown = 0.26f;
                }
                else if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x > playerMovement.playerTwoX.transform.position.x && isAttacking == true && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isRightPressed && isCancel && !playerMovement.isDashing)
                {
                    //On Right Side + Cancelled the move
                    animator.SetTrigger("isSB1Attacking");
                    isAttacking = true;
                    attackCooldown = 0.26f;
                    animator.ResetTrigger("isCrouchingEnd");
                }

                //Performs the Special Down Attack
                if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && (isDownPressed || isCrouching) && !playerMovement.isDashing)
                {
                    animator.SetTrigger("isSC1Attacking");
                    isAttacking = true;
                    attackCooldown = 0.24f;
                }
                else if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && isAttacking == true && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && (isDownPressed || isCrouching) && isCancel && !playerMovement.isDashing)
                {
                    animator.SetTrigger("isSC1Attacking");
                    isAttacking = true;
                    attackCooldown = 0.24f;
                    animator.ResetTrigger("isCrouchingEnd");
                }


                //Triggers the Hitstun Animations
                if (hurtboxManager.isStunned)
                {
                    animator.SetBool("isStunned", true);
                    Debug.Log("isSTOONED");
                    if(hurtboxManager.hitAnimInfo == 1)
                    {
                        animator.SetTrigger("isStunnedLow");
                        animator.ResetTrigger("isStunnedHigh");
                        animator.ResetTrigger("isStunnedLaunched");
                        hurtboxManager.hitAnimInfo = 0;
                        Debug.Log("LOW");
                    }
                    else if(hurtboxManager.hitAnimInfo == 2)
                    {
                        animator.SetTrigger("isStunnedHigh");
                        animator.ResetTrigger("isStunnedLow");
                        animator.ResetTrigger("isStunnedLaunched");
                        hurtboxManager.hitAnimInfo= 0;
                        Debug.Log("HIGH");
                    }
                    else if(hurtboxManager.hitAnimInfo == 3)
                    {
                        animator.SetTrigger("isStunnedLaunched");
                        animator.ResetTrigger("isStunnedHigh");
                        animator.ResetTrigger("isStunnedLow");
                        hurtboxManager.hitAnimInfo = 0;
                        Debug.Log("LAUNCH");
                    }
                }

                


                if (attackCooldown > 0f && isAirAttacking == false)
                {
                    attackCooldown = attackCooldown - Time.deltaTime;
                }

                if (attackCooldown <= 0f)
                {
                    isAttacking = false;
                }

                if (attackCooldown < 0f)
                {
                    attackCooldown = 0f;
                }

                if (isAirAttacking && playerMovement.IsGrounded())
                {
                    isAirAttacking = false;
                }


                //handles the directional inputs

                if (playerMovement.horizontal.x > 0.2 && !playerInput.actions["Crouch"].WasPressedThisFrame())
                {
                    isRightPressed = true;
                    isLeftPressed = false;
                }
                else if (playerMovement.horizontal.x < -0.2 && !playerInput.actions["Crouch"].WasPressedThisFrame())
                {
                    isLeftPressed = true;
                    isRightPressed = false;
                }
                else
                {
                    isRightPressed = false;
                    isLeftPressed = false;
                }

                if (playerInput.actions["Crouch"].WasPressedThisFrame())
                {
                    isDownPressed = true;
                    isRightPressed = false;
                    isLeftPressed = false;
                    if (playerMovement.IsGrounded() && isDownPressed && !playerMovement.isDashing)
                    {
                        isCrouching = true;
                        isRightPressed = false;
                        isLeftPressed = false;
                    }
                }

                if (isCrouching)
                {
                    animator.SetBool("isCrouching", true);
                }

                //Ends the crouch when the player releases down
                if (playerInput.actions["Crouch"].WasReleasedThisFrame())
                {
                    isCrouching = false;
                    isDownPressed = false;
                    animator.ResetTrigger("isLCAttacking");
                    animator.SetBool("isCrouching", false);
                }

                //Runs the bool that checks if it's possible to perform a Null attack
                if (isRightPressed == false && isLeftPressed == false && isDownPressed == false)
                {
                    isNothingPressed = true;
                }
                else
                {
                    isNothingPressed = false;
                }

                if (isCancel)
                {
                    animator.SetBool("canCancel", true);
                }
                else if (!isCancel)
                {
                    animator.SetBool("canCancel", false);
                }
            }
            else if(playerHealth.p1Dead || playerHealth.p2Dead)
            {
                if(playerHealth.p2Dead && gameObject.tag == "Player1")
                {
                    Debug.Log("P1 Wins");
                    animator.SetTrigger("isWin");
                }
                else if(playerHealth.p1Dead && gameObject.tag == "Player2")
                {
                    animator.SetTrigger("isWin");
                }
                else if(playerHealth.p1Dead && gameObject.tag == "Player1")
                {
                    animator.SetBool("isDead", true);
                }
                else if(playerHealth.p2Dead && gameObject.tag == "Player2")
                {
                    animator.SetBool("isDead", true);
                }
            }
        }

        public void RoundEndAnimReset()
        {
            Debug.Log("Player Reset Animations");
            animator.SetTrigger("resetAnims");
        }


        public bool IsCrouched()
        {
            return isCrouching;
        }

        public bool CanCancel()                             //Starts the chance to parry an attack
        {
            return isCancel = true;
        }

        public bool EndCancel()                             //Ends the chance to parry an attack
        {
            return isCancel = false;
        }


        public void CanParry()
        {
            playerAttackingHitboxes.isParry = true;
        }
        public void CantParry()
        {
            playerAttackingHitboxes.isParry = false;
        }
        public void EndParry()
        {
            hurtboxManager.parried = false;
            animator.SetBool("isParried", false);
        }


        public void TimeSlowParry()                         //Slows down time upon Perfect Parry
        {
            timeManager.timeSlowedParry = true;

            if (gameObject.tag == "Player1")
            {
                materialManager.p1Light = true;
            }
            else if (gameObject.tag == "Player2")
            {
                materialManager.p2Light = true;
            }
        }
        public void TimeSlowbreak()                         //Slows down time upon Shield Break
        {
            timeManager.timeSlowedParry = true;
            if(gameObject.tag == "Player2")
            {
                materialManager.p1Light = true;
            }
            else if(gameObject.tag == "Player1")
            {
                materialManager.p2Light = true;
            }
        }
        public void TimeNormal()                               //Reverts time back to normal after being slowed down
        {
            timeManager.timeSlowedParry = false;
            materialManager.p1Light = false;
            materialManager.p2Light = false;
        }

        public void StageLights()                               //Changes the lights to match the player
        {
            if(gameObject.tag == "Player1")
            {
                materialManager.p1Light = true;
            }
            else if(gameObject.tag == "Player2")
            {
                materialManager.p2Light = true;
            }
        }

        public void NormalLights()                             //Changes the Stage lights back to white
        {
            materialManager.p1Light = false;
            materialManager.p2Light = false;
        }

        public void StunEnd()
        {
            if(hurtboxManager.stunDuration <= 0)
            {
                animator.SetBool("isStunned", false);
            }
        }

        public void LandEnd()
        {
            if (playerMovement.canRecover)
            {
                animator.ResetTrigger("recoverNorm");
                animator.ResetTrigger("recoverBack");
                animator.SetTrigger("knockedDown");
            }
        }

        public void RecoverChance()
        {
            if(playerMovement.canRecover && playerMovement.isRecovering)
            {
                animator.SetTrigger("recoverBack");
                animator.ResetTrigger("knockedDown");
            }
            else if(!playerMovement.canRecover)
            {
                animator.SetTrigger("recoverNorm");
                animator.ResetTrigger("knockedDown");
            }
        }
    }
}