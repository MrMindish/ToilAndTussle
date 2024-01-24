using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AB
{
    public class VersusButton : MonoBehaviour
    {
        public GameObject VersusIcon1;
        public GameObject VersusIcon2;

        private void Awake()
        {
            VersusIcon1 = GetComponentInChildren<GameObject>();
            VersusIcon2 = GetComponentInChildren<GameObject>();
        }

        private void Update()
        {
            if (VersusIcon2 == false)
            {
                Debug.Log("works");
            }
        }

        private void OnMouseEnter()
        {
            Debug.Log("works");
        }
    }
}