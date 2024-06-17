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
        public float shieldDamage;                                      //The maximum damage an attack does to the opponents shield

        public float horizontalKnockback;                               //How far left/right the opponent is sent
        public float verticalKnockback;                                 //How far up the opponent is sent (Made for juggling)

        public float horizontalAirKnockback;                            //How far left/right the opponent is sent while in the air
        public float verticalAirKnockback;                              //How far up/down the opponent is sent while airbourne

        public float stunTime;                                          //How long the opponent is in stun for
        public float shieldStunTime;                                    //How long the opponent is in stun for after blocking
        public float knockbackTime;                                     //How long the opponent is knocked back for

        public bool resetAerialOpponent;                                //If used on an airbourne fighter, they get "reset", forcing the combo to end

        public int attackAnimInfo;                                        //Tells the animator what animation to play. The number represents the animation played

        //Identifies the specific move used, which HurtboxManager uses to calculate how many times that specific move can hit it in a combo
        public bool isSpecialMove;
        public bool isAerialMove;
        public bool isRegularMove;
        public bool isJugglingMove;
        public bool isLowMove;

        public bool isProjectile;

        // Flag to check if the hit has already occurred
        public bool hasHit;


        //Used for the invisible boxes before the attack activates, forcing the opponent to block
        public bool isBlockBox;
        public bool isParry;


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
            else if (!hasHit && enemyTarget != null && ((1 << other.gameObject.layer) & hurtboxs) != 0 && isProjectile)
            {
                hasHit = true;
                Debug.Log("Projectile Hit");
                gameObject.SetActive(false);
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