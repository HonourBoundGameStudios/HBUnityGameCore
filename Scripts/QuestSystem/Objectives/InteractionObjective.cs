using System.Collections.Generic;

namespace QuestSystem
{
    public class InteractionObjective : QuestObjective
    {
        public string targetId;
        
        public InteractionObjective(ObjectiveKey key, string targetId, string title = "", string description = "")
                        : base(
                            key,
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
}
