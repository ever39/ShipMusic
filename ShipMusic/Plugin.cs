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
        private readonly Harmony har = new Harmony(GUID);

        private void Awake()
        {
            mls = Logger;
            musicVolume = Config.Bind("General", "Volume", 2f, "be glad that i added this");
            maxMusicDistance = Config.Bind("General", "MaxDistance", 25f, "max distance of the sound");
            enableFilter = Config.Bind("General", "Enable Speaker Filter?", false, "Should the mod enable the radio filter?");
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
