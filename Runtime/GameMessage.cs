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

    public class GameMessage : IMessage
    {
        private GameEventType _gameEventType;

        public GameMessage(GameEventType gameEventType)
        {
            _gameEventType = gameEventType;
        }
    }
}
