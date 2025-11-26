using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    // Attach this script to a delivered object to trigger delivery objectives
    public class DeliveryObjectiveTrigger : MonoBehaviour
    {
        [SerializeField]
        public DeliveryObjective deliveryObjective;
        
        [SerializeField]
        string itemId;
        
        [SerializeField]
        string destinationId;
        
        private void Awake()
        {
            if (string.IsNullOrEmpty(itemId))
            {
                Debug.LogError("ItemId must be set on DeliveryObjectiveTrigger.");
            }
            
            if (string.IsNullOrEmpty(destinationId))
            {
                Debug.LogError("DestinationId must be set on DeliveryObjectiveTrigger.");
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            OnItemDelivered(itemId, destinationId);
        }

        public void OnItemDelivered(string itemId, string destinationId)
        {
            var context = new Dictionary<string, object>
            {
                { "itemId", itemId },
                { "destinationId", destinationId }
            };

            if (deliveryObjective != null)
            {
                deliveryObjective.TryUpdateFromContext(context);
            }
        }
    }
}
