using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
namespace AB
{
    public class PlayerMovement : MonoBehaviour
    {
        PlayerPushBox playerPushBox;
        PlayerAttackManager playerAttackManager;
        PlayerHurtboxManager hurtboxManager;

        //Handles all of the movement speed and such
        private float horizontal;
        public float moveSpeed;
        public float slowedMoveSpeed;
        public float jumpingPower;

        public float pushForce;

        public string JumpName;

        //Once the player has landed after being juggled, they can recover/jump backwards
        public bool canRecover;
        public float recoveryTime = 2;
        public float recoveryForce = 3;
        public bool isRecovering;

        //Used for the movement code to understand who the target is
        public GameObject playerOneX;
        public GameObject playerTwoX;

        //Makes sure the player is facing towards the enemy
        private bool isFacingRight = true;

        [SerializeField] private Rigidbody rb;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform orientation;

        private void Awake()
        {
            playerPushBox = GetComponentInChildren<PlayerPushBox>();
            playerAttackManager = GetComponentInChildren<PlayerAttackManager>();
            hurtboxManager = GetComponentInChildren<PlayerHurtboxManager>();
        }

        private void Start()
        {
            canRecover = false;
            isRecovering = false;
        }

        void Update()
        {
                horizontal = Input.GetAxisRaw("Horizontal");

                if (Input.GetButtonDown(JumpName) && IsGrounded() && playerAttackManager.isAttacking == false && playerAttackManager.isCrouching == false && hurtboxManager.isStunned == false)
                {
                    //if the jump is inputed while the fighter is grounded, not attacking or being attacked, or crouching, then the jump is performed
                    rb.velocity = new Vector3(rb.velocity.x, jumpingPower, 0);
                    
                }

                if (Input.GetButtonUp(JumpName) && rb.velocity.y > 0f)
                {
                    //Allows the jump to be cancelled halfway, allowing short hops to happen
                    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, 0f);
                }

            IsGrounded();
            Flip();

        }

        private void FixedUpdate()
        {
            if (IsGrounded() && playerPushBox.canPush == false && playerAttackManager.isAttacking == false && playerAttackManager.isCrouching == false && hurtboxManager.isStunned == false)
            {
                //movement part
                rb.velocity = new Vector3(horizontal * moveSpeed, rb.velocity.y, 0f);
            }
            else if (hurtboxManager.isStunned && hurtboxManager.isKnockback)
            {
                //Allows the player to get knocked backwards
                Debug.Log("is stunned");
                rb.velocity = new Vector3(hurtboxManager.hKnockback, hurtboxManager.vKnockback, 0f);
            }
            else if (hurtboxManager.isStunned && !hurtboxManager.isKnockback)
            {
                //Prevents the knockback from lasting throughout the entirety of the hitstun
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0f);
            }

            if (canRecover)
            {
                hurtboxManager.isInvincible = true;
                recoveryTime -= Time.deltaTime;

                if(Input.GetButtonDown(JumpName) && recoveryTime <= 0.4 && recoveryTime > 0 && isRecovering == false)
                {
                    isRecovering = true;
                    recoveryTime = 0.4f;
                }

                if(recoveryTime <= 0)
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
        }

        public bool IsGrounded()
        {
            return Physics.Raycast(orientation.position, Vector3.down, 0.1f);
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
            else if (gameObject.tag == "Player1" && IsGrounded() && transform.position.x < playerTwoX.transform.position.x && isFacingRight == true && playerAttackManager.isAttacking == false)
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
            }
            else if (gameObject.tag == "Player1" && transform.position.x > playerTwoX.transform.position.x && hurtboxManager.hKnockback < 0f)
            {
                hurtboxManager.hKnockback *= -1;
            }

            if (gameObject.tag == "Player2" && transform.position.x < playerOneX.transform.position.x && hurtboxManager.hKnockback > 0f)
            {
                hurtboxManager.hKnockback *= -1;
            }
            else if (gameObject.tag == "Player2" && transform.position.x > playerOneX.transform.position.x && hurtboxManager.hKnockback < 0f)
            {
                hurtboxManager.hKnockback *= -1;
            }
        }

        public void OnDrawGizmos()
        {

        }
    }
}