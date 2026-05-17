using System;
using Photon.Pun;
using ScoreboardPlusPlus.Tools;
using ScoreboardPlusPlus.Utilities;
using UnityEngine;

namespace ScoreboardPlusPlus.Behaviours
{
    public class CoreBehaviour : MonoBehaviour
    {
        public static CoreBehaviour Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            ContentLoader.LoadContent<GameObject>("Scoreboard", "GorillaScoreBoard++").AddComponent<ScoreboardHandler>();

            Transform _functions = ContentLoader.GetContent<GameObject>("Scoreboard").transform.Find("Canvas/Functions");
            Transform _lineParent = ContentLoader.GetContent<GameObject>("Scoreboard").transform.Find("Canvas/LineParent");

            try
            {
                PushButton.CreateStatic(_functions.transform.Find("QuickAction"), () =>
                {
                    switch (Configuration.ActionButton.Value)
                    {
                        case Configuration.ActionType.Disconnect:
                            RoomUtility.ReturnToSinglePlayer(); 
                            break;

                        case Configuration.ActionType.JoinRandom:
                            RoomUtility.AttemptToJoinPublicRoom();
                            break;

                        case Configuration.ActionType.JoinSpecific:
                            RoomUtility.AttemptToJoinSpecificRoom();
                            break;
                    }
                });

                PushButton.CreateDynamic(_functions.transform.Find("Options"), ["Options", "Exit Options"], () =>
                {
                    _lineParent.gameObject.SetActive(!_lineParent.gameObject.activeSelf);
                });
            }
            catch (Exception ex)
            {
                LogSource.LogError($"Unable to create push buttons: {ex}");
            }
        }
    }
}
