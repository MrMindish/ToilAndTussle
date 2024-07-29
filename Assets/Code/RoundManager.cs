using System;
using System.Collections;
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

        public bool playerReset;
        public bool fadeToBlack;
        public bool fadeFromBlack;
        public bool endGameScene;

        public bool playerOneWins;
        public bool playerTwoWins;

        public bool p1WinsSet;
        public bool p2WinsSet;

        public int roundCount;
        public int p1WinCount;
        public int p2WinCount;

        public GameObject player1;
        public GameObject player2;
        public Transform player1StartPoint;
        public Transform player2StartPoint;

        public bool roundStart;
        public bool roundStarted;

        public static RoundManager instance;

        public enum GameState
        {
            Round1,
            Round2,
            Round3,
            GameOver
        }

        private void Awake()
        {
            instance = this;

            if (player1 != null)
            {
                player1Health = player1.GetComponentInChildren<PlayerHealth>();
                attackManager1 = player1.GetComponentInChildren<PlayerAttackManager>();
            }
            else
            {
                Debug.LogError("Player1 GameObject is not assigned in the Inspector.");
            }

            if (player2 != null)
            {
                player2Health = player2.GetComponentInChildren<PlayerHealth>();
                attackManager2 = player2.GetComponentInChildren<PlayerAttackManager>();
            }
            else
            {
                Debug.LogError("Player2 GameObject is not assigned in the Inspector.");
            }

            versusTimer = GetComponentInChildren<VersusTimer>();
            if (versusTimer == null)
            {
                Debug.LogError("VersusTimer component is missing.");
            }

            materialManager = GetComponent<MaterialManager>();
            if (materialManager == null)
            {
                Debug.LogError("MaterialManager component is missing.");
            }
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

            if (player1 == null || player2 == null || player1StartPoint == null || player2StartPoint == null)
            {
                Debug.LogError("Player GameObjects or start points are not assigned in the Inspector.");
            }

            StartCoroutine(OpeningRoundStart());
        }

        private void Update()
        {
            if (roundStart && !roundStarted)
            {
                roundStarted = true;
                if (versusTimer != null)
                {
                    StartCoroutine(versusTimer.Countdown());
                }
            }
        }

        public IEnumerator OpeningRoundStart()
        {
            yield return new WaitForSeconds(3.0f);
            roundStart = true;
        }

        public IEnumerator P2WinSequence()
        {
            playerTwoWins = true;
            roundStarted = false;
            p2WinCount++;
            yield return new WaitForSeconds(2.0f);
            RoundEndChecker();
        }

        public IEnumerator P1WinSequence()
        {
            playerOneWins = true;
            roundStarted = false;
            p1WinCount++;
            yield return new WaitForSeconds(2.0f);
            RoundEndChecker();
        }

        public IEnumerator RoundResetSequence()
        {
            FadeToBlack();
            yield return new WaitForSeconds(1.0f);
            ResetRound();
            yield return new WaitForSeconds(1.0f);
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

            if (materialManager != null)
            {
                materialManager.p1Light = false;
                materialManager.p2Light = false;
            }

            if (player1Health != null) player1Health.HealthReset();
            if (player2Health != null) player2Health.HealthReset();
            if (attackManager1 != null) attackManager1.RoundEndAnimReset();
            if (attackManager2 != null) attackManager2.RoundEndAnimReset();
            ResetPlayerPosition();
            if (versusTimer != null) versusTimer.ResetTimer();
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
            if (playerReset && player1 != null && player2 != null && player1StartPoint != null && player2StartPoint != null)
            {
                player1.transform.position = player1StartPoint.position;
                player2.transform.position = player2StartPoint.position;
            }
        }

        public void RoundEndChecker()
        {
            roundCount = p1WinCount + p2WinCount;

            if (p1WinCount >= 2 && p2WinCount <= 1)
            {
                p1WinsSet = true;
                StartCoroutine(EndGameScene());
            }
            else if (p2WinCount >= 2 && p1WinCount <= 1)
            {
                p2WinsSet = true;
                StartCoroutine(EndGameScene());
            }
            else if (p2WinCount == 2 && p1WinCount == 2)
            {
                p1WinsSet = true;
                p2WinsSet = true;
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