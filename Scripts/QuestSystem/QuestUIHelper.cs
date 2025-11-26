using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
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
}
