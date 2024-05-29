using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AB
{
    public class RoundManager : MonoBehaviour
    {
        PlayerHealth playerHealth;

        public bool playerReset;
        public bool fadeToBlack;
        public bool fadeFromBlack;

        public enum GameState
        {
            Round1,
            Round2,
            Round3,
            // Add more rounds as needed
            GameOver
        }
        private void Start()
        {
            playerHealth = GetComponentInChildren<PlayerHealth>();
            playerReset = false;
            fadeToBlack = false;
            fadeFromBlack = false;
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
            fadeToBlack=false;
        }

        private IEnumerator FadeFromBlack()
        {
            playerReset = false;
            Debug.Log("Fade from Black");
            fadeFromBlack = true;
            yield return null;
        }

    }
}