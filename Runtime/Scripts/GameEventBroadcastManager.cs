using System;
using UnityEngine;

namespace HBUnityGameCore
{
    public class Receiver
    {
        public void OnEventReceived(IEvent @event)
        {
            Console.WriteLine("Message received: " + @event);
        }
    }

    public class Bus
    {
        public delegate void EventHandler(IEvent rewardEvent);

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
        public void Subscribe(EventHandler messageHandler)
        {
            HandleEvent += messageHandler;
        }

        // Method to unsubscribe from the event
        public void Unsubscribe(EventHandler messageHandler)
        {
            HandleEvent -= messageHandler;
        }
    }

    public class GameEventBroadcastManager
    {
        public readonly Bus GameEventBus = new();
        public readonly Bus AnalyticsEventBus = new();
        public readonly Bus RewardEventBus = new();
        public readonly Bus EconomyEventBus = new();
        // public Bus WorldEventBus = new();
        // public Bus PlayerEventBus = new();
        // public Bus AchievementEventBus = new();
        // public Bus SocialEventBus = new();
        // public Bus NotificationEventBus = new();

        private static GameEventBroadcastManager _instance;
        public static GameEventBroadcastManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameEventBroadcastManager();

                    if (_instance == null)
                    {
                         Debug.LogError("GameEventBroadcastManager instance could not be created.");
                    }
                }

                return _instance;
            }
        }

        public void GiveReward(GameRewardType rewardType, uint amount, uint total)
        {
            Debug.Log("Reward awarded!");
            _instance.RewardEventBus.Emit(new RewardEvent(rewardType, amount, total));
        }

        public void GameBegin()
        {
            Debug.Log("Game began!");
            GameEventBus.Emit(new GameEvent(GameEventType.Begin));
            AnalyticsEventBus.Emit(new AnalyticEvent());
        }

        public void GameEnd()
        {
            Debug.Log("Game ended!");
            GameEventBus.Emit(new GameEvent(GameEventType.End));
            AnalyticsEventBus.Emit(new AnalyticEvent());
        }

        public void GamePause()
        {
            Debug.Log("Game paused!");
            GameEventBus.Emit(new GameEvent(GameEventType.Pause));
            AnalyticsEventBus.Emit(new AnalyticEvent());
        }

        public void GameResume()
        {
            Debug.Log("Game resumed!");
            GameEventBus.Emit(new GameEvent(GameEventType.Resume));
            AnalyticsEventBus.Emit(new AnalyticEvent());
        }
    }
}
