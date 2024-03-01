using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AB
{
    public class PlayerHurtboxManager : MonoBehaviour
    {
        PlayerAttackingHitboxes playerAttackingHitboxes;
        PlayerMovement playerMovement;

        //Recieves the data from the Attacking Hitboxes
        public float damageToHealth;
        public float stunDuration;
        public float vKnockback;
        public float hKnockback;
        public float kTime;
        public float vAirKnockback;
        public float hAirKnockback;
        
        public bool isHit;
        public bool isStunned;
        public bool isKnockback;
        public bool isLaunched;
        public bool isInvincible;

        public bool canReset;

        private float knockbackTimer;


        //Checks how many times specific moves can hit the fighter in one combo
        private float specialHitsCounter;
        private float maxSpecialHits;
        public bool canHitSpecial;

        private float aerialHitsCounter;
        private float maxAerialHits;
        public bool canHitAerial;

        private float regularHitsCounter;
        private float maxRegularHits;
        public bool canHitRegular;

        private float juggleHitsCounter;
        private float maxJuggleHits;
        public bool canHitJuggle;

        // Counter for hits during stun
        private int hitCountDuringStun = 0;

        // The maximum reduction factor for damage
        public float maxDamageReductionFactor = 0.5f;

        private void Awake()
        {
            playerAttackingHitboxes = GetComponent<PlayerAttackingHitboxes>();
            playerMovement = GetComponentInParent<PlayerMovement>();
        }

        private void Start()
        {
            isHit = false;
            isStunned = false;
            isKnockback = false;
            isLaunched = false;
            isInvincible = false;
            canReset = false;

            aerialHitsCounter = 0;
            regularHitsCounter = 0;
            specialHitsCounter = 0;
            juggleHitsCounter = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Get the component of type PlayerAttackingHitboxes from the collider's GameObject
            PlayerAttackingHitboxes attackerHitboxes = other.gameObject.GetComponent<PlayerAttackingHitboxes>();

            // Check if the attackerHitboxes is not null and print its attack damage
            if (attackerHitboxes != null && isInvincible == false)
            {
                //Pulls various information from the attacking hitboxes
                damageToHealth = attackerHitboxes.attackDamage;                         //The Damage of the Attack
                vKnockback = attackerHitboxes.verticalKnockback;                        //The Vertical (Up and Down) knockback
                hKnockback = attackerHitboxes.horizontalKnockback;                      //The Horizontal (Left and Right) knockback
                vAirKnockback = attackerHitboxes.verticalAirKnockback;                  //The Vertical Knockback while in the Air
                hAirKnockback = attackerHitboxes.horizontalAirKnockback;                //The Horizontal Knockback while in the Air
                if (!playerMovement.IsGrounded())
                {
                    canReset = attackerHitboxes.resetAerialOpponent;                    //Checks if the attack resets airbourne fighters
                }


                canHitRegular = attackerHitboxes.isRegularMove;                         //Checks if the attack is Light or Heavy
                canHitSpecial = attackerHitboxes.isSpecialMove;                         //Checks if the attack is special
                canHitAerial = attackerHitboxes.isAerialMove;                           //Checks if the attack is Aerial
                canHitJuggle = attackerHitboxes.isJugglingMove;                         //Checks if the attack can launch fighters upwards


                isStunned = true;
                isKnockback = true;
                if (isStunned)
                {
                    // Increase hit count during stun and calculate damage reduction
                    hitCountDuringStun++;
                    float damageReductionFactor = Mathf.Lerp(1f, maxDamageReductionFactor, hitCountDuringStun / 1f);
                    damageToHealth *= damageReductionFactor;
                }

                stunDuration = attackerHitboxes.stunTime;
                kTime = attackerHitboxes.knockbackTime;
                isHit = true;
                // Reset hit count when entering stun
                hitCountDuringStun = 0;


                if(isStunned && canHitJuggle)
                {
                    juggleHitsCounter++;
                    canHitJuggle = false;
                }
            }
        }

        private void Update()
        {
            if (isStunned && playerMovement.IsGrounded() && !isLaunched)
            {
                stunDuration -= Time.deltaTime;
                if (stunDuration <= 0)
                {
                    isStunned = false;
                }
            }
            else if (isStunned && playerMovement.IsGrounded() && isLaunched)
            {
                stunDuration = 0;
                playerMovement.canRecover = true;
            }
            
            if (isStunned && !playerMovement.IsGrounded())
            {
                isLaunched = true;
            }

            if (isKnockback)
            {
                kTime -= Time.deltaTime;
                if(kTime <= 0)
                {
                    isKnockback = false;
                }
            }
        }
    }
}