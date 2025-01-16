using System;
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

    public class GameEvent : IEvent
    {
        private GameEventType _gameEventType;

        public GameEvent(GameEventType gameEventType)
        {
            _gameEventType = gameEventType;
        }
    }
}
