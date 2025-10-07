using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VRGame.QuestSystem
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
    
    public enum RewardType
    {
        Currency,
        Item,
        Experience,
        Unlock,
        Reputation,
        Cosmetic,
        Progressive
    }
    
    // =============================================================================
    // QUEST EVENTS
    // =============================================================================
    
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

    // =============================================================================
    // QUEST REWARD SYSTEM
    // =============================================================================
    
    [System.Serializable]
    public class QuestReward
    {
        public string id;
        public RewardType type;
        public string itemId;
        public int amount;
        public string description;
        public bool scaleWithLevel;
        public Dictionary<string, object> customData = new Dictionary<string, object>();
        
        public QuestReward(RewardType rewardType, string itemId, int amount, string description = "")
        {
            this.id = Guid.NewGuid().ToString();
            this.type = rewardType;
            this.itemId = itemId;
            this.amount = amount;
            this.description = description;
        }
        
        public void GrantReward()
        {
            // This would integrate with your game's systems
            switch (type)
            {
                case RewardType.Currency:
                    // PlayerInventory.AddCurrency(itemId, amount);
                    break;
                case RewardType.Item:
                    // PlayerInventory.AddItem(itemId, amount);
                    break;
                case RewardType.Experience:
                    // PlayerStats.AddExperience(amount);
                    break;
                // Add other reward types...
            }
            
            QuestEvents.RewardGranted(this);
            Debug.Log($"Granted reward: {type} - {itemId} x{amount}");
        }
    }

    // =============================================================================
    // QUEST OBJECTIVE SYSTEM
    // =============================================================================
    
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

    // =============================================================================
    // MAIN QUEST CLASS
    // =============================================================================
    
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

    // =============================================================================
    // QUEST BUILDER (for easy quest creation)
    // =============================================================================
    
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
            
            // Load quests from data (ScriptableObjects, JSON, etc.)
            LoadQuests();
            
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
        
        private void LoadQuests()
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
        
        private void CreateExampleQuests()
        {
            // Example: Tutorial Quest Chain
            var tutorialQuest1 = new QuestBuilder("Welcome to VR", "Learn the basics of VR interaction", QuestType.Tutorial)
                .SetAutoAccept(true)
                .SetAutoComplete(true)
                .AddInteractionObjective("tutorial_orb", "Pick up the glowing orb", "Use your controller to grab the orb")
                .AddLocationObjective("tutorial_zone", Vector3.zero, 3f, "Move to the marked area", "Walk to the glowing circle")
                .AddExperienceReward(100, "Tutorial completion bonus")
                .AddFollowUpQuest("combat_tutorial")
                .Build();
            
            var tutorialQuest2 = new QuestBuilder("Combat Training", "Learn to fight enemies", QuestType.Tutorial)
                .SetAutoAccept(true)
                .AddRequiredQuest(tutorialQuest1.id)
                .AddKillObjective("training_dummy", 3, "training_room", "Destroy training dummies", "Use your weapon to destroy 3 training dummies")
                .AddItemReward("basic_sword", 1, "Your first weapon")
                .AddExperienceReward(150)
                .Build();
            
            // Example: Collection Quest
            var gatheringQuest = new QuestBuilder("Resource Gathering", "Collect materials for crafting", QuestType.SideQuest)
                .AddCollectionObjective("wood", 10, "Gather Wood", "Collect wood from trees")
                .AddCollectionObjective("stone", 5, "Gather Stone", "Mine stone from rocks")
                .AddCurrencyReward("gold", 100)
                .AddItemReward("crafting_recipe_basic", 1, "Basic Crafting Recipe")
                .Build();
            
            // Example: Kill Quest with Area Restriction
            var huntingQuest = new QuestBuilder("Wolf Hunt", "Clear wolves from the forest", QuestType.SideQuest)
                .SetRequiredLevel(5)
                .AddKillObjective("wolf", 8, "dark_forest", "Hunt Forest Wolves", "Eliminate 8 wolves in the Dark Forest")
                .AddCurrencyReward("gold", 250)
                .AddItemReward("wolf_pelt", 3, "Wolf Pelts")
                .AddExperienceReward(300)
                .Build();
            
            // Example: Daily Quest
            var dailyQuest = new QuestBuilder("Daily Training", "Complete daily combat training", QuestType.Daily)
                .SetRepeatable(true)
                .SetExpiration(DateTime.Now.AddDays(1))
                .AddKillObjective("any", 15, "", "Defeat any enemies", "Kill 15 enemies of any type")
                .AddCurrencyReward("gold", 50)
                .AddExperienceReward(100)
                .Build();
            
            // Set up quest chain relationships
            tutorialQuest2.previousQuestId = tutorialQuest1.id;
            
            allQuests.AddRange(new[] { tutorialQuest1, tutorialQuest2, gatheringQuest, huntingQuest, dailyQuest });
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
            return questDatabase.ContainsKey(questId) ? questDatabase[questId] : null;
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
                        newObjective = new CollectionObjective(collectionObj.itemId, collectionObj.targetAmount, collectionObj.title, collectionObj.description);
                        break;
                    case ObjectiveType.Kill:
                        var killObj = originalObjective as KillObjective;
                        newObjective = new KillObjective(killObj.enemyType, killObj.targetAmount, killObj.areaId, killObj.title, killObj.description);
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
                        objectiveId = o.id,
                        currentProgress = o.currentProgress,
                        isCompleted = o.isCompleted
                    }).ToList()
                }).ToList()
            };
        }
        
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
                        var objective = quest.objectives.FirstOrDefault(o => o.id == objState.objectiveId);
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
        public string objectiveId;
        public int currentProgress;
        public bool isCompleted;
    }

    // =============================================================================
    // QUEST UI HELPER
    // =============================================================================
    
    public class QuestUIHelper : MonoBehaviour
    {
        [Header("UI References")]
        public Transform questLogParent;
        public GameObject questEntryPrefab;
        public GameObject objectiveEntryPrefab;
        
        private Dictionary<string, GameObject> questUIElements = new Dictionary<string, GameObject>();
        
        private void Start()
        {
            // Subscribe to quest events for UI updates
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.OnActiveQuestAdded += OnQuestAdded;
                QuestManager.Instance.OnActiveQuestRemoved += OnQuestRemoved;
                QuestManager.Instance.OnQuestProgressChanged += OnQuestProgressChanged;
            }
            
            RefreshQuestUI();
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.OnActiveQuestAdded -= OnQuestAdded;
                QuestManager.Instance.OnActiveQuestRemoved -= OnQuestRemoved;
                QuestManager.Instance.OnQuestProgressChanged -= OnQuestProgressChanged;
            }
        }
        
        private void OnQuestAdded(Quest quest)
        {
            CreateQuestUIElement(quest);
        }
        
        private void OnQuestRemoved(Quest quest)
        {
            if (questUIElements.ContainsKey(quest.id))
            {
                Destroy(questUIElements[quest.id]);
                questUIElements.Remove(quest.id);
            }
        }
        
        private void OnQuestProgressChanged(Quest quest)
        {
            UpdateQuestUIElement(quest);
        }
        
        private void CreateQuestUIElement(Quest quest)
        {
            if (questEntryPrefab == null || questLogParent == null) return;
            
            var questUI = Instantiate(questEntryPrefab, questLogParent);
            questUIElements[quest.id] = questUI;
            
            // Set quest title and description (assuming your prefab has these components)
            var titleText = questUI.GetComponentInChildren<UnityEngine.UI.Text>();
            if (titleText != null)
            {
                titleText.text = quest.title;
            }
            
            UpdateQuestUIElement(quest);
        }
        
        private void UpdateQuestUIElement(Quest quest)
        {
            if (!questUIElements.ContainsKey(quest.id)) return;
            
            var questUI = questUIElements[quest.id];
            
            // Update progress bar, objective list, etc.
            // This would depend on your specific UI setup
            
            Debug.Log($"Updated UI for quest: {quest.title} - Progress: {quest.GetOverallProgress():P0}");
        }
        
        public void RefreshQuestUI()
        {
            // Clear existing UI
            foreach (var ui in questUIElements.Values)
            {
                if (ui != null) Destroy(ui);
            }
            questUIElements.Clear();
            
            // Create UI for all active quests
            if (QuestManager.Instance != null)
            {
                foreach (var quest in QuestManager.Instance.GetActiveQuests())
                {
                    CreateQuestUIElement(quest);
                }
            }
        }
        
        // Methods for quest interaction in VR
        public void ShowQuestDetails(string questId)
        {
            var quest = QuestManager.Instance.GetQuestById(questId);
            if (quest != null)
            {
                // Show detailed quest information in VR UI
                Debug.Log($"Showing details for: {quest.title}");
                Debug.Log($"Description: {quest.description}");
                Debug.Log($"Progress: {quest.GetOverallProgress():P0}");
                
                foreach (var objective in quest.GetActiveObjectives())
                {
                    Debug.Log($"- {objective.title}: {objective.currentProgress}/{objective.targetAmount}");
                }
            }
        }
        
        public void AbandonQuest(string questId)
        {
            var quest = QuestManager.Instance.GetQuestById(questId);
            quest?.AbandonQuest();
        }
    }

    // =============================================================================
    // QUEST INTEGRATION COMPONENTS
    // =============================================================================
    
    // Component for objects that can be interacted with for quests
    public class QuestInteractable : MonoBehaviour
    {
        [Header("Quest Settings")]
        public string interactableId;
        public bool oneTimeUse = false;
        public string requiredQuestId = "";
        
        private bool hasBeenUsed = false;
        
        public void OnInteract()
        {
            if (oneTimeUse && hasBeenUsed) return;
            
            // Check if required quest is active
            if (!string.IsNullOrEmpty(requiredQuestId))
            {
                var quest = QuestManager.Instance.GetQuestById(requiredQuestId);
                if (quest == null || quest.status != QuestStatus.Active) return;
            }
            
            // Notify quest system
            QuestManager.Instance.OnObjectInteracted(interactableId);
            
            if (oneTimeUse)
            {
                hasBeenUsed = true;
            }
            
            Debug.Log($"Interacted with: {interactableId}");
        }
    }
    
    // Component for collectible items
    public class QuestCollectible : MonoBehaviour
    {
        [Header("Collectible Settings")]
        public string itemId;
        public int amount = 1;
        public bool autoCollectOnTrigger = true;
        
        private void OnTriggerEnter(Collider other)
        {
            if (autoCollectOnTrigger && other.CompareTag("Player"))
            {
                CollectItem();
            }
        }
        
        public void CollectItem()
        {
            // Notify quest system
            QuestManager.Instance.OnItemCollected(itemId, amount);
            
            // Add to player inventory (integrate with your inventory system)
            // PlayerInventory.AddItem(itemId, amount);
            
            Debug.Log($"Collected: {itemId} x{amount}");
            
            // Destroy or disable the collectible
            Destroy(gameObject);
        }
    }
    
    // Component for enemies that count toward kill objectives
    public class QuestEnemy : MonoBehaviour
    {
        [Header("Enemy Settings")]
        public string enemyType;
        public string areaId = "";
        
        public void OnDeath()
        {
            // Get current area if not specified
            string currentArea = string.IsNullOrEmpty(areaId) ? GetCurrentArea() : areaId;
            
            // Notify quest system
            QuestManager.Instance.OnEnemyKilled(enemyType, currentArea);
            
            Debug.Log($"Enemy killed: {enemyType} in {currentArea}");
        }
        
        private string GetCurrentArea()
        {
            // This would integrate with your area/zone system
            // For example, check what area trigger the enemy is in
            var areaCollider = GetComponentInParent<AreaTrigger>();
            return areaCollider?.areaId ?? "unknown";
        }
    }
    
    // Component for area detection
    public class AreaTrigger : MonoBehaviour
    {
        [Header("Area Settings")]
        public string areaId;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Update player position for location objectives
                QuestManager.Instance.OnLocationReached(other.transform.position);
                
                Debug.Log($"Player entered area: {areaId}");
            }
        }
    }

    // =============================================================================
    // EXAMPLE USAGE AND QUEST TEMPLATES
    // =============================================================================
    
    public class QuestExamples
    {
        // Example of creating complex quests
        public static Quest CreateTutorialChain()
        {
            return new QuestBuilder("VR Tutorial", "Learn the basics of VR interaction", QuestType.Tutorial)
                .SetAutoAccept(true)
                .SetAutoComplete(false) // Player needs to manually complete
                .AddInteractionObjective("tutorial_button", "Press the Tutorial Button", "Find and press the glowing button")
                .AddCollectionObjective("tutorial_gem", 1, "Collect the Tutorial Gem", "Pick up the floating gem")
                .AddLocationObjective("finish_area", new Vector3(10, 0, 10), 2f, "Reach the Finish Area", "Walk to the marked circle")
                .AddExperienceReward(200, "Tutorial completion")
                .AddItemReward("starter_weapon", 1, "Beginner's weapon")
                .AddFollowUpQuest("combat_tutorial")
                .Build();
        }
        
        public static Quest CreateDynamicKillQuest(string enemyType, int amount, string area, int playerLevel)
        {
            // Scale rewards based on player level
            int goldReward = 50 * playerLevel;
            int expReward = 100 * playerLevel;
            
            return new QuestBuilder($"Hunt {enemyType.ToUpper()}", $"Eliminate {amount} {enemyType} creatures", QuestType.SideQuest)
                .SetRequiredLevel(Math.Max(1, playerLevel - 2))
                .AddKillObjective(enemyType, amount, area)
                .AddCurrencyReward("gold", goldReward)
                .AddExperienceReward(expReward)
                .Build();
        }
        
        public static Quest CreateMultiStageQuest()
        {
            return new QuestBuilder("The Ancient Artifact", "Recover the lost artifact", QuestType.MainStory)
                .AddInteractionObjective("sage_npc", "Speak to the Sage", "Talk to the village sage")
                .AddCollectionObjective("ancient_key", 1, "Find the Ancient Key", "Locate the key to the temple")
                .AddLocationObjective("ancient_temple", new Vector3(50, 0, 50), 5f, "Reach the Ancient Temple", "Travel to the temple location")
                .AddKillObjective("temple_guardian", 1, "ancient_temple", "Defeat the Temple Guardian", "Battle the guardian protecting the artifact")
                .AddCollectionObjective("ancient_artifact", 1, "Retrieve the Artifact", "Take the ancient artifact")
                .AddCurrencyReward("gold", 1000)
                .AddExperienceReward(500)
                .AddItemReward("artifact_power", 1, "Ancient Power")
                .Build();
        }
    }
}