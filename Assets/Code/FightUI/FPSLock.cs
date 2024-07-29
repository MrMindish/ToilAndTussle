using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AB
{
    public class FPSLock : MonoBehaviour
    {
        void Start()
        {
            Application.targetFrameRate = 60;
        }
    }
}