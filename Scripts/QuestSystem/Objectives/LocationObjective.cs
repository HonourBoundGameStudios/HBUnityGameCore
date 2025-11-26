using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class LocationObjective : QuestObjective
    {
        public string locationId;
        public Vector3 targetPosition;
        public float radius;
        
        public LocationObjective(ObjectiveKey key, string locationId, Vector3 position, float radius = 5f, string title = "", string description = "")
                        : base(
                            key, 
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
