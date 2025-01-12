using System;
using UnityEngine;

namespace HBUnityGameCore
{
    public class Receiver<IMessage>
    {
        public void OnMessageReceived(IMessage message)
        {
            Console.WriteLine("Message received: " + message);
        }
    }

    public class Bus
    {
        public delegate void MyMessageHandler(IMessage rewardMessage);

        // Declare the event using the delegate
        public event MyMessageHandler HandleMessage;

        // Method to raise the event
        public void Emit(IMessage message)
        {
            // Check if there are any subscribers
            if (HandleMessage != null)
            {
                HandleMessage(message);
            }
        }

        // Method to subscribe to the event
        public void Subscribe(MyMessageHandler messageHandler)
        {
            HandleMessage += messageHandler;
        }

        // Method to unsubscribe from the event
        public void Unsubscribe(MyMessageHandler messageHandler)
        {
            HandleMessage -= messageHandler;
        }
    }


    class Test
    {
        void TestSomething(string[] args)
        {
            // Create instances of publisher and subscriber
            Bus bus = new Bus();
            Receiver<IMessage> receiver = new Receiver<IMessage>();

            // Subscribe to the event
            bus.Subscribe(receiver.OnMessageReceived);

            // Trigger the event
            bus.Emit(new RewardMessage(GameRewardType.Gold, 100, 1000));

            // Unsubscribe from the event
            bus.Unsubscribe(receiver.OnMessageReceived);
        }
    }

    public class GameEventBroadcastManager : MonoBehaviour
    {
        public Bus GameEventBus = new();
        public Bus AnalyticsEventBus = new();
        public Bus RewardEventBus = new();
        // public Bus WorldEventBus = new();
        // public Bus PlayerEventBus = new();
        // public Bus AchievementEventBus = new();
        // public Bus SocialEventBus = new();
        // public Bus NotificationEventBus = new();

        private static GameEventBroadcastManager _instance;

        /// <summary>
        /// Singleton pattern implementation,
        /// If there is no instance of the class, it creates one.
        /// An object to hold the component is created as needed.
        /// </summary>
        public static GameEventBroadcastManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (GameEventBroadcastManager)FindFirstObjectByType(typeof(GameEventBroadcastManager));

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<GameEventBroadcastManager>();
                        singleton.name = "(singleton) " + typeof(GameEventBroadcastManager).ToString();

                        DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] An instance of " + typeof(GameEventBroadcastManager) +
                                  " is needed in the scene, so '" + singleton +
                                  "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance already created: " +
                                  _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }

        public void GiveReward(GameRewardType rewardType, uint amount, uint total)
        {
            Debug.Log("Reward awarded!");
            _instance.RewardEventBus.Emit(new RewardMessage(rewardType, amount, total));
        }

        public void GameBegin()
        {
            Debug.Log("Game began!");
            GameEventBus.Emit(new GameMessage(GameEventType.Begin));
            AnalyticsEventBus.Emit(new AnalyticMessage());
        }

        public void GameEnd()
        {
            Debug.Log("Game ended!");
            GameEventBus.Emit(new GameMessage(GameEventType.End));
            AnalyticsEventBus.Emit(new AnalyticMessage());
        }

        public void GamePause()
        {
            Debug.Log("Game paused!");
            GameEventBus.Emit(new GameMessage(GameEventType.Pause));
            AnalyticsEventBus.Emit(new AnalyticMessage());
        }

        public void GameResume()
        {
            Debug.Log("Game resumed!");
            GameEventBus.Emit(new GameMessage(GameEventType.Resume));
            AnalyticsEventBus.Emit(new AnalyticMessage());
        }
    }

}
