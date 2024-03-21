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

        //Registers the directional inputs
        public bool isRightPressed;
        public bool isLeftPressed;
        public bool isDownPressed;
        public bool isNothingPressed;

        //Used for the crouch input
        public bool isCrouching = false;

        //Used for Cancelling Moves into Specials
        public bool isCancel = false;

        private float timer;



        private void Awake()
        {
            anim = GetComponent<Animation>();
            animator = GetComponent<Animator>();
            playerInput = GetComponent<PlayerInput>();

            playerMovement = GetComponentInParent<PlayerMovement>();
            hurtboxManager = GetComponentInChildren<PlayerHurtboxManager>();
            playerAttackingHitboxes = GetComponentInChildren<PlayerAttackingHitboxes>();
            timeManager = GetComponentInParent<TimeManager>();
            playerShield = GetComponentInParent<PlayerShield>();

            isAttacking = false;
            isAirAttacking = false;
            timer = 0;
        }

        private void Update()
        {

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
            else if(playerMovement.isJumping && playerMovement.IsGrounded())
            {
                animator.ResetTrigger("jumpStart");
                animator.SetTrigger("jumpEnd");
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

            //Performs the Light Null Attack
            if (playerInput.actions["Light Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isNothingPressed)
            {
                animator.SetTrigger("isLNAttacking");
                isAttacking = true;
                attackCooldown = 0.26f;
            }

            //Performs the Light Forward Attack
            if (playerInput.actions["Light Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x > playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isLeftPressed)
            {
                //On the Left Side of the Opponent
                animator.SetTrigger("isLFAttacking");
                isAttacking = true;
                attackCooldown = 0.32f;
            }
            else if (playerInput.actions["Light Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x < playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isRightPressed)
            {
                //On the Right Side of the Opponent
                animator.SetTrigger("isLFAttacking");
                isAttacking = true;
                attackCooldown = 0.32f;
            }

            //Performs the Light Backwards Attack
            if (playerInput.actions["Light Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x < playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isLeftPressed)
            {
                //On the Right Side of the Opponent
                animator.SetTrigger("isLBAttacking");
                isAttacking = true;
                attackCooldown = 0.36f;
            }
            else if (playerInput.actions["Light Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x > playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isRightPressed)
            {
                //On the Left Side of the Opponent
                animator.SetTrigger("isLBAttacking");
                isAttacking = true;
                attackCooldown = 0.36f;
            }

            //Performs the Light Crouch Attack
            if (playerInput.actions["Light Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isDownPressed && isCrouching)
            {
                animator.SetTrigger("isLCAttacking");
                isAttacking = true;
                attackCooldown = 0.5f;
                animator.SetBool("canCancel", true);
            }

            //Performs the Light Aerial Attack
            if(playerInput.actions["Light Attack"].WasPressedThisFrame() && !playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false)
            {
                animator.SetTrigger("isLAAttacking");
                isAttacking = true;
                isAirAttacking = true;
                attackCooldown = 0.1f;
            }




            //Performs the Heavy Null Attack
            if (playerInput.actions["Heavy Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isNothingPressed)
            {
                animator.SetTrigger("isHNAttacking");
                isAttacking = true;
                attackCooldown = 0.4f;
                animator.SetBool("canCancel", true);
            }

            //Performs the Heavy Forward Attack
            if (playerInput.actions["Heavy Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x > playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isLeftPressed)
            {
                //On the Left Side of the Opponent
                animator.SetTrigger("isHFAttacking");
                isAttacking = true;
                attackCooldown = 0.8f;
            }
            else if (playerInput.actions["Heavy Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x < playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isRightPressed)
            {
                //On the Right Side of the Opponent
                animator.SetTrigger("isHFAttacking");
                isAttacking = true;
                attackCooldown = 0.8f;
            }

            //Performs the Heavy Backwards Attack
            if (playerInput.actions["Heavy Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x < playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isLeftPressed)
            {
                //On the Right Side of the Opponent
                animator.SetTrigger("isHBAttacking");
                isAttacking = true;
                attackCooldown = 0.5f;
            }
            else if (playerInput.actions["Heavy Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x > playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isRightPressed)
            {
                //On the Left Side of the Opponent
                animator.SetTrigger("isHBAttacking");
                isAttacking = true;
                attackCooldown = 0.5f;
            }

            //Performs the Heavy Crouch Attack
            if (playerInput.actions["Heavy Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isDownPressed && isCrouching)
            {
                animator.SetTrigger("isHCAttacking");
                isAttacking = true;
                attackCooldown = 0.7f;
            }

            //Performs the Heavy Aerial Attack
            if (playerInput.actions["Heavy Attack"].WasPressedThisFrame() && !playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false)
            {
                Debug.Log("HA");
                animator.SetTrigger("isHAAttacking");
                isAttacking = true;
                isAirAttacking = true;
                attackCooldown = 0.2f;
            }


            //Performs the Special Null Attack
            if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isNothingPressed)
            {
                animator.SetTrigger("isSN1Attacking");
                isAttacking= true;
                attackCooldown = 0.4f;
            }
            else if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && isAttacking == true && hurtboxManager.isStunned == false && isNothingPressed && hurtboxManager.isShieldStunned == false && isCancel)
            {
                animator.SetTrigger("isSN1Attacking");
                isAttacking = true;
                attackCooldown = 0.4f;
                animator.ResetTrigger("isCrouchingEnd");
            }

            //Performs the Special Forward Attack
            if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x > playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isLeftPressed)
            {
                //On the Left Side of the opponent
                animator.SetTrigger("isSF1Attacking");
                isAttacking = true;
                attackCooldown = 0.7f;
            }
            else if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x > playerMovement.playerTwoX.transform.position.x && isAttacking == true && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isLeftPressed && isCancel)
            {
                //On Left Side + Cancelled the move
                animator.SetTrigger("isSF1Attacking");
                isAttacking = true;
                attackCooldown = 0.7f;
                animator.ResetTrigger("isCrouchingEnd");
            }
            else if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x < playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isRightPressed)
            {
                //On the Right Side of the opponent
                animator.SetTrigger("isSF1Attacking");
                isAttacking = true;
                attackCooldown = 0.7f;
            }
            else if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x < playerMovement.playerTwoX.transform.position.x && isAttacking == true && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isRightPressed && isCancel)
            {
                //On Right Side + Cancelled the move
                animator.SetTrigger("isSF1Attacking");
                isAttacking = true;
                attackCooldown = 0.7f;
                animator.ResetTrigger("isCrouchingEnd");
            }

            //Performs the Special Backward Attack
            if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x < playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isLeftPressed)
            {
                //On the Left Side of the opponent
                animator.SetTrigger("isSB1Attacking");
                isAttacking = true;
                attackCooldown = 0.26f;
            }
            else if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x < playerMovement.playerTwoX.transform.position.x && isAttacking == true && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isLeftPressed && isCancel)
            {
                //On Left Side + Cancelled the move
                animator.SetTrigger("isSB1Attacking");
                isAttacking = true;
                attackCooldown = 0.26f;
                animator.ResetTrigger("isCrouchingEnd");
            }
            else if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x > playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isRightPressed)
            {
                //On the Right Side of the opponent
                animator.SetTrigger("isSB1Attacking");
                isAttacking = true;
                attackCooldown = 0.26f;
            }
            else if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && transform.position.x > playerMovement.playerTwoX.transform.position.x && isAttacking == true && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isRightPressed && isCancel)
            {
                //On Right Side + Cancelled the move
                animator.SetTrigger("isSB1Attacking");
                isAttacking = true;
                attackCooldown = 0.26f;
                animator.ResetTrigger("isCrouchingEnd");
            }

            //Performs the Special Down Attack
            if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && (isDownPressed || isCrouching))
            {
                animator.SetTrigger("isSC1Attacking");
                isAttacking = true;
                attackCooldown = 0.24f;
            }
            else if (playerInput.actions["Special Attack"].WasPressedThisFrame() && playerMovement.IsGrounded() && isAttacking == true && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && (isDownPressed || isCrouching) && isCancel)
            {
                animator.SetTrigger("isSC1Attacking");
                isAttacking = true;
                attackCooldown = 0.24f;
                animator.ResetTrigger("isCrouchingEnd");
            }

            if (attackCooldown > 0f && isAirAttacking == false)
            {
                attackCooldown = attackCooldown - Time.deltaTime;
            }

            if (attackCooldown <= 0f)
            {
                isAttacking = false;
            }

            if(attackCooldown < 0f)
            {
                attackCooldown = 0f;
            }

            if (isAirAttacking && playerMovement.IsGrounded())
            {
                isAirAttacking = false;
            }


            //handles the directional inputs

            if (playerMovement.horizontal.x > 0.2 && !isDownPressed)
            {
                isRightPressed = true;
                isLeftPressed = false;
                isDownPressed = false;
            }
            else if (playerMovement.horizontal.x < -0.2 && !isDownPressed)
            {
                isLeftPressed = true;
                isRightPressed = false;
                isDownPressed = false;
            }
            else if (playerInput.actions["Crouch"].WasPressedThisFrame() || playerMovement.horizontal.y < -0.5)
            {
                isDownPressed = true;
                isRightPressed = false;
                isLeftPressed = false;
                if (playerMovement.IsGrounded() && isDownPressed)
                {
                    isCrouching = true;
                }
            }
            else
            {
                isRightPressed = false;
                isLeftPressed = false;
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

        public bool IsCrouched()
        {
            return isCrouching;
        }

        public bool CanCancel()
        {
            return isCancel = true;
        }

        public bool EndCancel()
        {
            return isCancel = false;
        }

        public bool CanParry()
        {
            return playerAttackingHitboxes.isParry = true;
        }
        public bool CantParry()
        {
            return playerAttackingHitboxes.isParry = false;
        }
        public void EndParry()
        {
            hurtboxManager.parried = false;
            animator.SetBool("isParried", false);
        }
        public void TimeSlow()
        {
            timeManager.timeSlowed = true;
        }
        public void TimeNormal()
        {
            timeManager.timeSlowed = false;
        }
    }
}