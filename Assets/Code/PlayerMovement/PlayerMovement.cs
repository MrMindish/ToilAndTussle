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

        private float horizontal;
        public float moveSpeed;
        public float slowedMoveSpeed;
        public float jumpingPower;
        public bool isStunned;

        public float pushForce;

        public string JumpName;

        public GameObject playerOneX;
        public GameObject playerTwoX;
        

        private bool isFacingRight = true;

        [SerializeField] private Rigidbody rb;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform orientation;

        private void Awake()
        {
            playerPushBox = GetComponentInChildren<PlayerPushBox>();
        }

        void Update()
        {
                horizontal = Input.GetAxisRaw("Horizontal");

                if (Input.GetButtonDown(JumpName) && IsGrounded())
                {
                    rb.velocity = new Vector3(rb.velocity.x, jumpingPower, 0);
                    
                }

                if (Input.GetButtonUp(JumpName) && rb.velocity.y > 0f)
                {
                    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, 0f);
                }

                Flip();
            
        }

        private void OnTriggerStay(Collider other)
        {
            
        }

        private void FixedUpdate()
        {
            if (IsGrounded() && playerPushBox.canPush == false)
            {
                //movement part
                rb.velocity = new Vector3(horizontal * moveSpeed, rb.velocity.y, 0f);

            }
            else if (IsGrounded() && playerPushBox.canPush == true)
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



        private void Flip()
        {
            if (gameObject.tag == "Player1" && IsGrounded() && transform.position.x > playerTwoX.transform.position.x && isFacingRight == true)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            if (gameObject.tag == "Player1" && IsGrounded() && transform.position.x < playerTwoX.transform.position.x && isFacingRight == false)
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