using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.QuestSystem
{
    public enum RewardType
    {
        Currency,
        Item,
        Experience,
        Unlock,
        Reputation,
        Cosmetic,
        Progressive
    }
    
    [System.Serializable]
    public class QuestReward
    {
        public string id;
        public RewardType type;
        public string itemId;
        public int amount;
        public string description;
        public bool scaleWithLevel;
        public Dictionary<string, object> customData = new Dictionary<string, object>();
        
        public QuestReward(RewardType rewardType, string itemId, int amount, string description = "")
        {
            this.id = Guid.NewGuid().ToString();
            this.type = rewardType;
            this.itemId = itemId;
            this.amount = amount;
            this.description = description;
        }
        
        public void GrantReward()
        {
            // This would integrate with your game's systems
            switch (type)
            {
                case RewardType.Currency:
                    // PlayerInventory.AddCurrency(itemId, amount);
                    break;
                case RewardType.Item:
                    // PlayerInventory.AddItem(itemId, amount);
                    break;
                case RewardType.Experience:
                    // PlayerStats.AddExperience(amount);
                    break;
                // Add other reward types...
            }
            
            QuestEvents.RewardGranted(this);
            Debug.Log($"Granted reward: {type} - {itemId} x{amount}");
        }
    }
}
