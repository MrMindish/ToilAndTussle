using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AB
{
    public class VersusTimer : MonoBehaviour
    {
        RoundManager roundManager;

        public int startingTime = 99;
        public Image tensPlaceImage;
        public Image onesPlaceImage;
        public Sprite[] numberSprites; // Array to store the number sprites from 0 to 9

        private int currentTime = 99;

        void Start()
        {
            roundManager = GetComponentInParent<RoundManager>();

            currentTime = startingTime;
        }

        private void Update()
        {
            if (!roundManager.roundStarted)
            {
                ResetTimer();
            }
        }

        public IEnumerator Countdown()
        {
            roundManager.roundStart = false;
            while (currentTime >= 0)
            {
                UpdateTimerDisplay();
                yield return new WaitForSeconds(1f);
                currentTime--;
            }
        }

        void UpdateTimerDisplay()
        {
            int tens = currentTime / 10;
            int ones = currentTime % 10;
            tensPlaceImage.sprite = numberSprites[tens];
            onesPlaceImage.sprite = numberSprites[ones];
        }

        public void ResetTimer()
        {
            currentTime = startingTime;
        }
    }
}