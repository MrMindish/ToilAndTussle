using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AB
{
    public class NewPlayerMovement : MonoBehaviour
    {
        PlayerPushBox playerPushBox;

        private float horizontal;
        public float moveSpeed;
        public float jumpingPower;

        public string JumpName;

        public GameObject playerOneX;
        public GameObject playerTwoX;

        [SerializeField] private Rigidbody rb;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform orientation;

        void Update()
        {
            horizontal = Input.GetAxisRaw("Horizontal");
        }

        private void FixedUpdate()
        {
            rb.velocity = new Vector3(horizontal * moveSpeed, rb.velocity.y, 0);
        }
    }
}