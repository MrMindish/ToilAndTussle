using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AB
{
    public class VersusComboManager : MonoBehaviour
    {
        UIManager uiManager;

        public Image tensComboImage;
        public Image onesComboImage;
        public Sprite[] numberSprites; // Array to store the number sprites from 0 to 9

        private int currentCombo;
        private int currentCombotens;

        public int updatedComboCount;


        void Start()
        {
            currentCombo = 0;
            currentCombotens = 0;

            uiManager = GetComponentInParent<UIManager>();
        }

        private void Update()
        {
            UpdateComboDisplay();

        }

        void UpdateComboDisplay()
        {
            if(updatedComboCount <= 0)
            {
                tensComboImage.sprite = numberSprites[updatedComboCount];
                onesComboImage.sprite = numberSprites[updatedComboCount];
            }
            else if(updatedComboCount > 0)
            {
                int tens = updatedComboCount / 10;
                int ones = updatedComboCount % 10;
                tensComboImage.sprite = numberSprites[tens];
                onesComboImage.sprite = numberSprites[ones];
            }
        }

        public void ResetCombo()
        {
            
        }
    }
}