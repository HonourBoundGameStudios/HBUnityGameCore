using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace HBUnityGameCore
{
    /// <summary>
    /// A skill is a special ability that a character can use that does consume resources (energy of any kind).
    /// A skill can have a cooldown time, a number of charges, and a recharge rate.
    /// A skill can also have a recharge booster. This is a multiplier that improves the recharge time.
    /// The recharge booster is a value between 1 and X. Anything above 1 will improve the recharge time.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "Skill", menuName = "HBUnityGameCore/Skill")]
    public class Skill : BaseObjectScriptableObject
    {
        [SerializeField, Tooltip("The maximum number of charges the skill can have.")]
        public int maxCharges;

        [SerializeField, Tooltip("The current number of charges the skill has.")]
        private int currentCharges;

        [SerializeField, Tooltip("The base recharge value of the skill.")]
        public float rechargeValue;

        [SerializeField, Tooltip("The current recharge timer of the skill.")]
        private float rechargeTimer;

        [SerializeField, Tooltip("The multiplier to decrease recharge time."), Range(1, float.MaxValue)]
        public float rechargeBooster;

        public void Initialize(int newCharges, int newMaxCharges, float newRechargeValue, float booster = 1f)
        {
            currentCharges = newCharges;
            maxCharges = newMaxCharges;
            rechargeValue = newRechargeValue;
            rechargeBooster = booster;
            rechargeTimer = 0f;
        }

        public virtual bool CanUse()
        {
            return currentCharges > 0;
        }

        /// <summary>
        /// Use the skill charge.
        /// Remove a charge from the skill and reset the recharge timer.
        /// </summary>
        public virtual void Use()
        {
            if (CanUse())
            {
                currentCharges--;
                rechargeTimer = 0f;
                Debug.Log($"{name} used with power booster of {rechargeBooster}! Charges left: {currentCharges}");
            }
        }
        /// <summary>
        /// For now, this must be called manually in the Update method of the class that uses the skill or
        /// at the user's discretion.
        /// </summary>
        public virtual void Recharge()
        {
            // Only recharge if we are not at max charges
            if (currentCharges < maxCharges)
            {
                // Advance the recharge timer... accounting for the recharge booster also.
                rechargeTimer += Time.deltaTime * rechargeBooster;

                if (rechargeTimer >= rechargeValue)
                {
                    currentCharges++;
                    rechargeTimer -= rechargeValue;
                    Debug.Log($"{name} charge gained! Charges available: {currentCharges}");
                }
            }
            else
            {
                rechargeTimer = 0f;
            }
        }

        public virtual int GetCurrentCharges()
        {
            return currentCharges;
        }
    }
}
