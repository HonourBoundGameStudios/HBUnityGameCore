using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuestSystem
{
    [System.Serializable]
    public class Quest
    {
        public string id;
        public string title;
        public string description;
        public QuestType type;
        public QuestStatus status;
        public int priority;
        
        // Prerequisites
        public List<string> requiredQuests = new List<string>();
        public int requiredLevel;
        public Dictionary<string, object> prerequisites = new Dictionary<string, object>();
        
        // Objectives
        public List<QuestObjective> objectives = new List<QuestObjective>();

        // Rewards
        public List<QuestReward> rewards = new List<QuestReward>();
        
        // Quest Chain
        public List<string> followUpQuests = new List<string>();
        public string previousQuestId;
        
        // Configuration
        public bool autoAccept;
        public bool autoComplete; // if true, quest completes automatically when objectives are done
        public DateTime? expirationDate;
        public bool isRepeatable;
        public Dictionary<string, object> customData = new Dictionary<string, object>();
        
        // Progress tracking
        public DateTime startedAt;
        public DateTime? completedAt;
        
        public Quest(string title, string description, QuestType questType = QuestType.SideQuest)
        {
            this.id = Guid.NewGuid().ToString();
            this.title = title;
            this.description = description;
            this.type = questType;
            this.status = QuestStatus.NotStarted;
            this.priority = 0;
            this.requiredLevel = 1;
        }
        
        public bool CanStart(Dictionary<string, object> playerContext = null)
        {
            if (status != QuestStatus.NotStarted) return false;
            if (IsExpired()) return false;
            
            // Check level requirement
            if (playerContext != null && playerContext.ContainsKey("playerLevel"))
            {
                int playerLevel = (int)playerContext["playerLevel"];
                if (playerLevel < requiredLevel) return false;
            }
            
            // Check required quests (would integrate with QuestManager)
            foreach (string requiredQuestId in requiredQuests)
            {
                // QuestManager would check if required quest is completed
                // if (!QuestManager.Instance.IsQuestCompleted(requiredQuestId)) return false;
            }
            
            return true;
        }
        
        public void StartQuest()
        {
            if (!CanStart()) return;
            
            status = QuestStatus.Active;
            startedAt = DateTime.Now;
            
            QuestEvents.QuestStarted(this);
            Debug.Log($"Quest started: {title}");
        }
        
        public void UpdateProgress(Dictionary<string, object> context)
        {
            if (status != QuestStatus.Active) return;
            
            if (IsExpired())
            {
                FailQuest();
                return;
            }
            
            bool anyObjectiveUpdated = false;
            
            foreach (var objective in objectives)
            {
                if (!objective.isCompleted && objective.TryUpdateFromContext(context))
                {
                    QuestEvents.ObjectiveCompleted(this, objective);
                    anyObjectiveUpdated = true;
                }
            }
            
            if (anyObjectiveUpdated)
            {
                QuestEvents.ObjectiveProgressUpdated(this, null);
                CheckForCompletion();
            }
        }
        
        private void CheckForCompletion()
        {
            if (status != QuestStatus.Active) return;
            
            bool canComplete = objectives.Where(o => o.isRequired).All(o => o.isCompleted);
            
            if (canComplete)
            {
                if (autoComplete)
                {
                    CompleteQuest();
                }
            }
        }
        
        private bool CheckSequentialCompletion()
        {
            var requiredObjectives = objectives.Where(o => o.isRequired).ToList();
            
            for (int i = 0; i < requiredObjectives.Count; i++)
            {
                if (!requiredObjectives[i].isCompleted)
                {
                    // In sequential, all previous objectives must be completed
                    return i == 0 ? false : requiredObjectives.Take(i).All(o => o.isCompleted);
                }
            }
            
            return true; // All completed
        }
        
        public void CompleteQuest()
        {
            if (status != QuestStatus.Active) return;
            
            status = QuestStatus.Completed;
            completedAt = DateTime.Now;
            
            // Grant quest rewards
            foreach (var reward in rewards)
            {
                reward.GrantReward();
            }
            
            QuestEvents.QuestCompleted(this);
            
            // Trigger follow-up quests
            foreach (string followUpQuestId in followUpQuests)
            {
                // QuestManager would handle starting follow-up quests
                // QuestManager.Instance.TryStartQuest(followUpQuestId);
            }
            
            Debug.Log($"Quest completed: {title}");
        }
        
        public void FailQuest()
        {
            if (status != QuestStatus.Active) return;
            
            status = QuestStatus.Failed;
            QuestEvents.QuestFailed(this);
            Debug.Log($"Quest failed: {title}");
        }
        
        public void AbandonQuest()
        {
            if (status != QuestStatus.Active) return;
            
            status = QuestStatus.Abandoned;
            QuestEvents.QuestAbandoned(this);
            Debug.Log($"Quest abandoned: {title}");
        }
        
        public bool IsExpired()
        {
            return expirationDate.HasValue && DateTime.Now > expirationDate.Value;
        }
        
        public float GetOverallProgress()
        {
            if (objectives.Count == 0) return 0f;
            
            var requiredObjectives = objectives.Where(o => o.isRequired).ToList();
            if (requiredObjectives.Count == 0) return 1f;
            
            float totalProgress = requiredObjectives.Sum(o => o.GetProgressPercentage());
            return totalProgress / requiredObjectives.Count;
        }
        
        public List<QuestObjective> GetActiveObjectives()
        {
            var activeObjectives = new List<QuestObjective>();
            
            activeObjectives.AddRange(objectives.Where(o => !o.isCompleted));
            
            return activeObjectives.Where(o => o.isVisible).ToList();
        }
    }
}
