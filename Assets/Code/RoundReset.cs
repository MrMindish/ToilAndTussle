using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AB
{
    public class RoundReset : MonoBehaviour
    {
        RoundManager roundManager;
        public GameObject player;  // Reference to the player GameObject
        public Transform playerStartPoint;  // Reference to the Transform of the start point

        private void Awake()
        {
            roundManager = GetComponentInParent<RoundManager>();
        }

        private void Update()
        {
            if (roundManager.playerReset)
            {
                ResetPlayerPosition();
                roundManager.playerReset = false;  // Reset the flag to prevent repeated resets
            }
        }

        private void ResetPlayerPosition()
        {
            if (player != null && playerStartPoint != null)
            {
                player.transform.position = playerStartPoint.position;
                player.transform.rotation = playerStartPoint.rotation;  // Optionally reset rotation
            }
        }
    }
}