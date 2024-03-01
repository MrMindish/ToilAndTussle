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
        public float attackDamage;                                      //The maximum damage the attack can do

        public float horizontalKnockback;                               //How far left/right the opponent is sent
        public float verticalKnockback;                                 //How far up the opponent is sent (Made for juggling)

        public float horizontalAirKnockback;                            //How far left/right the opponent is sent while in the air
        public float verticalAirKnockback;                              //How far up/down the opponent is sent while airbourne

        public float stunTime;                                          //How long the opponent is in stun for
        public float knockbackTime;                                     //How long the opponent is knocked back for

        //Identifies the specific move used, which HurtboxManager uses to calculate how many times that specific move can hit it in a combo
        public bool isSpecialMove;
        public bool isAerialMove;
        public bool isRegularMove;
        public bool isJugglingMove;

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