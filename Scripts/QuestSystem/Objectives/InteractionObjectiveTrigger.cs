using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    // Attach this script to an interactable object to trigger interaction objectives
    public class InteractionObjectiveTrigger : MonoBehaviour
    {
        [SerializeField]
        public InteractionObjective interactionObjective;
        
        [SerializeField]
        string interactableId;
        
        private void Awake()
        {
            if (string.IsNullOrEmpty(interactableId))
            {
                Debug.LogError("InteractableId must be set on InteractionObjectiveTrigger.");
            }
        }

        public void OnItemInteracted(string interactableId)
        {
            var context = new Dictionary<string, object>
            {
                { "interactableId", interactableId }
            };

            if (interactionObjective != null)
            {
                interactionObjective.TryUpdateFromContext(context);
            }
        }
    }
}
