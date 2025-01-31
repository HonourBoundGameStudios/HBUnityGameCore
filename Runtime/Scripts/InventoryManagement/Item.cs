using System;
using UnityEngine;

namespace HBUnityGameCore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Item", menuName = "HBUnityGameCore/Inventory/Item")]
    public class Item : BaseObjectScriptableObject
    {
        [Header("OTHER PROPERTIES")]
        [SerializeField, Tooltip("A single instance of this item can exist in the game.")]
        public bool unique;

        [SerializeField, Tooltip("A single instance of this item can exist on a player.")]
        public bool uniquePerPlayer;

        [SerializeField, Tooltip("The item can be equipped by the player.")]
        public bool equipable;

        [SerializeField, Tooltip("The item can be upgraded using crafting or any other gameplay means")]
        public bool upgradeable;

        [SerializeField, Tooltip("The item can be consumed by the player")]
        public bool consumable;

        [SerializeField, Tooltip("The weight of this item in game units (kg or lbs)")]
        public float weight;

        [SerializeField, Tooltip("The value of this item in the game's currency")]
        public float value;
    }
}
