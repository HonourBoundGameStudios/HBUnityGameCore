using System;
using UnityEngine;

namespace HBUnityGameCore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Melee Weapon", menuName = "HBUnityGameCore/Inventory/MeleeWeapon")]
    public class MeleeWeapon : Item
    {
        [Header("MELEE WEAPON")]
        [SerializeField]
        public int damage; // The damage dealt by the weapon when attacking

        [SerializeField]
        public float range; // The range of the weapon when attacking

        [SerializeField]
        public float attackSpeed; // The speed at which the weapon can be used to attack

        [SerializeField]
        public float criticalChance; // The chance of dealing a critical hit

        [SerializeField]
        public float criticalDamage; // The damage multiplier when dealing a critical hit
    }

}
