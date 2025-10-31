using UnityEngine;

namespace VRGame.QuestSystem
{
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
}
