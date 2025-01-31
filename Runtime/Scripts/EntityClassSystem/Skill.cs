using System;
using UnityEngine;

namespace HBUnityGameCore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Skill", menuName = "HBUnityGameCore/Skill")]
    public class Skill : BaseObjectScriptableObject
    {
        [SerializeField, Tooltip("The base cooldown time of the skill.")]
        public float baseCooldownTime;

        [SerializeField, Tooltip("The maximum number of charges the skill can have.")]
        public int maxCharges;

        [SerializeField, Tooltip("The current number of charges the skill has.")]
        private int currentCharges;

        [SerializeField, Tooltip("The next time the skill can be used.")]
        private float nextUseTime;

        [SerializeField, Tooltip("The base recharge rate of the skill.")]
        public float baseRechargeRate;

        [SerializeField, Tooltip("The current recharge timer of the skill.")]
        private float rechargeTimer;

        [field: SerializeField]
        [field: Tooltip("The multiplier to increase skill power.")]
        public float PowerBooster
        {
            set; get;
        }

        [field: SerializeField]
        [field: Tooltip("The multiplier to decrease recharge time.")]
        public float RechargeBooster
        {
            set; get;
        }

        public Skill(float baseCooldownTime, int maxCharges, float baseRechargeRate, float powerBooster = 1f, float rechargeBooster = 1f)
        {
            this.baseCooldownTime = baseCooldownTime;
            this.maxCharges = maxCharges;
            this.currentCharges = maxCharges;
            this.baseRechargeRate = baseRechargeRate;
            this.PowerBooster = powerBooster;
            this.RechargeBooster = rechargeBooster;
            this.nextUseTime = 0f;
            this.rechargeTimer = 0f;
        }

        public bool CanUse()
        {
            return currentCharges > 0 && Time.time >= nextUseTime;
        }

        public void Use()
        {
            if (CanUse())
            {
                currentCharges--;
                nextUseTime = Time.time + baseCooldownTime / RechargeBooster; // Apply rechargeBooster to cooldown time
                rechargeTimer = 0f; // Reset recharge timer after use
                Debug.Log($"{name} used with power booster of {PowerBooster}! Charges left: {currentCharges}");
            }
        }

        public void Recharge()
        {
            if (currentCharges < maxCharges)
            {
                rechargeTimer += Time.deltaTime * RechargeBooster; // Apply rechargeBooster to recharge rate
                if (rechargeTimer >= baseRechargeRate)
                {
                    currentCharges++;
                    rechargeTimer = 0f;
                    Debug.Log($"{name} partially recharged! Charges: {currentCharges}");
                }
            }
        }

        public float GetCurrentRechargeValue()
        {
            return rechargeTimer / baseRechargeRate;
        }
    }
}
