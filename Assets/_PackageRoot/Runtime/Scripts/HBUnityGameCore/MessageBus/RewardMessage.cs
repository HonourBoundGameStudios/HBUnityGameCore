using System;
using UnityEngine;

namespace HBUnityGameCore
{
    public enum GameRewardType
    {
        Gold
    }

    public class RewardMessage : IMessage
    {
        private GameRewardType _gameRewardType;
        private uint _amount;
        private readonly float _sinceStartupTimeStamp;
        private readonly DateTime _utcTimeStamp;

        public RewardMessage(GameRewardType gameRewardType, uint amount)
        {
            _gameRewardType = gameRewardType;
            _amount = amount;
            _sinceStartupTimeStamp = Time.realtimeSinceStartup;
            _utcTimeStamp = DateTime.UtcNow;
        }
    }
}
