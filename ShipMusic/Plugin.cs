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
            musicVolume = Config.Bind("General", "Volume", 1f, new ConfigDescription("Volume of the music", new AcceptableValueRange<float>(0,1)));
            maxMusicDistance = Config.Bind("General", "MaxDistance", 25f, "The maximum distance from which the music can reach the player listener");
            enableFilter = Config.Bind("General", "Sound Filter", false, "When true, adds a special filter that makes the music sound like it's actually coming from a old speaker (not recommended, the filter isn't great, it's just there in case you want the 144p music experience)");
            SceneManager.sceneLoaded += OnSceneLoaded;
            SetModCompatibility();
            mls.LogInfo("Loaded!");
        }


        private void SetModCompatibility()
        {
            if (!Chainloader.PluginInfos.ContainsKey("BMX.LobbyCompatibility"))
            {
                mls.LogInfo("no lobby compatibility");
                return;
            }

            var method = AccessTools.Method("LobbyCompatibility.Features.PluginHelper:RegisterPlugin");
            if(method is null)
            {
                mls.LogWarning("failed to get BMX.LobbyCompatibility RegisterPlugin method, as it is null");
                return;
            }

            mls.LogInfo("registering mod to BMX.LobbyCompatibility");
            try
            {
                method.Invoke(null, new object[] { GUID, new Version(VERSION), 0, 0 });
            }
            catch(Exception e)
            {
                mls.LogError($"got error while registering mod\n {e}");
                return;
            }

            mls.LogInfo("registered mod with LobbyCompatibility!");
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
