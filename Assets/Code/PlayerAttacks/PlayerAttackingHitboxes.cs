using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Checks the attacking Hitboxes on the players. 
namespace AB
{
    public class PlayerAttackingHitboxes : MonoBehaviour
    {
        PlayerHealth playerHealth;
        PlayerHurtboxManager playerHurtboxManager;

        public LayerMask hurtboxs;

        public GameObject enemyTarget;

        //helps the Hurtbox manager identify how many times each attack hits
        private float timer;
        public float hitboxTimer;

        //calculates what kind of damage the attack does
        public float attackDamage;
        public float horizontalKnockback;
        public float verticalKnockback;
        public float stunTime;
        public float knockbackTime;

        // Flag to check if the hit has already occurred
        public bool hasHit;

        private void Start()
        {
            hasHit = false;
            timer = hitboxTimer;
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
                hasHit = true;
                Debug.Log("Hit" + other.gameObject.name);
            }
            
        }

        private void Update()
        {
            if (hasHit)
            {
                timer -= Time.deltaTime;
            }
            
            if(timer <= 0)
            {
                timer = 0;
                hasHit = false;
                timer = hitboxTimer;
            }
        }
    }
}