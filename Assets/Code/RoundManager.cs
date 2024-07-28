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

        VersusTimer versusTimer;
        MaterialManager materialManager;

        public bool playerReset;                        //Tells the other scripts to revert any changes made during the fight
        public bool fadeToBlack;
        public bool fadeFromBlack;
        public bool endGameScene;

        public bool playerOneWins;                      //NOT USED FOR WINNING THE OVERALL SET! Simply refers to the victory of each round
        public bool playerTwoWins;                      //NOT USED FOR WINNING THE OVERALL SET! Simply refers to the victory of each round

        public bool p1WinsSet;                          //USED FOR ENDING THE GAME AFTER PLAYER WINS ENOUGH ROUNDS
        public bool p2WinsSet;                          //USED FOR ENDING THE GAME AFTER PLAYER WINS ENOUGH ROUNDS

        public int roundCount;
        public int p1WinCount;
        public int p2WinCount;

        public GameObject player1;  // Reference to the player GameObject
        public GameObject player2;
        public Transform player1StartPoint;  // Reference to the Transform of the start point
        public Transform player2StartPoint;

        public bool roundStart; //Used to give the players a few seconds to breathe between rounds
        public bool roundStarted;

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

            versusTimer = GetComponentInChildren<VersusTimer>();
            materialManager = GetComponent<MaterialManager>();

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
            endGameScene = false;

            p1WinsSet = false;
            p2WinsSet = false;

            StartCoroutine(OpeningRoundStart());
        }
        private void Update()
        {
            if (roundStart)
            {
                roundStarted = true;
                StartCoroutine(versusTimer.Countdown());
            }
        }

        public IEnumerator OpeningRoundStart()                                                     //Activates at the start of the scene
        {
            yield return new WaitForSeconds(3.0f);

            roundStart = true;

        }

        public IEnumerator P2WinSequence()                                                          //Activates when Player 2 wins the round
        {
            playerTwoWins = true;
            roundStarted = false;
            p2WinCount++;
            yield return new WaitForSeconds(2.0f); // Wait for the animation to play
            RoundEndChecker();
        }

        public IEnumerator P1WinSequence()                                                          //Activates when Player 1 wins the round
        {
            playerOneWins = true;
            roundStarted = false;
            p1WinCount++;
            yield return new WaitForSeconds(2.0f);
            RoundEndChecker();
        }

        public IEnumerator RoundResetSequence()                                                     //Starts the second round
        {
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

            yield return new WaitForSeconds(3.0f);
            StartCoroutine(OpeningRoundStart());
            roundStarted = true;
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
            playerOneWins = false;
            playerTwoWins = false;
            materialManager.p1Light = false;
            materialManager.p2Light = false;
            player1Health.HealthReset();
            player2Health.HealthReset();
            attackManager1.RoundEndAnimReset();
            attackManager2.RoundEndAnimReset();
            ResetPlayerPosition();
            versusTimer.ResetTimer();

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

        public void RoundEndChecker()
        {
            roundCount = p1WinCount + p2WinCount;

            if(p1WinCount >= 2 && p2WinCount <= 1)
            {
                p1WinsSet = true;
                StartCoroutine(EndGameScene());
            }
            else if(p2WinCount >= 2 && p1WinCount <= 1)
            {
                p2WinsSet = true;
                StartCoroutine(EndGameScene());
            }
            else if(p2WinCount == 2 && p1WinCount == 2)
            {
                p1WinsSet = true;
                p2WinsSet=true;
                StartCoroutine(EndGameScene());
            }
            else
            {
                StartCoroutine(RoundResetSequence());
            }
        }

        public IEnumerator EndGameScene()
        {
            yield return new WaitForSeconds(3.0f);
            endGameScene = true;
        }
    }
}