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
            }
            else if (!roundManager.fadeToBlack && roundManager.fadeFromBlack)
            {
                animator.SetTrigger("FadeIn");
            }
        }
    }
}