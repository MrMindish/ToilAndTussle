using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AB
{
    public class PlayerAttackChecker : MonoBehaviour
    {
        PlayerAttackingHitboxes playerAttackingHitboxes;
        private void Start()
        {
            playerAttackingHitboxes = GetComponentInChildren<PlayerAttackingHitboxes>();
        }
        void Update()
        {
            playerAttackingHitboxes.hasHit = false;
        }
    }
}