namespace HBUnityGameCore
{
    public enum EconomyEventType
    {
        Purchase,
        Sale,
        RequestResourceBalance,
        ResourceBalance,
        ResourceGained,
        ResourceSpent
    }

    public class EconomyEvent : IEvent
    {
        public EconomyEventType EventType
        {
            get;
        }

        public float Amount
        {
            get;
        }

        public Item TransactionItem
        {
            get;
        }

        public string ResourceName
        {
            get;
        }

        public EconomyEvent(EconomyEventType eventType, string resourceName)
        {
            EventType = eventType;
            ResourceName = resourceName;
        }

        public EconomyEvent(EconomyEventType eventType, string resourceName, float amount)
        {
            EventType = eventType;
            ResourceName = resourceName;
            Amount = amount;
        }

        public EconomyEvent(EconomyEventType eventType, float resourceBalance)
        {
            EventType = eventType;
            Amount = resourceBalance;
        }

        public EconomyEvent(EconomyEventType eventType, Item item, float amount)
        {
            EventType = eventType;
            TransactionItem = item;
            Amount = amount;
        }
    }
}
