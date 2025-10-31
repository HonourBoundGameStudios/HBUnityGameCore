using System;
using UnityEngine;

namespace VRGame.QuestSystem
{
    public class QuestBuilder
    {
        private Quest quest;
        
        public QuestBuilder(string title, string description, QuestType type = QuestType.SideQuest)
        {
            quest = new Quest(title, description, type);
        }
        
        public QuestBuilder SetPriority(int priority)
        {
            quest.priority = priority;
            return this;
        }
        
        public QuestBuilder SetRequiredLevel(int level)
        {
            quest.requiredLevel = level;
            return this;
        }
        
        public QuestBuilder AddRequiredQuest(string questId)
        {
            quest.requiredQuests.Add(questId);
            return this;
        }
        
        public QuestBuilder SetAutoAccept(bool autoAccept)
        {
            quest.autoAccept = autoAccept;
            return this;
        }
        
        public QuestBuilder SetAutoComplete(bool autoComplete)
        {
            quest.autoComplete = autoComplete;
            return this;
        }
        
        public QuestBuilder SetExpiration(DateTime expiration)
        {
            quest.expirationDate = expiration;
            return this;
        }
        
        public QuestBuilder SetRepeatable(bool repeatable)
        {
            quest.isRepeatable = repeatable;
            return this;
        }
        
        public QuestBuilder AddObjective(QuestObjective objective)
        {
            quest.objectives.Add(objective);
            return this;
        }
        
        public QuestBuilder AddCollectionObjective(string itemId, int amount, string title = "", string description = "")
        {
            quest.objectives.Add(new CollectionObjective(itemId, amount, title, description));
            return this;
        }
        
        public QuestBuilder AddKillObjective(string enemyType, int amount, string areaId = "", string title = "", string description = "")
        {
            quest.objectives.Add(new KillObjective(enemyType, amount, areaId, title, description));
            return this;
        }
        
        public QuestBuilder AddInteractionObjective(string targetId, string title = "", string description = "")
        {
            quest.objectives.Add(new InteractionObjective(targetId, title, description));
            return this;
        }
        
        public QuestBuilder AddLocationObjective(string locationId, Vector3 position, float radius = 5f, string title = "", string description = "")
        {
            quest.objectives.Add(new LocationObjective(locationId, position, radius, title, description));
            return this;
        }
        
        public QuestBuilder AddReward(QuestReward reward)
        {
            quest.rewards.Add(reward);
            return this;
        }
        
        public QuestBuilder AddCurrencyReward(string currencyType, int amount, string description = "")
        {
            quest.rewards.Add(new QuestReward(RewardType.Currency, currencyType, amount, description));
            return this;
        }
        
        public QuestBuilder AddItemReward(string itemId, int amount, string description = "")
        {
            quest.rewards.Add(new QuestReward(RewardType.Item, itemId, amount, description));
            return this;
        }
        
        public QuestBuilder AddExperienceReward(int amount, string description = "")
        {
            quest.rewards.Add(new QuestReward(RewardType.Experience, "xp", amount, description));
            return this;
        }
        
        public QuestBuilder AddFollowUpQuest(string questId)
        {
            quest.followUpQuests.Add(questId);
            return this;
        }
        
        public QuestBuilder SetPreviousQuest(string questId)
        {
            quest.previousQuestId = questId;
            return this;
        }
        
        public Quest Build()
        {
            return quest;
        }
        
        public QuestBuilder SetId(string questId)
        {
            quest.id = questId;
            return this;
        }
    }
}
