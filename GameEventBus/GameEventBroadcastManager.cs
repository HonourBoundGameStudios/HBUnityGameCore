using System;
using Messaging;
using Messaging.Message;
using UnityEngine;

namespace HBUnityGameCore
{
    public enum GameEventType
    {
        Begin,
        End,
        Pause,
        Resume
    }

    public class GameMessage : IMessage
    {
        private GameEventType _gameEventType;
        private readonly float _sinceStartupTimeStamp;
        private readonly DateTime _utcTimeStamp;

        public GameMessage(GameEventType gameEventType)
        {
            _gameEventType = gameEventType;
            _sinceStartupTimeStamp = Time.realtimeSinceStartup;
            _utcTimeStamp = System.DateTime.UtcNow;
        }
    }

    public class GameEventBroadcastManager : MonoBehaviour
    {
        public static readonly Bus GameEventBus = new();
        public static readonly Bus AnalyticsEventBus = new();
        public static readonly Bus WorldEventBus = new();
        public static readonly Bus PlayerEventBus = new();
        public static readonly Bus EnemyEventBus = new();
        public static readonly Bus ItemEventBus = new();
        public static readonly Bus UIEventBus = new();
        public static readonly Bus AchievementEventBus = new();
        public static readonly Bus LeaderboardEventBus = new();
        public static readonly Bus SocialEventBus = new();
        public static readonly Bus AudioEventBus = new();
        public static readonly Bus VideoEventBus = new();
        public static readonly Bus NotificationEventBus = new();
        public static readonly Bus ProfileEventBus = new();
        public static readonly Bus TimeEventBus = new();
        public static readonly Bus WeatherEventBus = new();
        public static readonly Bus EnvironmentEventBus = new();
        public static readonly Bus EffectEventBus = new();

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
                    _instance = (GameEventBroadcastManager)FindObjectOfType(typeof(GameEventBroadcastManager));

                    GameEventBroadcastManager[] gameEventBroadcastManagers = FindObjectsByType<GameEventBroadcastManager>(FindObjectsSortMode.None);

                    if (gameEventBroadcastManagers.Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                                       " - there should never be more than 1 singleton!" +
                                       " Reopening the scene might fix it.");
                        return _instance;
                    }

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

        void GameBegin()
        {
            Debug.Log("Game began!");
            GameEventBus.emit(new GameMessage(GameEventType.Begin));
            AnalyticsEventBus.emit(new GameMessage(GameEventType.Begin));
        }

        void GameEnd()
        {
            Debug.Log("Game ended!");
            GameEventBus.emit(new GameMessage(GameEventType.End));
            AnalyticsEventBus.emit(new GameMessage(GameEventType.End));
        }

        void GamePause()
        {
            Debug.Log("Game paused!");
            GameEventBus.emit(new GameMessage(GameEventType.Pause));
            AnalyticsEventBus.emit(new GameMessage(GameEventType.Pause));
        }

        void GameResume()
        {
            Debug.Log("Game resumed!");
            GameEventBus.emit(new GameMessage(GameEventType.Resume));
            AnalyticsEventBus.emit(new GameMessage(GameEventType.Resume));
        }
    }
}
