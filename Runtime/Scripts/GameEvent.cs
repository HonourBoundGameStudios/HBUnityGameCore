namespace HBUnityGameCore
{
    public enum GameEventType
    {
        Begin,
        End,
        Pause,
        Resume,
        MainMenuBegin,
        MainMenuEnd,
        LevelBegin,
        LevelEnd,
        LevelWon,
        LevelLost
    }

    public class GameEvent : IEvent
    {
        public readonly GameEventType GameEventType;

        public GameEvent(GameEventType gameEventType)
        {
            GameEventType = gameEventType;
        }
    }
}
