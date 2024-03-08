using AB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        //Registers the directional inputs
        public bool isRightPressed;
        public bool isLeftPressed;
        public bool isDownPressed;
        public bool isNothingPressed;

        //Used for the crouch input
        public bool isCrouching = false;

        //Used for Cancelling Moves into Specials
        public bool isCancel = false;



        private void Awake()
        {
            anim = GetComponent<Animation>();
            animator = GetComponent<Animator>();
            playerMovement = GetComponentInParent<PlayerMovement>();
            hurtboxManager = GetComponentInChildren<PlayerHurtboxManager>();
            playerAttackingHitboxes = GetComponentInChildren<PlayerAttackingHitboxes>();

            isAttacking = false;
            isAirAttacking = false;
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

            //Performs the Blocking Animation
            if(playerMovement.isBlocking && !hurtboxManager.isStunned)
            {
                animator.SetBool("isBlocking", true);
            }
            else
            {
                animator.SetBool("isBlocking", false);
            }

            //Performs the Parry Animation
            if(hurtboxManager.parried && !hurtboxManager.isStunned)
            {
                animator.SetBool("isParried", true);
                hurtboxManager.parried = false;
            }
            else
            {
                animator.SetBool("isParried", false);
            }

            //Performs the Light Null Attack
            if (Input.GetKeyDown(lightInput) && playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isNothingPressed)
            {
                animator.SetTrigger("isLNAttacking");
                isAttacking = true;
                attackCooldown = 0.26f;
            }

            //Performs the Light Forward Attack
            if (Input.GetKeyDown(lightInput) && playerMovement.IsGrounded() && transform.position.x > playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isLeftPressed)
            {
                //On the Left Side of the Opponent
                animator.SetTrigger("isLFAttacking");
                isAttacking = true;
                attackCooldown = 0.32f;
            }
            else if (Input.GetKeyDown(lightInput) && playerMovement.IsGrounded() && transform.position.x < playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isRightPressed)
            {
                //On the Right Side of the Opponent
                animator.SetTrigger("isLFAttacking");
                isAttacking = true;
                attackCooldown = 0.32f;
            }

            //Performs the Light Backwards Attack
            if (Input.GetKeyDown(lightInput) && playerMovement.IsGrounded() && transform.position.x < playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isLeftPressed)
            {
                //On the Right Side of the Opponent
                animator.SetTrigger("isLBAttacking");
                isAttacking = true;
                attackCooldown = 0.36f;
            }
            else if (Input.GetKeyDown(lightInput) && playerMovement.IsGrounded() && transform.position.x > playerMovement.playerTwoX.transform.position.x && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isRightPressed)
            {
                //On the Left Side of the Opponent
                animator.SetTrigger("isLBAttacking");
                isAttacking = true;
                attackCooldown = 0.36f;
            }

            //Performs the Light Crouch Attack
            if (Input.GetKeyDown(lightInput) && playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isDownPressed && isCrouching)
            {
                animator.SetTrigger("isLCAttacking");
                isAttacking = true;
                attackCooldown = 0.5f;
                animator.SetBool("canCancel", true);
            }

            //Performs the Light Aerial Attack
            if(Input.GetKeyDown(lightInput) && !playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false)
            {
                animator.SetTrigger("isLAAttacking");
                isAttacking = true;
                isAirAttacking = true;
                attackCooldown = 0.1f;
            }




            //Performs the Heavy Null Attack
            if (Input.GetKeyDown(heavyInput) && playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isNothingPressed)
            {
                animator.SetTrigger("isHNAttacking");
                isAttacking = true;
                attackCooldown = 0.4f;
                animator.SetBool("canCancel", true);
            }


            //Performs the Special Null Attack
            if(Input.GetKeyDown(specialInput) && playerMovement.IsGrounded() && isAttacking == false && hurtboxManager.isStunned == false && hurtboxManager.isShieldStunned == false && isNothingPressed)
            {
                animator.SetTrigger("isSN1Attacking");
                isAttacking= true;
                attackCooldown = 0.4f;
            }
            else if (Input.GetKeyDown(specialInput) && playerMovement.IsGrounded() && isAttacking == true && hurtboxManager.isStunned == false && isNothingPressed && hurtboxManager.isShieldStunned == false && isCancel)
            {
                animator.SetTrigger("isSN1Attacking");
                isAttacking = true;
                attackCooldown = 0.4f;
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
            if (Input.GetKey(KeyCode.DownArrow))
            {
                isDownPressed = true;
                isRightPressed = false;
                isLeftPressed = false;
                if (playerMovement.IsGrounded())
                {
                    animator.SetTrigger("isCrouchingStart");
                    isCrouching = true;
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                isRightPressed = true;
                isLeftPressed = false;
                isDownPressed = false;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                isLeftPressed = true;
                isRightPressed = false;
                isDownPressed = false;
            }
            else
            {
                isRightPressed = false;
                isLeftPressed = false;
                isDownPressed = false;
            }

            //Ends the crouch when the player releases down
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                isCrouching = false;
                animator.ResetTrigger("isCrouchingStart");
                animator.SetTrigger("isCrouchingEnd");
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
    }
}