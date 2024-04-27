using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using LCSoundTool;
using UnityEngine.SceneManagement;
using BepInEx.Bootstrap;
using System;
using System.Runtime.CompilerServices;

namespace ShipMusic
{
    [BepInPlugin(GUID,NAME,VERSION)]
    [BepInDependency("LCSoundTool", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("BMX.LobbyCompatibility", BepInDependency.DependencyFlags.SoftDependency)]
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
            musicVolume = Config.Bind("General", "SoundVolume", 1f, new ConfigDescription("Volume of the music", new AcceptableValueRange<float>(0,1)));
            maxMusicDistance = Config.Bind("General", "MaxDistance", 25f, "The maximum distance from which the music can reach the player listener");
            enableFilter = Config.Bind("General", "SoundFilter", false, "When true, adds a filter that makes the music sound like it's heard from an actual old radio.");
            SceneManager.sceneLoaded += OnSceneLoaded;

            SetModCompatibility();

            mls.LogInfo("Plugin loaded, and the ship now has music constantly playing in it. how fun");
        }


        private void SetModCompatibility()
        {
            if (!Chainloader.PluginInfos.ContainsKey("BMX.LobbyCompatibility"))
            {
                mls.LogInfo("BMX.LobbyCompatibility wasn't found, skipped checking everything else.");
                return;
            }

            var method = AccessTools.Method("LobbyCompatibility.Features.PluginHelper:RegisterPlugin");
            if(method == null)
            {
                mls.LogError("failed to get BMX.LobbyCompatibility RegisterPlugin method, as it is somehow null");
                return;
            }

            mls.LogInfo("Registering mod to BMX.LobbyCompatibility..");

            try
            {
                method.Invoke(null, new object[] { GUID, new Version(VERSION), 0, 0 });
            }
            catch
            {
                mls.LogError("got an error while registering mod, so this mod probably isn't registered.");
                return;
            }

            mls.LogInfo("Registered mod with BMX.LobbyCompatibility");
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
