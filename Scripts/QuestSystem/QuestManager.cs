using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuestSystem
{
    // =============================================================================
    // ENUMS AND DATA STRUCTURES
    // =============================================================================
    
    public enum QuestStatus
    {
        NotStarted,
        Active,
        Completed,
        Failed,
        Abandoned
    }
    
    public enum QuestType
    {
        MainStory,
        SideQuest,
        Daily,
        Weekly,
        Repeatable,
        Hidden,
        Tutorial
    }

    // =============================================================================
    // SAVE/LOAD DATA STRUCTURES
    // =============================================================================

    [System.Serializable]
    public class QuestSaveData
    {
        public List<string> activeQuestIds = new List<string>();
        public List<string> completedQuestIds = new List<string>();
        public List<QuestState> questStates = new List<QuestState>();
    }

    [System.Serializable]
    public class QuestState
    {
        public string questId;
        public QuestStatus status;
        public DateTime startedAt;
        public DateTime? completedAt;
        public List<ObjectiveState> objectiveStates = new List<ObjectiveState>();
    }

    [System.Serializable]
    public class ObjectiveState
    {
        public ObjectiveKey objectiveKey;
        public int currentProgress;
        public bool isCompleted;
    }

    // =============================================================================
    // QUEST MANAGER (Singleton)
    // =============================================================================
    
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }
        
        [SerializeField] private List<Quest> allQuests = new List<Quest>();
        [SerializeField] private List<Quest> activeQuests = new List<Quest>();
        [SerializeField] private List<Quest> completedQuests = new List<Quest>();
        [SerializeField] private List<Quest> availableQuests = new List<Quest>();
        
        // Quest tracking
        private Dictionary<string, Quest> questDatabase = new Dictionary<string, Quest>();
        private Dictionary<string, object> playerContext = new Dictionary<string, object>();
        
        // Events
        public event Action<Quest> OnActiveQuestAdded;
        public event Action<Quest> OnActiveQuestRemoved;
        public event Action<Quest> OnQuestProgressChanged;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeQuestSystem();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void InitializeQuestSystem()
        {
            // Subscribe to quest events
            QuestEvents.OnQuestStarted += HandleQuestStarted;
            QuestEvents.OnQuestCompleted += HandleQuestCompleted;
            QuestEvents.OnQuestFailed += HandleQuestFailed;
            QuestEvents.OnQuestAbandoned += HandleQuestAbandoned;
            QuestEvents.OnObjectiveCompleted += HandleObjectiveCompleted;
            
            // Initialize player context
            UpdatePlayerContext();
            
            Debug.Log("Quest System initialized");
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            QuestEvents.OnQuestStarted -= HandleQuestStarted;
            QuestEvents.OnQuestCompleted -= HandleQuestCompleted;
            QuestEvents.OnQuestFailed -= HandleQuestFailed;
            QuestEvents.OnQuestAbandoned -= HandleQuestAbandoned;
            QuestEvents.OnObjectiveCompleted -= HandleObjectiveCompleted;
        }

        public void LoadQuests()
        {
            // This would typically load from ScriptableObjects, JSON files, or a database
            // For now, we'll create some example quests
            // CreateExampleQuests();
            
            // Build quest database
            foreach (var quest in allQuests)
            {
                questDatabase[quest.id] = quest;
                
                if (quest.status == QuestStatus.NotStarted && quest.CanStart(playerContext))
                {
                    availableQuests.Add(quest);
                }
            }
        }
        
        public void UpdatePlayerContext()
        {
            // This would be called when player stats change
            playerContext["playerLevel"] = 1; // Get from player stats
            playerContext["playerPosition"] = Vector3.zero; // Get from player transform
            // Add other relevant player data...
        }
        
        public Quest GetQuestById(string questId)
        {
            return questDatabase.GetValueOrDefault(questId);
        }
        
        public List<Quest> GetAvailableQuests()
        {
            // Refresh available quests based on current player state
            availableQuests.Clear();
            
            foreach (var quest in allQuests)
            {
                if (quest.status == QuestStatus.NotStarted && quest.CanStart(playerContext))
                {
                    availableQuests.Add(quest);
                }
            }
            
            return new List<Quest>(availableQuests);
        }
        
        public List<Quest> GetActiveQuests()
        {
            return new List<Quest>(activeQuests);
        }
        
        public List<Quest> GetCompletedQuests()
        {
            return new List<Quest>(completedQuests);
        }
        
        public bool TryStartQuest(string questId)
        {
            var quest = GetQuestById(questId);
            if (quest == null || !quest.CanStart(playerContext))
                return false;
            
            quest.StartQuest();
            return true;
        }
        
        public bool IsQuestCompleted(string questId)
        {
            var quest = GetQuestById(questId);
            return quest != null && quest.status == QuestStatus.Completed;
        }
        
        public void UpdateQuestProgress(string eventType, Dictionary<string, object> eventData)
        {
            // Update player context if needed
            if (eventData.ContainsKey("playerPosition"))
            {
                playerContext["playerPosition"] = eventData["playerPosition"];
            }
            
            // Update all active quests with the event data
            foreach (var quest in activeQuests.ToList()) // ToList to avoid modification during iteration
            {
                quest.UpdateProgress(eventData);
            }
        }
        
        // Event handlers
        private void HandleQuestStarted(Quest quest)
        {
            if (!activeQuests.Contains(quest))
            {
                activeQuests.Add(quest);
                availableQuests.Remove(quest);
                OnActiveQuestAdded?.Invoke(quest);
                
                // Auto-start follow-up quests if they have auto-accept
                foreach (string followUpId in quest.followUpQuests)
                {
                    var followUp = GetQuestById(followUpId);
                    if (followUp != null && followUp.autoAccept && followUp.CanStart(playerContext))
                    {
                        followUp.StartQuest();
                    }
                }
            }
        }
        
        private void HandleQuestCompleted(Quest quest)
        {
            activeQuests.Remove(quest);
            if (!completedQuests.Contains(quest))
            {
                completedQuests.Add(quest);
            }
            OnActiveQuestRemoved?.Invoke(quest);
            
            // Start follow-up quests
            foreach (string followUpId in quest.followUpQuests)
            {
                TryStartQuest(followUpId);
            }
            
            // Handle repeatable quests
            if (quest.isRepeatable)
            {
                CreateRepeatableQuestInstance(quest);
            }
        }
        
        private void HandleQuestFailed(Quest quest)
        {
            activeQuests.Remove(quest);
            OnActiveQuestRemoved?.Invoke(quest);
            
            // Optionally add back to available quests if it can be retried
            if (quest.CanStart(playerContext))
            {
                availableQuests.Add(quest);
            }
        }
        
        private void HandleQuestAbandoned(Quest quest)
        {
            activeQuests.Remove(quest);
            OnActiveQuestRemoved?.Invoke(quest);
            
            // Reset quest state
            quest.status = QuestStatus.NotStarted;
            foreach (var objective in quest.objectives)
            {
                objective.currentProgress = 0;
                objective.isCompleted = false;
            }
            
            // Add back to available quests
            if (quest.CanStart(playerContext))
            {
                availableQuests.Add(quest);
            }
        }
        
        private void HandleObjectiveCompleted(Quest quest, QuestObjective objective)
        {
            OnQuestProgressChanged?.Invoke(quest);
        }
        
        private void CreateRepeatableQuestInstance(Quest originalQuest)
        {
            // Create a new instance of the repeatable quest
            var newQuest = new Quest(originalQuest.title, originalQuest.description, originalQuest.type)
            {
                requiredLevel = originalQuest.requiredLevel,
                autoAccept = originalQuest.autoAccept,
                autoComplete = originalQuest.autoComplete,
                isRepeatable = originalQuest.isRepeatable
            };
            
            // Copy objectives (create new instances)
            foreach (var originalObjective in originalQuest.objectives)
            {
                QuestObjective newObjective = null;
                
                switch (originalObjective.type)
                {
                    case ObjectiveType.Collection:
                        var collectionObj = originalObjective as CollectionObjective;
                        newObjective = new CollectionObjective(originalObjective.key, collectionObj.itemId, collectionObj.targetAmount, collectionObj.title, collectionObj.description);
                        break;
                    case ObjectiveType.Kill:
                        var killObj = originalObjective as KillObjective;
                        newObjective = new KillObjective(originalObjective.key, killObj.enemyType, killObj.targetAmount, killObj.areaId, killObj.title, killObj.description);
                        break;
                    // Add other objective types...
                }
                
                if (newObjective != null)
                {
                    newObjective.isRequired = originalObjective.isRequired;
                    newObjective.isVisible = originalObjective.isVisible;
                    newQuest.objectives.Add(newObjective);
                }
            }
            
            // Copy rewards
            foreach (var originalReward in originalQuest.rewards)
            {
                var newReward = new QuestReward(originalReward.type, originalReward.itemId, originalReward.amount, originalReward.description);
                newQuest.rewards.Add(newReward);
            }
            
            // Set new expiration if it was a daily/weekly quest
            if (originalQuest.type == QuestType.Daily)
            {
                newQuest.expirationDate = DateTime.Now.AddDays(1);
            }
            else if (originalQuest.type == QuestType.Weekly)
            {
                newQuest.expirationDate = DateTime.Now.AddDays(7);
            }
            
            // Add to system
            allQuests.Add(newQuest);
            questDatabase[newQuest.id] = newQuest;
            
            if (newQuest.CanStart(playerContext))
            {
                availableQuests.Add(newQuest);
            }
        }
        
        // Utility methods for game integration
        public void OnItemCollected(string itemId, int amount)
        {
            var eventData = new Dictionary<string, object>
            {
                ["itemId"] = itemId,
                ["amount"] = amount
            };
            UpdateQuestProgress("item_collected", eventData);
        }
        
        public void OnEnemyKilled(string enemyType, string areaId = "")
        {
            var eventData = new Dictionary<string, object>
            {
                ["enemyType"] = enemyType,
                ["areaId"] = areaId
            };
            UpdateQuestProgress("enemy_killed", eventData);
        }
        
        public void OnObjectInteracted(string objectId)
        {
            var eventData = new Dictionary<string, object>
            {
                ["targetId"] = objectId
            };
            UpdateQuestProgress("object_interacted", eventData);
        }
        
        public void OnLocationReached(Vector3 playerPosition)
        {
            var eventData = new Dictionary<string, object>
            {
                ["playerPosition"] = playerPosition
            };
            UpdateQuestProgress("location_reached", eventData);
        }
        
        // Save/Load functionality

        // TODO
        public QuestSaveData GetSaveData()
        {
            return new QuestSaveData
            {
                activeQuestIds = activeQuests.Select(q => q.id).ToList(),
                completedQuestIds = completedQuests.Select(q => q.id).ToList(),
                questStates = questDatabase.Values.Select(q => new QuestState
                {
                    questId = q.id,
                    status = q.status,
                    startedAt = q.startedAt,
                    completedAt = q.completedAt,
                    objectiveStates = q.objectives.Select(o => new ObjectiveState
                    {
                        objectiveKey = o.key,
                        currentProgress = o.currentProgress,
                        isCompleted = o.isCompleted
                    }).ToList()
                }).ToList()
            };
        }
        
        // TODO
        public void LoadSaveData(QuestSaveData saveData)
        {
            if (saveData == null) return;
            
            // Clear current state
            activeQuests.Clear();
            completedQuests.Clear();
            availableQuests.Clear();
            
            // Restore quest states
            foreach (var questState in saveData.questStates)
            {
                var quest = GetQuestById(questState.questId);
                if (quest != null)
                {
                    quest.status = questState.status;
                    quest.startedAt = questState.startedAt;
                    quest.completedAt = questState.completedAt;
                    
                    // Restore objective states
                    foreach (var objState in questState.objectiveStates)
                    {
                        var objective = quest.objectives.FirstOrDefault(o => o.key == objState.objectiveKey);
                        
                        if (objective != null)
                        {
                            objective.currentProgress = objState.currentProgress;
                            objective.isCompleted = objState.isCompleted;
                        }
                    }
                    
                    // Add to appropriate lists
                    switch (quest.status)
                    {
                        case QuestStatus.Active:
                            activeQuests.Add(quest);
                            break;
                        case QuestStatus.Completed:
                            completedQuests.Add(quest);
                            break;
                        case QuestStatus.NotStarted:
                            if (quest.CanStart(playerContext))
                            {
                                availableQuests.Add(quest);
                            }
                            break;
                    }
                }
            }
            
            Debug.Log($"Loaded quest save data: {activeQuests.Count} active, {completedQuests.Count} completed");
        }
        
        public void AddRange(Quest[] quests)
        {
            allQuests.AddRange(quests);
        }
        
        public bool IsQuestActive(string questIdValue)
        {
            return activeQuests.Any(q => q.id == questIdValue);
        }
    }
}