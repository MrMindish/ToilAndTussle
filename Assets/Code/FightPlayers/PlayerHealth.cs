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
        public float chipSpeed = 10f;
        public float healthRestore = 1000f;

        public bool isDead;

        public bool p1Dead;
        public bool p2Dead;

        public Image frontHealthBar;
        public Image backHealthBar;

        PlayerAttackingHitboxes playerAttackingHitboxes;
        PlayerHurtboxManager playerHurtboxManager;
        RoundManager roundManager;

        private void Awake()
        {
            playerAttackingHitboxes = GetComponentInChildren<PlayerAttackingHitboxes>();
            playerHurtboxManager = GetComponentInChildren<PlayerHurtboxManager>();

            roundManager = GetComponentInParent<RoundManager>();
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
                if(gameObject.tag == "Player1" && p1Dead == false)
                {
                    p1Dead=true;
                    StartCoroutine(RoundManager.instance.P2WinSequence());
                }
                else if (gameObject.tag == "Player2" && p2Dead == false)
                {
                    p2Dead=true;
                    StartCoroutine(RoundManager.instance.P1WinSequence());
                }
            }
            else
            {
                isDead = false;
                p1Dead = false;
                p2Dead = false;
            }

            

            if (roundManager.playerOneWins)         //Tells the winner if the other player is dead
            {
                p2Dead = true;
            }
            else if (roundManager.playerTwoWins)    //Tells the winner if the other player is dead
            {
                p1Dead = true;
            }
        }

        public void HealthReset()
        {
            if (gameObject.tag == "Player1")
            {
                health = maxHealth;
            }
            else if(gameObject.tag == "Player2")
            {
                health = maxHealth;
            }

            isDead = false;
            p1Dead = false;
            p2Dead = false;
            Debug.Log("Reset Health Through Code");
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
            if(fillF < hFraction)
            {
                backHealthBar.fillAmount = hFraction;
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / chipSpeed;
                percentComplete = percentComplete * percentComplete;
                frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
            }
            else if(fillB == hFraction)
            {
                lerpTimer = 0;
            }
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            lerpTimer = 0f;
        }

        public void RestoreHealth(float restoreAmount)
        {
            restoreAmount = 1000 - health;
            health += restoreAmount;
            lerpTimer = 0;
        }

    }
}