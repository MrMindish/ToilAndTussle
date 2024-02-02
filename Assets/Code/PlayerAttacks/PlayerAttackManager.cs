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

        public bool isAttacking;
        public float attackCooldown;

        public float startPosition;
        public float endPosition;

        private Animation anim;
        private Animator animator;

        PlayerMovement playerMovement;

        private void Awake()
        {
            anim = GetComponent<Animation>();
            animator = GetComponent<Animator>();
            playerMovement = GetComponentInParent<PlayerMovement>();

            isAttacking = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(lightInput) && playerMovement.IsGrounded() && isAttacking == false)
            {
                animator.SetTrigger("isLNAttacking");
                isAttacking = true;
                attackCooldown = 0.6f;
            }

            if (attackCooldown > 0f)
            {
                attackCooldown = attackCooldown - Time.deltaTime;
            }

            if (attackCooldown <= 0f)
            {
                isAttacking = false;
            }
        }

        private void AnimationChecks()
        {

        }



    }
}