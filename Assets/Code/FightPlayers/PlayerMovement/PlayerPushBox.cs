using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AB
{
    public class PlayerPushBox : MonoBehaviour
    {
        //Uses onTriggerStay to push player 1 and 2 slowly away from each other when grounded
        public float pushForce = 1f;
        public bool canPush = false;

        PlayerMovement playerMovement;

        [SerializeField] private Rigidbody rb;

        private void Awake()
        {
            playerMovement = GetComponentInParent<PlayerMovement>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (playerMovement.IsGrounded() == true)
            {
                // Check if the colliding object has a Rigidbody
                Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
                if (otherRigidbody != null && playerMovement.IsGrounded() == true)
                {
                    canPush = true;
                /*
                    if (playerMovement.playerTwoX.transform.position.x < transform.position.x)
                    {
                        playerMovement.moveSpeed = playerMovement.moveSpeed + 1;
                    }
                    else
                    {
                        new Vector3(rb.velocity.x - 1, rb.velocity.y + 1, 0f);
                    } 
                */
                    Debug.Log("Hello");
                }
                else
                {
                    canPush = false;
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            playerMovement.moveSpeed = 3;
        }

    }
}