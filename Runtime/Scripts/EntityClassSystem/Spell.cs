using System;
using UnityEngine;

namespace HBUnityGameCore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Spell", menuName = "HBUnityGameCore/Spell")]
    public class Spell : Skill
    {
        [SerializeField, Tooltip("The use cost of the spell.")]
        float _spellUseCost;

        public delegate void SpellCastDelegate(float useCost);
        public SpellCastDelegate OnSpellCastDelegate;

        public void Initialize(float spellUseCost, int charges, int newMaxCharges, float baseRechargeValue, float booster = 1, SpellCastDelegate onSpellCastDelegate = null)
        {
            base.Initialize(charges, newMaxCharges, baseRechargeValue, booster);
            _spellUseCost = spellUseCost;
            OnSpellCastDelegate = onSpellCastDelegate;
        }

        public override void Use()
        {
            base.Use();

            OnSpellCastDelegate?.Invoke(_spellUseCost);
        }
    }
}
