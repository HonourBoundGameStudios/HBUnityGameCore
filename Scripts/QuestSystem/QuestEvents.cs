using System;

namespace QuestSystem
{
    public static class QuestEvents
    {
        public static event Action<Quest> OnQuestStarted;
        public static event Action<Quest> OnQuestCompleted;
        public static event Action<Quest> OnQuestFailed;
        public static event Action<Quest> OnQuestAbandoned;
        public static event Action<Quest, QuestObjective> OnObjectiveCompleted;
        public static event Action<Quest, QuestObjective> OnObjectiveProgressUpdated;
        public static event Action<QuestReward> OnRewardGranted;
        public static event Action<Quest, Quest> OnQuestChainProgressed;
        
        public static void QuestStarted(Quest quest) => OnQuestStarted?.Invoke(quest);
        public static void QuestCompleted(Quest quest) => OnQuestCompleted?.Invoke(quest);
        public static void QuestFailed(Quest quest) => OnQuestFailed?.Invoke(quest);
        public static void QuestAbandoned(Quest quest) => OnQuestAbandoned?.Invoke(quest);
        public static void ObjectiveCompleted(Quest quest, QuestObjective objective) => OnObjectiveCompleted?.Invoke(quest, objective);
        public static void ObjectiveProgressUpdated(Quest quest, QuestObjective objective) => OnObjectiveProgressUpdated?.Invoke(quest, objective);
        public static void RewardGranted(QuestReward reward) => OnRewardGranted?.Invoke(reward);
        public static void QuestChainProgressed(Quest fromQuest, Quest toQuest) => OnQuestChainProgressed?.Invoke(fromQuest, toQuest);
        
        public static void ClearAllSubscribers()
        {
            OnQuestStarted = null;
            OnQuestCompleted = null;
            OnQuestFailed = null;
            OnQuestAbandoned = null;
            OnObjectiveCompleted = null;
            OnObjectiveProgressUpdated = null;
            OnRewardGranted = null;
            OnQuestChainProgressed = null;
        }
    }
}
