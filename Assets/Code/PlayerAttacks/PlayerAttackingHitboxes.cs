using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AB
{
    public class PlayerAttackingHitboxes : MonoBehaviour
    {
        PlayerHealth playerHealth;

        public GameObject enemyTarget;
        public bool isStunned;

        public int attackDamage;
        public int horizontalKnockback;
        public int verticalKnockback;

        //The code checks if the Trigger hitbox is colliding with the enemies hitbox. If true, the enemy is put into stun and takes damage

        private void Start()
        {
            isStunned = false;
        }
        private void Awake()
        {
            playerHealth = GetComponentInParent<PlayerHealth>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(enemyTarget != null && other.CompareTag("Hurtbox"))
            {
                playerHealth.health = playerHealth.health - attackDamage;
                Debug.Log("Hit" + other.gameObject.name);
            }
        }

    }
}