using System;

namespace HBUnityGameCore
{
    public class Receiver
    {
        public void OnEventReceived(IEvent @event)
        {
            Console.WriteLine("Event received: " + @event);
        }
    }
}
