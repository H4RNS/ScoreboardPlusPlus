using UnityEngine;
using UnityEngine.UI;

namespace ScoreboardPlusPlus.Models
{
    public class ScoreboardLine
    {
        public GameObject LocalLine;

        public Text Nickname;
        public Image Swatch;

        public Transform MicParent;

        public Transform MuteButton;
        public Transform HateSpeechButton;
        public Transform ToxicityButton;
        public Transform CheatingButton;
        public Transform InpectButton;

        public NetPlayer Player;
    }
}
