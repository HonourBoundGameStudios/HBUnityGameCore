using System.Collections.Generic;

namespace QuestSystem
{
    public class KillObjective : QuestObjective
    {
        public string enemyType;
        public string areaId;
        
        public KillObjective(ObjectiveKey key, string enemyType, int amount, string areaId = "", string title = "", string description = "")
                        : base(
                            key,
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
}
