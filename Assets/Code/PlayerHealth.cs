using AB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AB

// Player Health script. Calculates the player's health
{
    public class PlayerHealth : MonoBehaviour
    {
        private float health;
        private float lerpTimer;
        public float maxHealth = 1000f;
        public float chipSpeed = 20f;

        public Image frontHealthBar;
        public Image backHealthBar;

        PlayerAttackingHitboxes playerAttackingHitboxes;
        PlayerHurtboxManager playerHurtboxManager;

        private void Awake()
        {
            playerAttackingHitboxes = GetComponentInChildren<PlayerAttackingHitboxes>();
            playerHurtboxManager = GetComponentInChildren<PlayerHurtboxManager>();
        }


        private void Start()
        {
            health = maxHealth;
        }

        private void Update()
        {
            health = Mathf.Clamp(health, 0, maxHealth);
            UpdateHealthUI();
            if (playerHurtboxManager.isHit)
            {
                health -= playerHurtboxManager.damageToHealth;
                playerHurtboxManager.isHit = false;
            }
        }

        public void UpdateHealthUI()
        {
            Debug.Log(health);
            float fillF = frontHealthBar.fillAmount;
            float fillB = backHealthBar.fillAmount;
            float hFraction = health / maxHealth;
            if(fillB > hFraction)
            {
                //manages the chip timer
                frontHealthBar.fillAmount = hFraction;
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / chipSpeed;
                percentComplete = percentComplete * percentComplete;
                backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
            }
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            lerpTimer = 0f;
        }
    }
}