namespace HBUnityGameCore
{
    // Usage example:
    internal class Test
    {
        private void TestSomething(string[] args)
        {
            // Create instances of publisher and subscriber
            Bus bus = new Bus();
            Receiver receiver = new Receiver();

            // Subscribe to the event
            bus.Subscribe(receiver.OnEventReceived);

            // Trigger the event
            bus.Emit(new RewardEvent(GameRewardType.Gold, 100, 1000));

            // Unsubscribe from the event
            bus.Unsubscribe(receiver.OnEventReceived);
        }
    }
}
