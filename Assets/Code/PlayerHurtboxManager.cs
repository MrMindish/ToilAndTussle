using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AB
{
    public class PlayerHurtboxManager : MonoBehaviour
    {
        PlayerAttackingHitboxes playerAttackingHitboxes;

        public float damageToHealth;
        public float stunDuration;

        public bool isHit;
        public bool isStunned;

        // Counter for hits during stun
        private int hitCountDuringStun = 0;

        // The maximum reduction factor for damage
        public float maxDamageReductionFactor = 0.02f;

        private void Awake()
        {
            playerAttackingHitboxes = GetComponent<PlayerAttackingHitboxes>();
        }

        private void Start()
        {
            isHit = false;
            isStunned = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Get the component of type PlayerAttackingHitboxes from the collider's GameObject
            PlayerAttackingHitboxes attackerHitboxes = other.gameObject.GetComponent<PlayerAttackingHitboxes>();

            // Check if the attackerHitboxes is not null and print its attack damage
            if (attackerHitboxes != null)
            {
                damageToHealth = attackerHitboxes.attackDamage;

                if (isStunned)
                {
                    // Increase hit count during stun and calculate damage reduction
                    hitCountDuringStun++;
                    float damageReductionFactor = Mathf.Lerp(1f, maxDamageReductionFactor, hitCountDuringStun / 0.4f);
                    damageToHealth *= damageReductionFactor;
                }

                stunDuration = attackerHitboxes.stunTime;
                isHit = true;
                isStunned = true;
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
        }
    }
}