using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AB
{
    public class PlayerHurtboxManager : MonoBehaviour
    {
        PlayerAttackingHitboxes playerAttackingHitboxes;

        public float damageToHealth;
        public bool isHit;

        private void Awake()
        {
            playerAttackingHitboxes = GetComponent<PlayerAttackingHitboxes>();
        }
        private void Start()
        {
            isHit = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Get the component of type PlayerAttackingHitboxes from the collider's GameObject
            PlayerAttackingHitboxes attackerHitboxes = other.gameObject.GetComponent<PlayerAttackingHitboxes>();

            // Check if the attackerHitboxes is not null and print its attack damage
            if (attackerHitboxes != null)
            {
                damageToHealth = attackerHitboxes.attackDamage;
                isHit = true;
            }
        }
    }
}