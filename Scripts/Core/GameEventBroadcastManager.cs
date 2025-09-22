using UnityEngine;

namespace HBUnityGameCore
{

    public class GameEventBroadcastManager : MonoBehaviour
    {
        private readonly SerializableDictionary<string, Bus> _buses = new();

        public static GameEventBroadcastManager Instance { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject); // Prevent duplicates
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persist across scenes
        }
        
        // --- The New API: Get any bus by its string name ---
        /// <summary>
        /// Gets the event bus for a specific channel, identified by a string key.
        /// If the bus does not exist, it will be created automatically.
        /// </summary>
        /// <param name="busName">The case-sensitive name of the bus to retrieve (e.g., "Game", "Encounter").</param>
        /// <returns>The requested Bus instance.</returns>
        public Bus GetBus(string busName)
        {
            // Defensively check for null or empty strings
            if (string.IsNullOrEmpty(busName))
            {
                Debug.LogError("Bus name cannot be null or empty.");
                return null; // Return null to prevent further errors
            }

            // Check if the bus already exists in our dictionary
            if (!_buses.ContainsKey(busName))
            {
                // If not, create a new one and add it.
                _buses[busName] = new Bus();
            }
            
            return _buses[busName];
        }
    }
}
