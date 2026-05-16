using System.Collections.Generic;
using ScoreboardPlusPlus.Models;
using ScoreboardPlusPlus.Tools;
using ScoreboardPlusPlus.Utilities;
using UnityEngine;
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

                Lines.Add(new ScoreboardLine
                {
                    LocalLine = line.gameObject,
                    Nickname = line.Find("Name").GetComponent<Text>(),
                    Swatch = line.Find("Swatch").GetComponent<Image>()
                });
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