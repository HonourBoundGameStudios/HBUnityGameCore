using System;
using UnityEngine;

namespace HBUnityGameCore
{
    public enum GameRewardType
    {
        Gold
    }

    public class RewardEvent : IEvent
    {
        public GameRewardType RewardType
        {
            get;
        }
        public uint Total
        {
            get;
        }
        public uint Amount
        {
            get;
        }
        public float SinceStartupTimeStamp
        {
            get;
        }
        public DateTime UtcTimeStamp
        {
            get;
        }

        public RewardEvent(GameRewardType gameRewardType, uint amount, uint total)
        {
            RewardType = gameRewardType;
            Amount = amount;
            Total = total;
            SinceStartupTimeStamp = Time.realtimeSinceStartup;
            UtcTimeStamp = DateTime.UtcNow;
        }
    }
}
