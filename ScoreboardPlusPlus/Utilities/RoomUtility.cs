using GorillaNetworking;
using ScoreboardPlusPlus.Tools;

namespace ScoreboardPlusPlus.Utilities
{
    public static class RoomUtility
    {
        public static void ReturnToSinglePlayer() => NetworkSystem.Instance.ReturnToSinglePlayer();

        public static async void AttemptToJoinPublicRoom()
        {
            if (NetworkSystem.Instance.InRoom)
            {
                await NetworkSystem.Instance.ReturnToSinglePlayer();
            }

            string zone = PhotonNetworkController.Instance.currentJoinTrigger == null ? "forest" : PhotonNetworkController.Instance.currentJoinTrigger.networkZone;

            PhotonNetworkController.Instance.AttemptToJoinPublicRoom(GorillaComputer.instance.GetJoinTriggerForZone(zone));
        }

        public static void AttemptToJoinSpecificRoom()
        {  
            string code = Configuration.RoomCode.Value.ToUpper();

            if (string.IsNullOrEmpty(code))
            {
                LogSource.LogWarning("No room code set!");
                return;
            }

            PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(code, JoinType.Solo);
        }
    }
}
