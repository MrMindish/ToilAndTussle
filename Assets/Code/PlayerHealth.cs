using AB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AB
{
    public class PlayerHealth : MonoBehaviour
    {
        public int health;

        PlayerAttackingHitboxes playerAttackingHitboxes;

        private void Awake()
        {
            playerAttackingHitboxes = GetComponentInChildren<PlayerAttackingHitboxes>();
        }

    }
}