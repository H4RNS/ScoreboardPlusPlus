using System;
using ScoreboardPlusPlus.Tools;
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

            try
            {
                PushButton.CreateStatic(_functions.transform.Find("Disconnect"), () =>
                {
                    NetworkSystem.Instance.ReturnToSinglePlayer();  
                });
                 
                PushButton.CreateDynamic(_functions.transform.Find("Options"), ["Options", "Exit Options"], () =>
                {
                    var _lineParent = ContentLoader.GetContent<GameObject>("Scoreboard").transform.Find("Canvas/LineParent").gameObject;
                    _lineParent.SetActive(!_lineParent.activeSelf);
                });
            }
            catch (Exception ex)
            {
                LogSource.LogError($"Unable to create push buttons: {ex}");
            }
        }
    }
}
