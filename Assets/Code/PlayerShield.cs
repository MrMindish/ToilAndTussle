using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AB
{
    public class PlayerShield : MonoBehaviour
    {
        private float shieldHealth;
        public float maxShield = 100f;

        private int shieldRecharge;
        public float shieldBufferTimer;                    //Gives a full second before resuming the shield recharge

        public Image shieldBar;

        PlayerAttackingHitboxes playerAttackingHitboxes;
        PlayerHurtboxManager playerHurtboxManager;

        private void Awake()
        {
            playerAttackingHitboxes = GetComponentInChildren<PlayerAttackingHitboxes>();
            playerHurtboxManager = GetComponentInChildren<PlayerHurtboxManager>();
        }
        private void Start()
        {
            shieldHealth = maxShield;
            shieldBufferTimer = 0;
        }

        private void Update()
        {
            shieldHealth = Mathf.Clamp(shieldHealth, 0, maxShield);
            UpdateShieldUI();
            if (playerHurtboxManager.isShieldHit)
            {
                shieldHealth -= playerHurtboxManager.damageToShield;
                playerHurtboxManager.isShieldHit = false;
            }

            if (playerHurtboxManager.isStunned || playerHurtboxManager.isShieldStunned)
            {
                shieldBufferTimer = 1;
            }

            if(shieldBufferTimer > 0)
            {
                shieldBufferTimer -= Time.deltaTime;
            }

            if(shieldBufferTimer <= 0)
            {
                shieldBufferTimer = 0;
                shieldHealth += Time.deltaTime;
            }

            if(shieldHealth >= 100)
            {
                shieldHealth = 100;
            }

            if (playerHurtboxManager.parried)
            {
                shieldHealth = shieldHealth + 0.1f;
            }
        }


        public void UpdateShieldUI()
        {
            float fillF = shieldBar.fillAmount;
            float hFraction = shieldHealth / maxShield;
            if (fillF > hFraction)
            {
                //manages the chip timer
                shieldBar.fillAmount = hFraction;
            }
            else if(fillF < hFraction)
            {
                shieldBar.fillAmount = hFraction;
            }
        }

        public void TakeDamage(float damage)
        {
            shieldHealth -= damage;
        }

    }
}