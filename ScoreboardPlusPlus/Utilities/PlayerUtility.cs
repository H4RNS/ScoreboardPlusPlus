using GorillaNetworking;
using Photon.Realtime;
using Photon.Voice;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

namespace ScoreboardPlusPlus.Utilities
{
    public static class PlayerUtility
    {
        //Code Modified from: https://github.com/developer9998/GorillaInfoWatch/blob/main/GorillaInfoWatch/Models/Widgets/WidgetController_PlayerSwatch.cs

        public static void PlayerSwatch(Image swatch, NetPlayer player)
        {
            VRRig vrRig = null;

            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (rig != null && rig.Creator == player)
                {
                    vrRig = rig;
                    break;
                }
            }

            if (swatch == null || vrRig == null)
                return;

            Material material;
            Color colour;

            int setMatIndex = vrRig.setMatIndex;

            if (setMatIndex == 0)
            {
                material = vrRig.scoreboardMaterial;
                colour = vrRig.playerColor;
            }
            else
            {
                Material designatedMaterial = vrRig.materialsToChangeTo[setMatIndex];
                material = designatedMaterial;
                colour = designatedMaterial.color;
            }

            if (swatch.material != material) swatch.material = material;

            if (swatch.color != colour) swatch.color = colour;
        }

        public static bool CheckIfTalking(NetPlayer _player, VRRig _rig)
        {
            if(_player == null) return false;
            if(GorillaComputer.instance.voiceChatOn == "FALSE") return false;

            if (_rig.remoteUseReplacementVoice || _rig.localUseReplacementVoice)
                return _rig.SpeakingLoudness > _rig.replacementVoiceLoudnessThreshold;

            if (_player.IsLocal)
            {
                Recorder _recorder = NetworkSystem.Instance.LocalRecorder;
                return _recorder != null && _recorder.IsCurrentlyTransmitting;
            }

            Speaker _speaker = _rig.GetComponentInChildren<Speaker>();
            return _speaker != null && _speaker.IsPlaying;
        }
    }
}