using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AB
{
    public class ComboUIAnimations : MonoBehaviour
    {
        private Animation anim;
        private Animator animator;

        UIManager uiManager;
        VersusComboManager versusComboManager;
        PlayerMovement playerMovement1;
        PlayerMovement playerMovement2;

        private void Start()
        {
            anim = GetComponent<Animation>();
            animator = GetComponent<Animator>();
            uiManager = GetComponentInParent<UIManager>();
            versusComboManager = GetComponentInParent<VersusComboManager>();

            playerMovement1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerMovement>();
            playerMovement2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            if(uiManager.player1Combo > 0 && playerMovement1.transform.position.x > 0)
            {
                //Activates the Combo counter for Player 1 on the left
                animator.SetBool("P1Counter", true);
                animator.SetTrigger("LeftComboTrigger");
            }
            else if(uiManager.player1Combo > 0 && playerMovement1.transform.position.x < 0)
            {
                //Activates the Combo counter for Player 1 on the right
                animator.SetBool("P1Counter", true);
                animator.SetTrigger("RightComboTrigger");
            }
            else if (uiManager.player2Combo > 0 && playerMovement2.transform.position.x > 0)
            {
                //Activates the Combo counter for Player 2 on the Left
                animator.SetBool("P2Counter", true);
                animator.SetTrigger("LeftComboTrigger");
            }
            else if(uiManager.player2Combo > 0 && playerMovement2.transform.position.x < 0)
            {
                //Activates the Combo counter for Player 2 on the right
                animator.SetBool("P2Counter", true);
                animator.SetTrigger("RightComboTrigger");
            }

            if(versusComboManager.updatedComboCount <= 0)
            {
                animator.SetTrigger("EndTrigger");
                animator.SetBool("P1Counter", false);
                animator.SetBool("P2Counter", false);
            }
        }


    }
}