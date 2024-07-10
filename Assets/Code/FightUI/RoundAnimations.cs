using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AB
{
    public class RoundAnimations : MonoBehaviour
    {
        private Animation anim;
        private Animator animator;

        RoundManager roundManager;

        private void Awake()
        {
            anim = GetComponent<Animation>();
            animator = GetComponent<Animator>();

            roundManager = GetComponentInParent<RoundManager>();
        }
        private void Start()
        {
            StartCoroutine(Round1Sequence());
        }

        private void Update()
        {
            if(roundManager.playerReset && roundManager.roundCount == 1)
            {
                StartCoroutine(Round2Sequence());
            }
            else if(roundManager.playerReset && roundManager.roundCount == 2)
            {
                StartCoroutine(Round3Sequence());
            }

            if (roundManager.roundStarted)
            {
                animator.SetBool("FightTransition", true);
            }
            else if (!roundManager.roundStarted)
            {
                animator.SetBool("FightTransition", false);
            }

            if (roundManager.p1WinsSet && !roundManager.p2WinsSet)
            {
                animator.SetTrigger("P1WINS");
            }
            else if (roundManager.p2WinsSet && !roundManager.p1WinsSet)
            {
                animator.SetTrigger("P2WINS");
            }
            else if(roundManager.p1WinsSet && roundManager.p2WinsSet)
            {
                animator.SetTrigger("Stalemate");
            }
        }

        public IEnumerator Round1Sequence()
        {
            animator.SetTrigger("Round1");
            yield return new WaitForSeconds(2.0f);
            animator.SetTrigger("FIGHT");
        }

        public IEnumerator Round2Sequence()
        {
            animator.SetTrigger("Round2");
            yield return new WaitForSeconds(3.0f);
            animator.SetTrigger("FIGHT");
        }

        public IEnumerator Round3Sequence()
        {
            animator.SetTrigger("Round3");
            yield return new WaitForSeconds(3.0f);
            animator.SetTrigger("FIGHT");
        }
    }
}