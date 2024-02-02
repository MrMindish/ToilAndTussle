using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AB
{
    public class PlayerAttackingHitboxes : MonoBehaviour
    {
        PlayerHealth playerHealth;
        PlayerHurtboxManager playerHurtboxManager;

        public LayerMask hurtboxs;

        public GameObject enemyTarget;
        public bool isStunned;
        public bool isHit;

        public int attackDamage;
        public int horizontalKnockback;
        public int verticalKnockback;

        // Flag to check if the hit has already occurred
        private bool hasHit;

        private void Start()
        {
            isStunned = false;
            isHit = false;
            hasHit = false;
        }

        private void Awake()
        {
            playerHealth = GetComponentInParent<PlayerHealth>();
            playerHurtboxManager = GetComponent<PlayerHurtboxManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            // Check if the hit has not occurred and the collider belongs to the hurtbox layer
            if (!hasHit && enemyTarget != null && ((1 << other.gameObject.layer) & hurtboxs) != 0)
            {
                // Reduce player health
                playerHealth.health -= attackDamage;

                // Set the flag to true to indicate that the hit has occurred
                hasHit = true;

                // Debug log for testing
                Debug.Log("Hit " + other.gameObject.name);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (!hasHit && enemyTarget != null && ((1 << other.gameObject.layer) & hurtboxs) != 0)
            {
                hasHit = false;
            }
        }
    }
}