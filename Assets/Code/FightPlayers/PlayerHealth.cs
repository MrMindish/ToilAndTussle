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

        public bool isDead;

        public bool p1Dead;
        public bool p2Dead;

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
            isDead = false;
            p1Dead = false;
            p2Dead = false;
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

            if(health <= 0)
            {
                health = 0;
                isDead = true;

                //Tells the Round Manager if a player has died, and which one died
                if(gameObject.tag == "Player1")
                {
                    P1Death();
                }
                else if (gameObject.tag == "Player2")
                {
                    P2Death();
                }
            }
            else
            {
                isDead = false;
            }
        }

        private void P1Death()
        {
            p1Dead=true;
        }

        private void P2Death()
        {
            p2Dead=true;
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
                if (!playerHurtboxManager.isStunned)
                {
                    float percentComplete = lerpTimer / chipSpeed;
                    percentComplete = percentComplete * percentComplete;
                    backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
                }

            }
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            lerpTimer = 0f;
        }
    }
}