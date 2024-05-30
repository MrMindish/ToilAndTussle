using System;
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

        public static RoundManager instance;
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

            instance = this;
        }
        private void Start()
        {
            playerReset = false;
            fadeToBlack = false;
            fadeFromBlack = false;
            playerOneWins = false;
            playerTwoWins = false;
        }

        public IEnumerator P2WinSequence()
        {
            playerTwoWins = true;
            // Trigger animation events here
            yield return new WaitForSeconds(1.0f); // Wait for the animation to play

            // Fade to black
            FadeToBlack();

            // Wait a moment
            yield return new WaitForSeconds(1.0f);

            // Reset the scene for the next round
            Debug.Log("Reset Round Enumerator");
            ResetRound();

            // Fade back in
            FadeFromBlack();
        }

        public IEnumerator P1WinSequence()
        {
            playerOneWins = true;
            // Trigger animation events here
            yield return new WaitForSeconds(1.0f); // Wait for the animation to play

            // Fade to black
            FadeToBlack();

            // Wait a moment
            yield return new WaitForSeconds(1.0f);

            // Reset the scene for the next round
            ResetRound();

            // Fade back in
            FadeFromBlack();
        }

        private void FadeToBlack()
        {
            Debug.Log("Fade to Black");
            fadeToBlack = true;
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

        private void FadeFromBlack()
        {
            playerReset = false;
            Debug.Log("Fade from Black");
            fadeFromBlack = true;
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