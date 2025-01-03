using System;
using UnityEngine;

namespace HBUnityGameCore
{
    public abstract class IMessage
    {
        private readonly float _sinceStartupTimeStamp = Time.realtimeSinceStartup;
        private readonly DateTime _utcTimeStamp = DateTime.UtcNow;
    }
}
