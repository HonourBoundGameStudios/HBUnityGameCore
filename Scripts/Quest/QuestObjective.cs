using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRGame.QuestSystem
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
        Achievement,
        Puzzle,
        TimeBased,
        Conditional
    }

    [System.Serializable]
    public abstract class QuestObjective
    {
        public string id;
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
        
        public QuestObjective(string title, string description, ObjectiveType objectiveType, int target, bool required = true, bool visible = true)
        {
            this.id = Guid.NewGuid().ToString();
            this.title = title;
            this.description = description;
            this.type = objectiveType;
            this.targetAmount = target;
            this.isRequired = required;
            this.isVisible = visible;
            this.currentProgress = 0;
            this.isCompleted = false;
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
    
// Specific Objective Types
    public class CollectionObjective : QuestObjective
    {
        public string itemId;
        
        public CollectionObjective(string itemId, int amount, string title = "", string description = "")
            : base(
                string.IsNullOrEmpty(title) ? $"Collect {amount} {itemId}" : title,
                string.IsNullOrEmpty(description) ? $"Gather {amount} {itemId} items" : description,
                ObjectiveType.Collection, amount)
        {
            this.itemId = itemId;
            parameters["itemId"] = itemId;
        }
        
        public override bool CheckCondition(Dictionary<string, object> context)
        {
            if (context.ContainsKey("itemId") && context.ContainsKey("amount"))
            {
                string contextItemId = context["itemId"].ToString();
                return contextItemId == itemId && currentProgress < targetAmount;
            }
            return false;
        }
        
        public override bool TryUpdateFromContext(Dictionary<string, object> context)
        {
            if (context.ContainsKey("itemId") && context.ContainsKey("amount"))
            {
                string contextItemId = context["itemId"].ToString();
                int amount = (int)context["amount"];
                
                if (contextItemId == itemId)
                {
                    return UpdateProgress(amount);
                }
            }
            return false;
        }
    }
    
    public class KillObjective : QuestObjective
    {
        public string enemyType;
        public string areaId;
        
        public KillObjective(string enemyType, int amount, string areaId = "", string title = "", string description = "")
            : base(
                string.IsNullOrEmpty(title) ? $"Defeat {amount} {enemyType}" : title,
                string.IsNullOrEmpty(description) ? $"Eliminate {amount} {enemyType}" + (string.IsNullOrEmpty(areaId) ? "" : $" in {areaId}") : description,
                ObjectiveType.Kill, amount)
        {
            this.enemyType = enemyType;
            this.areaId = areaId;
            parameters["enemyType"] = enemyType;
            parameters["areaId"] = areaId;
        }
        
        public override bool CheckCondition(Dictionary<string, object> context)
        {
            if (context.ContainsKey("enemyType"))
            {
                string contextEnemyType = context["enemyType"].ToString();
                string contextArea = context.ContainsKey("areaId") ? context["areaId"].ToString() : "";
                
                bool enemyMatches = contextEnemyType == enemyType;
                bool areaMatches = string.IsNullOrEmpty(areaId) || contextArea == areaId;
                
                return enemyMatches && areaMatches && currentProgress < targetAmount;
            }
            return false;
        }
        
        public override bool TryUpdateFromContext(Dictionary<string, object> context)
        {
            if (CheckCondition(context))
            {
                return UpdateProgress(1);
            }
            return false;
        }
    }
    
    public class InteractionObjective : QuestObjective
    {
        public string targetId;
        
        public InteractionObjective(string targetId, string title = "", string description = "")
            : base(
                string.IsNullOrEmpty(title) ? $"Interact with {targetId}" : title,
                string.IsNullOrEmpty(description) ? $"Use or talk to {targetId}" : description,
                ObjectiveType.Interaction, 1)
        {
            this.targetId = targetId;
            parameters["targetId"] = targetId;
        }
        
        public override bool CheckCondition(Dictionary<string, object> context)
        {
            if (context.ContainsKey("targetId"))
            {
                string contextTargetId = context["targetId"].ToString();
                return contextTargetId == targetId && !isCompleted;
            }
            return false;
        }
        
        public override bool TryUpdateFromContext(Dictionary<string, object> context)
        {
            if (CheckCondition(context))
            {
                return UpdateProgress(1);
            }
            return false;
        }
    }
    
    public class LocationObjective : QuestObjective
    {
        public string locationId;
        public Vector3 targetPosition;
        public float radius;
        
        public LocationObjective(string locationId, Vector3 position, float radius = 5f, string title = "", string description = "")
            : base(
                string.IsNullOrEmpty(title) ? $"Reach {locationId}" : title,
                string.IsNullOrEmpty(description) ? $"Travel to {locationId}" : description,
                ObjectiveType.Location, 1)
        {
            this.locationId = locationId;
            this.targetPosition = position;
            this.radius = radius;
            parameters["locationId"] = locationId;
            parameters["position"] = position;
            parameters["radius"] = radius;
        }
        
        public override bool CheckCondition(Dictionary<string, object> context)
        {
            if (context.ContainsKey("playerPosition"))
            {
                Vector3 playerPos = (Vector3)context["playerPosition"];
                float distance = Vector3.Distance(playerPos, targetPosition);
                return distance <= radius && !isCompleted;
            }
            return false;
        }
        
        public override bool TryUpdateFromContext(Dictionary<string, object> context)
        {
            if (CheckCondition(context))
            {
                return UpdateProgress(1);
            }
            return false;
        }
    }
}
