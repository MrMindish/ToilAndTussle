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

        //Handles all of the movement speed and such
        private float horizontal;
        public float moveSpeed;
        public float slowedMoveSpeed;
        public float jumpingPower;

        public float pushForce;

        public string JumpName;

        //Used for the movement code to understand who the target is
        public GameObject playerOneX;
        public GameObject playerTwoX;

        //Makes sure the player is facing towards the enemy
        private bool isFacingRight = true;

        //Registers the directional inputs
        public bool isRightPressed;
        public bool isLeftPressed;
        public bool isDownPressed;

        //Used for the crouch input
        private bool isCrouching = false;

        [SerializeField] private Rigidbody rb;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform orientation;

        private void Awake()
        {
            playerPushBox = GetComponentInChildren<PlayerPushBox>();
            playerAttackManager = GetComponentInChildren<PlayerAttackManager>();
        }

        void Update()
        {
                horizontal = Input.GetAxisRaw("Horizontal");

                if (Input.GetButtonDown(JumpName) && IsGrounded() && playerAttackManager.isAttacking == false)
                {
                    rb.velocity = new Vector3(rb.velocity.x, jumpingPower, 0);
                    
                }

                if (Input.GetButtonUp(JumpName) && rb.velocity.y > 0f)
                {
                    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, 0f);
                }

                if(Input.GetKey(KeyCode.DownArrow) && !IsGrounded())
                {
                    IsCrouched();
                }

            //handles the directional inputs
            if (Input.GetKey(KeyCode.RightArrow))
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
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                isDownPressed = true;
                isRightPressed = false;
                isLeftPressed = false;
            }
            else
            {
                isRightPressed = false;
                isLeftPressed = false;
                isDownPressed = false;
            }


                Flip();
            
        }

        private void OnTriggerStay(Collider other)
        {
            
        }

        private void FixedUpdate()
        {
            if (IsGrounded() && playerPushBox.canPush == false && playerAttackManager.isAttacking == false)
            {
                //movement part
                rb.velocity = new Vector3(horizontal * moveSpeed, rb.velocity.y, 0f);
            }
            else if (IsGrounded() && playerPushBox.canPush == true && playerAttackManager.isAttacking == false)
            {
                //make it so that, if the player isn't moving, they get pushed back 
                rb.velocity = new Vector3(horizontal * slowedMoveSpeed, rb.velocity.y, 0f);
                if(slowedMoveSpeed == 0f)
                {
                    transform.position = transform.position;
                }
            }

        }

        public bool IsGrounded()
        {
            return Physics.Raycast(orientation.position, Vector3.down, 0.1f);
        }

        public bool IsCrouched()
        {
            return isCrouching;
        }



        private void Flip()
        {
            if (gameObject.tag == "Player1" && IsGrounded() && transform.position.x > playerTwoX.transform.position.x && isFacingRight == false && playerAttackManager.isAttacking == false)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            if (gameObject.tag == "Player1" && IsGrounded() && transform.position.x < playerTwoX.transform.position.x && isFacingRight == true && playerAttackManager.isAttacking == false)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }

        public void OnDrawGizmos()
        {

        }
    }
}