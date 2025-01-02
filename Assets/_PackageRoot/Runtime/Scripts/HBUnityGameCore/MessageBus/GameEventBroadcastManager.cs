using UnityEngine;

namespace HBUnityGameCore
{
    public class Bus
    {
        public void emit(IMessage message)
        {
        }
    }
    
    public class GameEventBroadcastManager : MonoBehaviour
    {
        public Bus GameEventBus = new();
        public Bus AnalyticsEventBus = new();
        public Bus RewardEventBus = new();
        public Bus WorldEventBus = new();
        public Bus PlayerEventBus = new();
        public Bus AchievementEventBus = new();
        public Bus SocialEventBus = new();
        public Bus NotificationEventBus = new();

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

        public void GiveReward(GameRewardType rewardType, uint amount)
        {
            Debug.Log("Reward awarded!");
            _instance.RewardEventBus.emit(new RewardMessage(rewardType, amount));
        }

        public void GameBegin()
        {
            Debug.Log("Game began!");
            GameEventBus.emit(new GameMessage(GameEventType.Begin));
            AnalyticsEventBus.emit(new GameMessage(GameEventType.Begin));
        }

        public void GameEnd()
        {
            Debug.Log("Game ended!");
            GameEventBus.emit(new GameMessage(GameEventType.End));
            AnalyticsEventBus.emit(new GameMessage(GameEventType.End));
        }

        public void GamePause()
        {
            Debug.Log("Game paused!");
            GameEventBus.emit(new GameMessage(GameEventType.Pause));
            AnalyticsEventBus.emit(new GameMessage(GameEventType.Pause));
        }

        public void GameResume()
        {
            Debug.Log("Game resumed!");
            GameEventBus.emit(new GameMessage(GameEventType.Resume));
            AnalyticsEventBus.emit(new GameMessage(GameEventType.Resume));
        }
    }

}
