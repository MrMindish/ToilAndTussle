using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AB
{
    public class MenuManager : MonoBehaviour
    {
        public bool mainMenu;
        public bool stageMenu;

        private Animator animator;

        private void Start()
        {
            mainMenu = false;
            stageMenu = false;
            animator = GetComponent<Animator>();
        }

        public void Update()
        {
            if(mainMenu == true)
            {
                animator.SetTrigger("HomeMenu");
                mainMenu = false;
            }
            else if(stageMenu == true)
            {
                animator.SetTrigger("StageMenu");
                stageMenu = false;
            }
        }
    }
}