using System;
using UnityEngine;

namespace HBUnityGameCore
{
    public enum ShopState
    {
        Opened,
        Closed
    }

    public class ShopMessage : IMessage
    {
        private ShopState shopState;

        public bool IsOpen()
        {
            return shopState == ShopState.Opened;
        }

        public float SinceStartupTimeStamp
        {
            get;
        }
        public DateTime UtcTimeStamp
        {
            get;
        }

        public ShopMessage(ShopState state)
        {
            shopState = state;
            SinceStartupTimeStamp = Time.realtimeSinceStartup;
            UtcTimeStamp = DateTime.UtcNow;
        }
    }
}
