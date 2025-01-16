using System;
using UnityEngine;

namespace HBUnityGameCore
{
    public abstract class IEvent
    {
        private readonly float _sinceStartupTimeStamp = Time.realtimeSinceStartup;
        private readonly DateTime _utcTimeStamp = DateTime.UtcNow;

        public float SinceStartupTimeStamp()
        {
            return _sinceStartupTimeStamp;
        }

        public DateTime UtcTimeStamp()
        {
            return _utcTimeStamp;
        }
    }
}
