using System;
using UnityEngine;

namespace HBUnityGameCore
{
    public class Receiver
    {
        public void OnEventReceived(IEvent @event)
        {
            Console.WriteLine("Event received: " + @event);
        }
    }

    public class Bus
    {
        public delegate void EventHandler(IEvent @event);

        // Declare the event using the delegate
        public event EventHandler HandleEvent;

        // Method to raise the event
        public void Emit(IEvent @event)
        {
            // Check if there are any subscribers
            if (HandleEvent != null)
            {
                HandleEvent(@event);
            }
        }

        // Method to subscribe to the event
        public void Subscribe(EventHandler eventHandler)
        {
            HandleEvent += eventHandler;
        }

        // Method to unsubscribe from the event
        public void Unsubscribe(EventHandler eventHandler)
        {
            HandleEvent -= eventHandler;
        }
    }

    public class GameEventBroadcastManager
    {
        private readonly SerializableDictionary<string, Bus> _buses = new();

        private static GameEventBroadcastManager _instance;
        public static GameEventBroadcastManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameEventBroadcastManager();
                }
                return _instance;
            }
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
