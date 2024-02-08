using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor.SceneManagement;
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



        void Start()
        {
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
            SceneManager.LoadScene("TestingFighting");
        }

        public void EnterStory()
        {
            Debug.Log("Story Selected");
        }

        public void EnterMore()
        {
            Debug.Log("More Selected");
        }
    }
}