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

        public void OnTriggerEnter(Collider other)
        {
            // Check if the hit has not occurred and the collider belongs to the hurtbox layer
            if (!hasHit && enemyTarget != null && ((1 << other.gameObject.layer) & hurtboxs) != 0)
            {
                playerHealth.health -= attackDamage;
                hasHit = true;
                StartCoroutine(hasHitTimer());
                Debug.Log("Hit" + other.gameObject.name);
            }
            
        }

        public IEnumerator hasHitTimer()
        {
            
            
            yield return new WaitForSeconds(0.6f);
            Debug.Log("WHYNOT");
            hasHit = false;
        }
    }
}