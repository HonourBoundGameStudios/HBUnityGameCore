namespace HBUnityGameCore
{
    public enum EconomyEventType
    {
        Purchase,
        Sale,
        Deposit,
        Withdrawal
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

        public EconomyEvent(EconomyEventType eventType, Item item, float amount)
        {
            EventType = eventType;
            Amount = amount;
            TransactionItem = item;
        }
    }
}
