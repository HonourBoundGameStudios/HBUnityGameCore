using System;
using UnityEngine;

namespace HBUnityGameCore
{
    public abstract class IEvent
    {
        public readonly IEventSource Source;

        private readonly float _sinceStartupTimeStamp = Time.realtimeSinceStartup;
        private readonly DateTime _utcTimeStamp = DateTime.UtcNow;

        public IEvent()
        {
            Source = null; // Default constructor for events without a specific source
        }
        
        public IEvent(IEventSource source)
        {
            Source = source;
        }

        public float SinceStartupTimeStamp()
        {
            return _sinceStartupTimeStamp;
        }

        public DateTime UtcTimeStamp()
        {
            return _utcTimeStamp;
        }
        
        public override string ToString()
        {
            return $"Event: {GetType().Name}, Source: {Source?.GetType().Name ?? "None"}, Time: {UtcTimeStamp()}";
        }
    }
}
