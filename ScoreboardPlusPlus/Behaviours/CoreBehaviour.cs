using System;
using ScoreboardPlusPlus.Tools;
using ScoreboardPlusPlus.Utilities;
using UnityEngine;
using UnityEngine.UI;

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

            // ignore the code below im lazy rn im just pushing so i can work on it later, ts so crappy wah.
            Transform _functions = ContentLoader.GetContent<GameObject>("Scoreboard").transform.Find("Canvas/Functions");

            Text button = _functions.transform.Find("QuickAction/Text").GetComponent<Text>();

            Transform _lineParent = ContentLoader.GetContent<GameObject>("Scoreboard").transform.Find("Canvas/LineParent");

            try
            {
                PushButton.CreateStatic(_functions.transform.Find("QuickAction"), () =>
                {
                    switch (Configuration.ActionButton.Value)
                    {
                        case Configuration.ActionType.Disconnect:
                            RoomUtility.ReturnToSinglePlayer();
                            button.text = "Disconnect";
                            break;

                        case Configuration.ActionType.JoinRandom:
                            RoomUtility.AttemptToJoinPublicRoom();
                            button.text = "Join Random";
                            break;

                        case Configuration.ActionType.JoinSpecific:
                            RoomUtility.AttemptToJoinSpecificRoom();
                            button.text = $"Join Room: {Configuration.RoomCode.Value.ToUpper()}";
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
