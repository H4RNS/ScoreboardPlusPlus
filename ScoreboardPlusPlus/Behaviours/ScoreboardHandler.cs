using System.Collections.Generic;
using Photon.Realtime;
using ScoreboardPlusPlus.Models;
using ScoreboardPlusPlus.Tools;
using ScoreboardPlusPlus.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ScoreboardPlusPlus.Behaviours
{
    public class ScoreboardHandler : MonoBehaviour
    {
        private readonly List<ScoreboardLine> Lines = [];

        private GameObject Scoreboard, Notice;
        private Transform LineParent;

        private void Awake()
        {
            Scoreboard = ContentLoader.GetContent<GameObject>("Scoreboard");
            LineParent = Scoreboard.transform.Find("Canvas/LineParent");
            Notice = Scoreboard.transform.Find("Canvas/Notice").gameObject;

            foreach (Transform line in LineParent)
            {
                line.gameObject.SetActive(false);

                bool open = false;

                ScoreboardLine playerLine = new()
                {
                    LocalLine = line.gameObject,

                    Nickname = line.Find("Name").GetComponent<Text>(),
                    Swatch = line.Find("Swatch").GetComponent<Image>(),

                    MuteButton = line.Find("InspectOptions/Mute"),
                    HateSpeechButton = line.Find("InspectOptions/HateSpeach"),
                    ToxicityButton = line.Find("InspectOptions/Toxicity"),
                    CheatingButton = line.Find("InspectOptions/Cheating"),
                    InpectButton = line.Find("InspectOptions/Inspect"),
                };

                PushButton.CreateDynamic(playerLine.InpectButton, ["Inspect", "Close"], () =>
                {
                    open = !open;

                    playerLine.MuteButton.gameObject.SetActive(open);
                    playerLine.HateSpeechButton.gameObject.SetActive(open);
                    playerLine.ToxicityButton.gameObject.SetActive(open);
                    playerLine.CheatingButton.gameObject.SetActive(open);
                });

                PushButton.CreateStatic(playerLine.MuteButton, () =>
                {

                });

                Lines.Add(playerLine);
            }

            RoomSystem.JoinedRoomEvent += JoinedRoomEvent;
            RoomSystem.LeftRoomEvent += LeftRoomEvent;
            RoomSystem.PlayerJoinedEvent += PlayerJoinedEvent;
            RoomSystem.PlayerLeftEvent += PlayerLeftEvent;

            RefreshScoreboard();
        }

        private void RefreshScoreboard()
        {
            foreach (var line in Lines)
            {
                line.LocalLine.SetActive(false);
            }

            if (!NetworkSystem.Instance.InRoom)
            {
                Notice.SetActive(true);
                return;
            }
            else
            {
                Notice.SetActive(false);
            }

            NetPlayer[] players = NetworkSystem.Instance.AllNetPlayers;
            System.Array.Sort(players, (a, b) => a.ActorNumber.CompareTo(b.ActorNumber));

            int index = 0;

            foreach (NetPlayer player in players)
            {
                if (player == null || player.IsNull)
                    continue;

                if (index >= Lines.Count)
                    break;

                ScoreboardLine line = Lines[index];

                line.LocalLine.SetActive(true);
                line.Nickname.text = player.NickName;
                line.Player = player;

                PlayerUtility.PlayerSwatch(line.Swatch, player);

                LogSource.LogInfo($"Scoreboard line created -> NickName '{player.NickName}', ActorNumber '{player.ActorNumber}'");

                index++;
            }
        }

        private void Update()
        {
            foreach (var line in Lines)
            {
                if (line.Player == null || line.Player.IsNull)
                    continue;

                PlayerUtility.PlayerSwatch(line.Swatch, line.Player);
            }
        }

        private void JoinedRoomEvent() => RefreshScoreboard();
        private void LeftRoomEvent() => RefreshScoreboard();
        private void PlayerJoinedEvent(NetPlayer player) => RefreshScoreboard();
        private void PlayerLeftEvent(NetPlayer player) => RefreshScoreboard();
    }
}