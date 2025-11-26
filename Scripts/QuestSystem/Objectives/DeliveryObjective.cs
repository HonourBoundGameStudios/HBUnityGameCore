using System.Collections.Generic;

namespace QuestSystem
{
    public class DeliveryObjective : QuestObjective
    {
        public string itemId;
        public string destinationId;
        
        public DeliveryObjective(ObjectiveKey key, string itemId, string destinationId, int amount, string title = "", string description = "")
                        : base(
                            key,
                            string.IsNullOrEmpty(title) ? $"Deliver {amount} of {itemId}" : title,
                            string.IsNullOrEmpty(description) ? $"Take {amount} of {itemId} to {destinationId}" : description,
                            ObjectiveType.Delivery, amount)
        {
            this.itemId = itemId;
            this.destinationId = destinationId;
            parameters["itemId"] = itemId;
            parameters["destinationId"] = destinationId;
        }
        
        public override bool CheckCondition(Dictionary<string, object> context)
        {
            if (context.ContainsKey("itemId") && context.ContainsKey("destinationId"))
            {
                string contextItemId = context["itemId"].ToString();
                string contextDestinationId = context["destinationId"].ToString();
                
                bool itemMatches = contextItemId == itemId;
                bool destinationMatches = contextDestinationId == destinationId;
                
                return itemMatches && destinationMatches && currentProgress < targetAmount;
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
