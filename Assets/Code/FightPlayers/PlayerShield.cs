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
        public bool shieldBreak;

        private float shieldRecharge;
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
            shieldBreak = false;
        }

        private void Update()
        {

        }

        private void FixedUpdate()
        {
            shieldHealth = Mathf.Clamp(shieldHealth, 0, maxShield);
            UpdateShieldUI();
            if (playerHurtboxManager.isShieldHit)
            {
                Debug.Log("Shield Hit");
                shieldHealth -= playerHurtboxManager.damageToShield;
                playerHurtboxManager.isShieldHit = false;
            }

            if (playerHurtboxManager.isStunned || playerHurtboxManager.isShieldStunned)
            {
                shieldBufferTimer = 1;
            }
            if (shieldBufferTimer > 0 && shieldBreak == false)
            {
                shieldBufferTimer -= Time.deltaTime;
            }
            if (shieldBufferTimer <= 0)
            {
                shieldBufferTimer = 0;
                shieldHealth += Time.deltaTime;
            }

            if (shieldHealth >= 100)
            {
                shieldHealth = 100;
            }

            if (playerHurtboxManager.parried)
            {
                shieldHealth = shieldHealth + 1f;
            }

            if (shieldHealth <= 0)
            {
                playerHurtboxManager.isStunned = true;
                shieldBreak = true;
                shieldRecharge = shieldRecharge + 0.2f;
            }
            if (shieldRecharge >= 100 || playerHurtboxManager.isHit)
            {
                shieldHealth = 100;
                shieldBreak = false;
                shieldRecharge = 0;
                if (!playerHurtboxManager.isHit)
                {
                    playerHurtboxManager.isStunned = false;
                }
            }
        }
        public void ResetShield()
        {
            shieldHealth = 100;
            shieldBreak = false;
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