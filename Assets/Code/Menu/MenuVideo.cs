using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace AB
{
    public class MenuVideo : MonoBehaviour
    {
        /* The video must change what it's playing when the mouse goes over one of the menu buttons
           This can be down by getting components and setting the various videos active in the "VideoPlayer" Component */
        public ButtonHover buttonHover;

        RawImage m_RawImage;
        public Texture m_Texture;
        void Start()
        {
            m_RawImage = GetComponent<RawImage>();
            m_RawImage.texture = m_Texture;
        }
    }
}

