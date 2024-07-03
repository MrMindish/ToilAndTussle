using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AB
{
    public class RoundManager : MonoBehaviour
    {
        PlayerHealth player1Health;
        PlayerHealth player2Health;

        PlayerAttackManager attackManager1;
        PlayerAttackManager attackManager2;

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
        public bool roundStart; //Used to give the players a few seconds to breathe between rounds

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
            player1Health = GameObject.FindGameObjectWithTag("Player1").GetComponentInChildren<PlayerHealth>();
            player2Health = GameObject.FindGameObjectWithTag("Player2").GetComponentInChildren<PlayerHealth>();

            attackManager1 = GameObject.FindGameObjectWithTag("Player1").GetComponentInChildren<PlayerAttackManager>();
            attackManager2 = GameObject.FindGameObjectWithTag("Player2").GetComponentInChildren<PlayerAttackManager>();

            instance = this;
        }
        private void Start()
        {
            playerReset = false;
            fadeToBlack = false;
            fadeFromBlack = false;
            playerOneWins = false;
            playerTwoWins = false;
            roundStart = false;

            StartCoroutine(OpeningRoundStart());
        }

        public IEnumerator OpeningRoundStart()
        {
            yield return new WaitForSeconds(3.0f);

            roundStart = true;
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

            yield return new WaitForSeconds(1.0f);
            // Fade back in
            FadeFromBlack();

            yield return new WaitForSeconds(0.5f);

            fadeFromBlack = false;
            roundStart = true;
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

            yield return new WaitForSeconds(1.0f);
            // Fade back in
            FadeFromBlack();

            yield return new WaitForSeconds(0.5f);

            fadeFromBlack = false;
            roundStart = true;
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
            roundStart = false;
            playerOneWins = false;
            playerTwoWins = false;
            player1Health.HealthReset();
            player2Health.HealthReset();
            attackManager1.RoundEndAnimReset();
            attackManager2.RoundEndAnimReset();
            if (playerReset && !hasReset)
            {
                ResetPlayerPosition();
                hasReset = true; // Set the flag to true to prevent further resets
            }
        }

        private void FadeFromBlack()
        {
            fadeFromBlack = true;
            playerReset = false;
            Debug.Log("Fade from Black");
            fadeToBlack = false;
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