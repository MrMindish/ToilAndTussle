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
        public float attackCooldown;

        //Calculates what direction the attacker moves if they land the attack (This is to prevent corner trapping)
        public float startPosition;
        public float endPosition;

        //Calls upon the animation and animator windows
        private Animation anim;
        private Animator animator;

        PlayerMovement playerMovement;

        //Registers the directional inputs
        public bool isRightPressed;
        public bool isLeftPressed;
        public bool isDownPressed;
        public bool isNothingPressed;

        //Used for the crouch input
        public bool isCrouching = false;

        private void Awake()
        {
            anim = GetComponent<Animation>();
            animator = GetComponent<Animator>();
            playerMovement = GetComponentInParent<PlayerMovement>();

            isAttacking = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(lightInput) && playerMovement.IsGrounded() && isAttacking == false && isNothingPressed)
            {
                animator.SetTrigger("isLNAttacking");
                isAttacking = true;
                attackCooldown = 0.26f;
            }
            if (Input.GetKeyDown(lightInput) && playerMovement.IsGrounded() && isAttacking == false && isRightPressed)
            {
                animator.SetTrigger("isLFAttacking");
                isAttacking = true;
                attackCooldown = 0.32f;
            }

            if (attackCooldown > 0f)
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
        }

        public bool IsCrouched()
        {
            return isCrouching;
        }


    }
}