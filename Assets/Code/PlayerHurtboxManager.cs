using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AB
{
    public class PlayerHurtboxManager : MonoBehaviour
    {
        PlayerAttackingHitboxes playerAttackingHitboxes;

        //Recieves the data from the Attacking Hitboxes
        public float damageToHealth;
        public float stunDuration;
        public float vKnockback;
        public float hKnockback;
        public float kTime;

        
        public bool isHit;
        public bool isStunned;
        public bool isKnockback;

        private float knockbackTimer;

        // Counter for hits during stun
        private int hitCountDuringStun = 0;

        // The maximum reduction factor for damage
        public float maxDamageReductionFactor = 0.5f;

        private void Awake()
        {
            playerAttackingHitboxes = GetComponent<PlayerAttackingHitboxes>();
        }

        private void Start()
        {
            isHit = false;
            isStunned = false;
            isKnockback = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Get the component of type PlayerAttackingHitboxes from the collider's GameObject
            PlayerAttackingHitboxes attackerHitboxes = other.gameObject.GetComponent<PlayerAttackingHitboxes>();

            // Check if the attackerHitboxes is not null and print its attack damage
            if (attackerHitboxes != null)
            {
                damageToHealth = attackerHitboxes.attackDamage;
                vKnockback = attackerHitboxes.verticalKnockback;
                hKnockback = attackerHitboxes.horizontalKnockback;

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
            }
        }

        private void Update()
        {
            if (isStunned)
            {
                stunDuration -= Time.deltaTime;
                if (stunDuration <= 0)
                {
                    isStunned = false;
                }
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