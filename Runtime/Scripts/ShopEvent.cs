using System;
using UnityEngine;

namespace HBUnityGameCore
{

    public enum ShopState
    {
        Opened,
        Closed
    }

    public class ShopEvent : IEvent
    {
        public readonly ShopState shopState;

        public bool IsOpen()
        {
            return shopState == ShopState.Opened;
        }

        public ShopEvent(ShopState state)
        {
            shopState = state;
        }
    }
}
