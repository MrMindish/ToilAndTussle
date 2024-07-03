using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AB
{
    public class ScreenFadeManager : MonoBehaviour
    {
        //Calls upon the animation and animator windows
        private Animation anim;
        private Animator animator;

        RoundManager roundManager;

        private void Awake()
        {
            anim = GetComponent<Animation>();
            animator = GetComponent<Animator>();

            roundManager = GetComponentInParent<RoundManager>();
        }

        private void FixedUpdate()
        {
            if(roundManager.fadeToBlack && !roundManager.fadeFromBlack)
            {
                animator.SetTrigger("FadeOut");
                animator.ResetTrigger("FadeIn");
            }
            else if (!roundManager.fadeToBlack && roundManager.fadeFromBlack)
            {
                animator.SetTrigger("FadeIn");
            }
            else if (!roundManager.fadeFromBlack && !roundManager.fadeFromBlack)
            {
                animator.ResetTrigger("FadeIn");
                animator.ResetTrigger("FadeOut");
            }
        }
    }
}