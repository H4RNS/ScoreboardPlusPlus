using BepInEx.Configuration;

namespace ScoreboardPlusPlus.Tools
{
    public static class Configuration
    {
        public static ConfigEntry<ActionType> ActionButton;
        public static ConfigEntry<string> RoomCode;

        public enum ActionType
        {
            Disconnect,
            JoinRandom,
            JoinSpecific,
        }

        public static void BuildFile(ConfigFile config)
        {
            ActionButton = config.Bind(
                "General",
                "Action Button",
                ActionType.Disconnect,
                "Description"
            );

            RoomCode = config.Bind(
                "General",
                "Room Code",
                "SBPP",
                "Description"
            );
        }
    }
}