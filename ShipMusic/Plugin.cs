using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using LCSoundTool;
using UnityEngine.SceneManagement;

namespace ShipMusic
{
    [BepInPlugin(GUID,NAME,VERSION)]
    [BepInDependency("LCSoundTool", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "command.ShipMusic";
        public const string NAME = "ShipMusic";
        public const string VERSION = "1.0.0";
        public static AudioClip selectedClip;
        public static ConfigEntry<float> musicVolume;
        public static ConfigEntry<float> maxMusicDistance;
        public static ConfigEntry<bool> enableFilter;
        public static ManualLogSource mls { get; private set; }

        private void Awake()
        {
            mls = Logger;
            musicVolume = Config.Bind("General", "Volume", 1f, new ConfigDescription("Volume of the music", new AcceptableValueRange<float>(0,1)));
            maxMusicDistance = Config.Bind("General", "MaxDistance", 25f, "The maximum distance from which the music can reach the player listener");
            enableFilter = Config.Bind("General", "Sound Filter", false, "When true, adds a special filter that makes the music sound like it's actually coming from a old speaker (not recommended, the filter isn't great, it's just there in case you want the 144p music experience)");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            if(scene.name == "SampleSceneRelay")
            {
                GameObject entireMod = new GameObject("ShipMusicController");
                entireMod.AddComponent<MusicScript>();
                DontDestroyOnLoad(entireMod);
                entireMod.hideFlags = HideFlags.HideAndDontSave;
                mls.LogInfo("added the music to the ship!");
            }
            if(scene.name == "MainMenu")
            {
                GameObject entireMod = GameObject.Find("ShipMusicController");
                GameObject.Destroy(entireMod);
                mls.LogInfo("in main menu, destroyed music object");
            }
        }
    }
}
