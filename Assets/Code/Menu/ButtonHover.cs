using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using AB;
using UnityEngine.Video;

namespace AB
{


    public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Images and Sprites - Buttons")]
        [Space]
        public Sprite normalSprite;
        public Sprite hoverSprite;
        public Sprite pressedSprite;
        private Image buttonImage;

        [Header("Menu Videos")]
        [Space]
        public RawImage VersusVideo;
        public RenderTexture OtherVideo;

        MenuManager menuManager;



        void Start()
        {
            menuManager = GetComponentInParent<MenuManager>();

            buttonImage = GetComponent<Image>();
            buttonImage.sprite = normalSprite;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            buttonImage.sprite = hoverSprite;
            VersusVideo.texture = OtherVideo;
        }

        public void OnPointerExit(PointerEventData eventData)
        {

            buttonImage.sprite = normalSprite;
        }

        public void EnterVersus()
        {
            menuManager.stageMenu = true;
        }

        public void EnterStory()
        {
            Debug.Log("Story Selected");
        }

        public void EnterMore()
        {
            Debug.Log("More Selected");
        }

        public void SelectStageHut()
        {
            SceneManager.LoadScene("HutFighting");
        }
        public void SelectStageBar()
        {
            SceneManager.LoadScene("PubFighting");
        }
        public void SelectStageRoof()
        {
            SceneManager.LoadScene("TestingFighting");
        }
        public void ReturnToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
        public void BackStages()
        {
            menuManager.mainMenu = true;
        }

        public void ResetGame()
        {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}