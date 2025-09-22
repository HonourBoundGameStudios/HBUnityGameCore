namespace HBUnityGameCore
{
    public class Bus
    {
        public delegate void EventHandler(IEvent @event);

        // Declare the event using the delegate
        public event EventHandler HandleEvent;

        // Method to raise the event
        public void Emit(IEvent @event)
        {
            // Check if there are any subscribers
            if (HandleEvent != null)
            {
                HandleEvent(@event);
            }
        }

        // Method to subscribe to the event
        public void Subscribe(EventHandler eventHandler)
        {
            HandleEvent += eventHandler;
        }

        // Method to unsubscribe from the event
        public void Unsubscribe(EventHandler eventHandler)
        {
            HandleEvent -= eventHandler;
        }
    }
}
