using UnityEngine;

namespace VRGame.QuestSystem
{
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
}
