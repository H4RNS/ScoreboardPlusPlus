using System.Collections.Generic;
using BepInEx;
using ScoreboardPlusPlus.Behaviours;
using ScoreboardPlusPlus.Tools;
using UnityEngine;

namespace ScoreboardPlusPlus
{
    [BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static readonly List<string> COCTextObjects = [];

        void Awake()
        {
            GorillaTagger.OnPlayerSpawned(() =>
            {
                DontDestroyOnLoad(new GameObject(Constants.Name, typeof(CoreBehaviour)));

                COCTextObjects.Add("Environment Objects/LocalObjects_Prefab/TreeRoom/CodeOfConductHeadingText");
                COCTextObjects.Add("Environment Objects/LocalObjects_Prefab/TreeRoom/COCBodyText_TitleData");

                COCTextObjects.ForEach(Object => GameObject.Find(Object).SetActive(false));

                Configuration.BuildFile(Config);
            });
        }
    }
}

