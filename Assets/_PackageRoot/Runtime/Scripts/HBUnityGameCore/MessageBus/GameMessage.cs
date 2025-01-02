using System;
using UnityEngine;

namespace HBUnityGameCore
{
    public class IMessage
    {
    }
    
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
            _utcTimeStamp = DateTime.UtcNow;
        }
    }
}
