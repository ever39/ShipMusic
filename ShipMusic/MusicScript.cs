using LCSoundTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace ShipMusic
{
    internal class MusicScript : MonoBehaviour
    {
        public static GameObject hangarShip;
        public static GameObject musicObj;
        private static AudioClip musicClip;
        public static AudioSource shipMusicSource;
        public static AudioMixerGroup gameMixerGroup;

        private void Update()
        {
            try
            {
                if (hangarShip == null)
                {
                    hangarShip = GameObject.Find("/Environment/HangarShip/");
                    gameMixerGroup = hangarShip.GetComponent<AudioSource>().outputAudioMixerGroup;
                    if (musicClip == null)
                    {
                        try
                        {
                            Plugin.mls.LogWarning("no file loaded, checking for files in the folder!");
                            musicClip = SoundTool.GetAudioClip("Command293-ShipMusic", "music.wav");
                        }
                        catch (Exception)
                        {
                            try
                            {
                                Plugin.mls.LogWarning("no wav, trying ogg!");
                                musicClip = SoundTool.GetAudioClip("Command293-ShipMusic", "music.ogg");
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    Plugin.mls.LogWarning("no wav, no ogg, trying mp3!");
                                    musicClip = SoundTool.GetAudioClip("Command293-ShipMusic", "music.mp3");
                                }
                                catch (Exception)
                                {
                                    Plugin.mls.LogError("Help. There's no files. ");
                                }
                            }
                        }
                    }
                    if (hangarShip != null && musicObj == null)
                    {
                            musicObj = new GameObject("ShipMusicSource");
                            musicObj.transform.SetParent(hangarShip.transform, false);
                            musicObj.transform.rotation = Quaternion.identity;
                            musicObj.transform.localScale = Vector3.one;
                            musicObj.transform.localPosition = new Vector3(2.2f, 0.5f, -6.7f);
                            musicObj.AddComponent<AudioSource>();

                            shipMusicSource = musicObj.GetComponent<AudioSource>();

                            if (Plugin.enableFilter.Value)
                            {
                                Plugin.mls.LogInfo("filter config is enabled, adding filters...");

                                musicObj.AddComponent<AudioHighPassFilter>();
                                musicObj.AddComponent<AudioDistortionFilter>();
                                musicObj.AddComponent<AudioLowPassFilter>();
                                musicObj.GetComponent<AudioHighPassFilter>().cutoffFrequency = 4150;
                                musicObj.GetComponent<AudioDistortionFilter>().distortionLevel = 0.92f;
                                musicObj.GetComponent<AudioLowPassFilter>().cutoffFrequency = 3064;

                                Plugin.mls.LogInfo("Added filters! unfortunately you can't remove them in-game yet, as that would need the mod to destroy this gameobject");
                            }

                            shipMusicSource.dopplerLevel = 0;
                            shipMusicSource.spatialBlend = 1;
                            shipMusicSource.volume = Plugin.musicVolume.Value;
                            shipMusicSource.loop = true;
                            shipMusicSource.priority = 0;
                            shipMusicSource.playOnAwake = false;
                            shipMusicSource.maxDistance = Plugin.maxMusicDistance.Value;
                            shipMusicSource.minDistance = 1;
                            shipMusicSource.rolloffMode = AudioRolloffMode.Linear;
                            shipMusicSource.clip = musicClip;
                            shipMusicSource.outputAudioMixerGroup = gameMixerGroup;
                            shipMusicSource.Play();

                            Plugin.mls.LogInfo("made ship music gameobject!");
                    }
                }
            }
            catch(Exception e)
            {
                Plugin.mls.LogError($"found error while making gameobject:\n{e}");
            }

            shipMusicSource.maxDistance = Plugin.maxMusicDistance.Value;
            shipMusicSource.volume = Plugin.musicVolume.Value;
        }
    }
}
