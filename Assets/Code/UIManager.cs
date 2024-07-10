using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AB
{
    public class UIManager : MonoBehaviour
    {
        PlayerHurtboxManager hurtboxManager1;
        PlayerHurtboxManager hurtboxManager2;

        VersusComboManager versusComboManager;

        public int player1Combo;
        public int player2Combo;

        void Start()
        {
            hurtboxManager1 = GameObject.FindGameObjectWithTag("Player1").GetComponentInChildren<PlayerHurtboxManager>();
            hurtboxManager2 = GameObject.FindGameObjectWithTag("Player2").GetComponentInChildren<PlayerHurtboxManager>();

            versusComboManager = GetComponentInChildren<VersusComboManager>();

        }

        private void Update()
        {
            UpdatePlayerCombo();


        }

        void UpdatePlayerCombo()
        {
            player1Combo = hurtboxManager2.hitCountDuringStun;
            player2Combo = hurtboxManager1.hitCountDuringStun;

            if (player1Combo > 0)
            {
                //Causes the combo counter to appear
                Debug.Log("Player 1 is balling");
                versusComboManager.updatedComboCount = player1Combo;
            }
            else if (player1Combo < 1 && player2Combo < 1)
            {
                //Causes combo counter to disappear/ maybe play supportive animation?
                Debug.Log("Player 1 is lagging");
                versusComboManager.updatedComboCount = 0;
            }

            if (player2Combo > 0)
            {
                Debug.Log("Player 2 is balling");
                versusComboManager.updatedComboCount = player2Combo;
            }
            else if(player2Combo < 1 && player1Combo < 1)
            {
                Debug.Log("Player 2 is lagging");
                versusComboManager.updatedComboCount = 0;
            }
        }

    }
}