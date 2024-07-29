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

        public bool comboHit;

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
            PlayerAttackingHitboxes attackerHitboxes = other.gameObject.GetComponent<PlayerAttackingHitboxes>();

            // Check if attackerHitboxes is null
            if (attackerHitboxes == null)
            {
                Debug.LogWarning("Collider does not have PlayerAttackingHitboxes component.");
                return;
            }

            isBlockable = attackerHitboxes.isBlockBox;

            if (!isInvincible && !playerMovement.isBlocking && !isBlockable)
            {
                hitAnimInfo = attackerHitboxes.attackAnimInfo;
                damageToHealth = attackerHitboxes.attackDamage;
                vKnockback = attackerHitboxes.verticalKnockback;
                hKnockback = attackerHitboxes.horizontalKnockback;
                vAirKnockback = attackerHitboxes.verticalAirKnockback;
                hAirKnockback = attackerHitboxes.horizontalAirKnockback;

                if (!playerMovement.IsGrounded())
                {
                    canReset = attackerHitboxes.resetAerialOpponent;
                }

                canHitRegular = attackerHitboxes.isRegularMove;
                canHitSpecial = attackerHitboxes.isSpecialMove;
                canHitAerial = attackerHitboxes.isAerialMove;
                canHitJuggle = attackerHitboxes.isJugglingMove;
                canHitLow = attackerHitboxes.isLowMove;

                isStunned = true;
                isKnockback = true;

                stunDuration = attackerHitboxes.stunTime;
                kTime = attackerHitboxes.knockbackTime;
                isHit = true;
                comboHit = true;

                if (isStunned && isHit)
                {
                    float damageReductionFactor = Mathf.Lerp(1f, maxDamageReductionFactor, hitCountDuringStun / 1.5f);
                    damageToHealth *= damageReductionFactor;
                }

                if (isStunned && canHitJuggle)
                {
                    juggleHitsCounter++;
                    canHitJuggle = false;
                }
            }
            else if (!isInvincible && playerMovement.isBlocking)
            {
                if (attackerHitboxes.isParry && playerMovement.canParry)
                {
                    Debug.Log("Spice");
                    parried = true;
                }
                else
                {
                    Debug.Log("The Blockening");
                    damageToShield = attackerHitboxes.shieldDamage;
                    vKnockback = attackerHitboxes.verticalKnockback * 0f;
                    hKnockback = attackerHitboxes.horizontalKnockback * 0.9f;

                    canHitRegular = attackerHitboxes.isRegularMove;
                    canHitSpecial = attackerHitboxes.isSpecialMove;
                    canHitAerial = attackerHitboxes.isAerialMove;
                    canHitJuggle = attackerHitboxes.isJugglingMove;

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
            // No need to check if attackerHitboxes is null in OnTriggerExit, but you can keep it for consistency
            PlayerAttackingHitboxes attackerHitboxes = other.gameObject.GetComponent<PlayerAttackingHitboxes>();

            if (attackerHitboxes == null)
            {
                Debug.LogWarning("Collider exiting does not have PlayerAttackingHitboxes component.");
                return;
            }

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

            if(isStunned && comboHit)
            {
                hitCountDuringStun++;
                comboHit = false;
            }
            else if (!isStunned)
            {
                hitCountDuringStun = 0;
            }
        }
    }
}