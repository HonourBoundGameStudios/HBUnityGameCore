using UnityEngine;

namespace VRGame.QuestSystem
{
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
}
