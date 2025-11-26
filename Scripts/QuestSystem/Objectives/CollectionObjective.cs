using System.Collections.Generic;

namespace QuestSystem
{
    public class CollectionObjective : QuestObjective
    {
        public string itemId;
        
        public CollectionObjective(ObjectiveKey key, string itemId, int amount, string title = "", string description = "")
                        : base(
                            key,
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
}
