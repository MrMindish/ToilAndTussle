using AB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AB
{
    public class PlayerFreeze : MonoBehaviour
    {

        PlayerMovement playerMovement;
        PlayerHealth playerHealth;

        private void Awake()
        {
            playerMovement = GetComponentInChildren<PlayerMovement>();
            playerHealth = GetComponentInChildren<PlayerHealth>();
        }

        void Update()
        {
            if (playerHealth.isDead)
            {
                playerMovement.gameObject.SetActive(false);
                Debug.Log("Hella Dead");
            }
        }
    }
}