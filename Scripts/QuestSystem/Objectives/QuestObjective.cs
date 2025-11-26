using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using Object = System.Object;

namespace QuestSystem
{

    public enum ObjectiveType
    {
        Collection,
        Kill,
        Delivery,
        Interaction,
        Location,
        Escort,
        Crafting,
        Puzzle,
        TimeBased
    }

    [CreateAssetMenu(menuName = "Quests/Objective Key")]
    public class ObjectiveKey : ScriptableObject
    {
        [Tooltip("Human-readable unique ID for designers")]
        public string id;
    }

    [Serializable]
    public abstract class QuestObjective
    {
        public ObjectiveKey key;
        public string title;
        public string description;
        public ObjectiveType type;
        public int currentProgress;
        public int targetAmount;
        public bool isRequired;
        public bool isVisible;
        public bool isCompleted;
        public Dictionary<string, object> parameters = new Dictionary<string, object>();
        public List<QuestReward> rewards = new List<QuestReward>();
        
        public QuestObjective(ObjectiveKey key, string title, string description, ObjectiveType objectiveType, int target, bool required = true, bool visible = true)
        {
            this.key = key;
            this.title = title;
            this.description = description;
            type = objectiveType;
            targetAmount = target;
            isRequired = required;
            isVisible = visible;
            currentProgress = 0;
            isCompleted = false;
        }
        
        public virtual bool UpdateProgress(int amount = 1)
        {
            if (isCompleted) return false;
            
            currentProgress = Mathf.Min(currentProgress + amount, targetAmount);
            
            if (currentProgress >= targetAmount && !isCompleted)
            {
                CompleteObjective();
                return true;
            }
            
            return false;
        }
        
        public virtual void CompleteObjective()
        {
            isCompleted = true;
            
            // Grant objective rewards
            foreach (var reward in rewards)
            {
                reward.GrantReward();
            }
        }
        
        public virtual bool CanComplete()
        {
            return currentProgress >= targetAmount;
        }
        
        public float GetProgressPercentage()
        {
            return targetAmount > 0 ? (float)currentProgress / targetAmount : 0f;
        }
        
        // Override this for custom objective logic - ONLY checks if condition is met
        public virtual bool CheckCondition(Dictionary<string, object> context)
        {
            return CanComplete();
        }
        
        // Override this to handle context-based progress updates
        public virtual bool TryUpdateFromContext(Dictionary<string, object> context)
        {
            // Base implementation does nothing - override in specific objective types
            return false;
        }
    }


}
