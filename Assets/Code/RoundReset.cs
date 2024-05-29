using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AB
{
    public class RoundReset : MonoBehaviour
    {
        RoundManager roundManager;
        public GameObject player1;  // Reference to the player GameObject
        public GameObject player2;
        public Transform player1StartPoint;  // Reference to the Transform of the start point
        public Transform player2StartPoint;

        private void Awake()
        {
            roundManager = GetComponentInParent<RoundManager>();
        }

        private void Update()
        {
            if (roundManager.playerReset)
            {
                Debug.Log("Round Reset 1");
                ResetPlayerPosition();
            }
        }

        public void ResetPlayerPosition()
        {
            if (player1 != null || player2 != null)
            {
                Debug.Log("Round Reset 2");
                player1.transform.position = player1StartPoint.position;
                player2.transform.position = player2StartPoint.position;
            }
        }
    }
}
