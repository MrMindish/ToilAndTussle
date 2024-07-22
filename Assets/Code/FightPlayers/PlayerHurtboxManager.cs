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

        public float damageToShield;
        public float shieldStunDuration;

        
        public bool isHit;
        public bool isShieldHit;
        public bool isStunned;
        public bool isKnockback;
        public bool isLaunched;
        public bool isInvincible;
        public bool isShieldStunned;

        public bool canReset;

        public int hitAnimInfo;             //1 means hitHigh, 2 means hitLow, 3 means hitLaunch. Number is recieved from AttackingHitboxes

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

        private float lowHitsCounter;
        private float maxLowHits;
        public bool canHitLow;

        // Counter for hits during stun
        public int hitCountDuringStun;

        // The maximum reduction factor for damage
        public float maxDamageReductionFactor = 0.2f;


        //Checks if the attack is blockable (Specifically, if the script is on the invisible box collider that activates before the actual attack
        public bool isBlockable;

        //If the correct variables are read, the parry is set to true
        public bool parried;

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
            isShieldStunned = false;
            isShieldHit = false;
            parried = false;

            hitAnimInfo = 0;

            aerialHitsCounter = 0;
            regularHitsCounter = 0;
            specialHitsCounter = 0;
            juggleHitsCounter = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Get the component of type PlayerAttackingHitboxes from the collider's GameObject
            PlayerAttackingHitboxes attackerHitboxes = other.gameObject.GetComponent<PlayerAttackingHitboxes>();

            isBlockable = attackerHitboxes.isBlockBox;

            // Check if the attackerHitboxes is not null and print its attack damage
            if (attackerHitboxes != null && isInvincible == false && !playerMovement.isBlocking && !isBlockable)
            {
                //Pulls various information from the attacking hitboxes
                hitAnimInfo = attackerHitboxes.attackAnimInfo;                          //The animation played upon being hit
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
                canHitLow = attackerHitboxes.isLowMove;                                 //Checks if the attack is low


                isStunned = true;
                isKnockback = true;


                stunDuration = attackerHitboxes.stunTime;
                kTime = attackerHitboxes.knockbackTime;
                isHit = true;

                if (isStunned && isHit)
                {
                    // Increase hit count during stun and calculate damage reduction
                    float damageReductionFactor = Mathf.Lerp(1f, maxDamageReductionFactor, hitCountDuringStun / 1.5f);
                    damageToHealth *= damageReductionFactor;
                }

                if (isStunned && canHitJuggle)
                {
                    juggleHitsCounter++;
                    canHitJuggle = false;
                }
            }
            else if (attackerHitboxes != null && isInvincible == false && playerMovement.isBlocking)
            {
                //Blocks the attack
                if (attackerHitboxes.isParry && playerMovement.canParry)
                {
                    Debug.Log("Spice");
                    parried = true;
                }
                else
                {
                    Debug.Log("The Bloockening");
                    //Pulls various information from the attacking hitboxes
                    damageToShield = attackerHitboxes.shieldDamage;                         //The Damage of the Attack
                    vKnockback = attackerHitboxes.verticalKnockback * 0f;                   //The Vertical (Up and Down) knockback
                    hKnockback = attackerHitboxes.horizontalKnockback * 0.9f;               //The Horizontal (Left and Right) knockback


                    canHitRegular = attackerHitboxes.isRegularMove;                         //Checks if the attack is Light or Heavy
                    canHitSpecial = attackerHitboxes.isSpecialMove;                         //Checks if the attack is special
                    canHitAerial = attackerHitboxes.isAerialMove;                           //Checks if the attack is Aerial
                    canHitJuggle = attackerHitboxes.isJugglingMove;                         //Checks if the attack can launch fighters upwards

                    isShieldStunned = true;
                    isKnockback = true;
                    if (isShieldStunned)
                    {
                        float damageReductionFactor = Mathf.Lerp(1f, maxDamageReductionFactor, hitCountDuringStun / 1f);
                        damageToShield *= damageReductionFactor;
                    }

                    stunDuration = attackerHitboxes.shieldStunTime;
                    kTime = attackerHitboxes.knockbackTime;
                    isShieldHit = true;
                }

            }
        }

        private void OnTriggerExit(Collider other)
        {
             // Get the component of type PlayerAttackingHitboxes from the collider's GameObject
             PlayerAttackingHitboxes attackerHitboxes = other.gameObject.GetComponent<PlayerAttackingHitboxes>();

            isBlockable = false;
        }


        private void Update()
        {
            Debug.Log(hitCountDuringStun);

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

            if (isShieldStunned)
            {
                stunDuration -= Time.deltaTime;
                if (stunDuration <= 0)
                {
                    isShieldStunned = false;
                    stunDuration = 0;
                }
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

            if(isStunned && isHit)
            {
                hitCountDuringStun++;
            }
            else if (!isStunned)
            {
                hitCountDuringStun = 0;
            }
        }
    }
}