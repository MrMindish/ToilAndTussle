using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace AB
{
    public class RoundManager : MonoBehaviour
    {
        PlayerHealth playerHealth;

        public bool playerReset;
        public bool fadeToBlack;
        public bool fadeFromBlack;

        public bool playerOneWins;
        public bool playerTwoWins;

        public GameObject player1;  // Reference to the player GameObject
        public GameObject player2;
        public Transform player1StartPoint;  // Reference to the Transform of the start point
        public Transform player2StartPoint;

        private bool hasReset; //Used to prevent playes being stuck after reset
        public enum GameState
        {
            Round1,
            Round2,
            Round3,
            // Add more rounds as needed
            GameOver
        }

        private void Awake()
        {
            playerHealth = GetComponentInChildren<PlayerHealth>();
        }
        private void Start()
        {
            playerReset = false;
            fadeToBlack = false;
            fadeFromBlack = false;
            playerOneWins = false;
            playerTwoWins = false;
        }
        private void Update()
        {
            if (playerHealth.p1Dead)
            {
                StartCoroutine(P2WinSequence());
            }
            else if (playerHealth.p2Dead)
            {
                StartCoroutine(P1WinSequence());
            }
        }

        private IEnumerator P2WinSequence()
        {
            playerTwoWins = true;
            // Trigger animation events here
            yield return new WaitForSeconds(1.0f); // Wait for the animation to play

            // Fade to black
            yield return StartCoroutine(FadeToBlack());

            // Wait a moment
            yield return new WaitForSeconds(1.0f);

            // Reset the scene for the next round
            ResetRound();

            // Fade back in
            yield return StartCoroutine(FadeFromBlack());
        }

        private IEnumerator P1WinSequence()
        {
            playerOneWins = true;
            // Trigger animation events here
            yield return new WaitForSeconds(1.0f); // Wait for the animation to play

            // Fade to black
            yield return StartCoroutine(FadeToBlack());

            // Wait a moment
            yield return new WaitForSeconds(1.0f);

            // Reset the scene for the next round
            ResetRound();

            // Fade back in
            yield return StartCoroutine(FadeFromBlack());
        }

        private IEnumerator FadeToBlack()
        {
            Debug.Log("Fade to Black");
            fadeToBlack = true;
            yield return new WaitForSeconds(1.0f);
        }

        private void ResetRound()
        {
            Debug.Log("Reset Scene");
            playerReset = true;
            fadeToBlack = false;
            if (playerReset && !hasReset)
            {
                ResetPlayerPosition();
                hasReset = true; // Set the flag to true to prevent further resets
            }
        }

        private IEnumerator FadeFromBlack()
        {
            playerReset = false;
            Debug.Log("Fade from Black");
            fadeFromBlack = true;
            yield return null;
        }


        public void ResetPlayerPosition()
        {
            if ((player1 != null || player2 != null) && playerReset)
            {
                player1.transform.position = player1StartPoint.position;
                player2.transform.position = player2StartPoint.position;
            }
        }
    }
}