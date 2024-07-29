using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AB
{
    public class ComboUIAnimations : MonoBehaviour
    {
        private Animator animator;
        private UIManager uiManager;
        private VersusComboManager versusComboManager;
        private PlayerMovement playerMovement1;
        private PlayerMovement playerMovement2;

        private void Start()
        {
            animator = GetComponent<Animator>();

            if (animator == null)
            {
                Debug.LogError("Animator component missing on " + gameObject.name);
            }

            uiManager = GetComponentInParent<UIManager>();
            if (uiManager == null)
            {
                Debug.LogError("UIManager component missing in parent of " + gameObject.name);
            }

            versusComboManager = GetComponentInParent<VersusComboManager>();
            if (versusComboManager == null)
            {
                Debug.LogError("VersusComboManager component missing in parent of " + gameObject.name);
            }

            GameObject player1 = GameObject.FindGameObjectWithTag("Player1");
            if (player1 != null)
            {
                playerMovement1 = player1.GetComponentInParent<PlayerMovement>();
                if (playerMovement1 == null)
                {
                    Debug.LogError("PlayerMovement component missing on Player1 GameObject.");
                }
            }
            else
            {
                Debug.LogError("Player1 GameObject is not found.");
            }

            GameObject player2 = GameObject.FindGameObjectWithTag("Player2");
            if (player2 != null)
            {
                playerMovement2 = player2.GetComponentInParent<PlayerMovement>();
                if (playerMovement2 == null)
                {
                    Debug.LogError("PlayerMovement component missing on Player2 GameObject.");
                }
            }
            else
            {
                Debug.LogError("Player2 GameObject is not found.");
            }
        }

        private void Update()
        {
                if (uiManager.player1Combo > 0 && playerMovement1 != null && playerMovement1.transform.position.x > 0)
                {
                    animator.SetBool("P1Counter", true);
                    animator.SetTrigger("LeftComboTrigger");
                }
                else if (uiManager.player1Combo > 0 && playerMovement1 != null && playerMovement1.transform.position.x < 0)
                {
                    animator.SetBool("P1Counter", true);
                    animator.SetTrigger("RightComboTrigger");
                }
                else if (uiManager.player2Combo > 0 && playerMovement2 != null && playerMovement2.transform.position.x > 0)
                {
                    animator.SetBool("P2Counter", true);
                    animator.SetTrigger("LeftComboTrigger");
                }
                else if (uiManager.player2Combo > 0 && playerMovement2 != null && playerMovement2.transform.position.x < 0)
                {
                    animator.SetBool("P2Counter", true);
                    animator.SetTrigger("RightComboTrigger");
                }

            if (versusComboManager != null && versusComboManager.updatedComboCount <= 0)
            {
                animator.SetTrigger("EndTrigger");
                animator.SetBool("P1Counter", false);
                animator.SetBool("P2Counter", false);
            }
        }
    }
}