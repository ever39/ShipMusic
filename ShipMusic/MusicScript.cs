using LCSoundTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShipMusic
{
    internal class MusicScript : MonoBehaviour
    {
        public static GameObject hangarShip;
        public static GameObject musicObj;
        private static AudioClip musicClip = SoundTool.GetAudioClip("Command293-ShipMusic", "music.mp3");
        public static AudioSource shipMusicSource;

        private void Update()
        {
            try
            {
                if (hangarShip == null)
                {
                    hangarShip = GameObject.Find("/Environment/HangarShip/");
                    musicClip = SoundTool.GetAudioClip("Command293-ShipMusic", "music.mp3");

                    if (hangarShip != null)
                    {
                        musicObj = new GameObject("ShipMusicSource");
                        musicObj.transform.SetParent(hangarShip.transform, false);
                        musicObj.transform.rotation = Quaternion.identity;
                        musicObj.transform.localScale = Vector3.one;
                        musicObj.transform.localPosition = new Vector3(2.2f, 0.5f, -6.7f);
                        musicObj.AddComponent<AudioSource>();

                        shipMusicSource = musicObj.GetComponent<AudioSource>();

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
                        shipMusicSource.Play();
                    }
                }
            }
            catch(Exception e)
            {
                Plugin.mls.LogError($"found exception: {e}");
            }
        }

        /*private void LateUpdate()
        {
            if(hangarShip == null | shipMusicSource.clip == null)
            {
                hangarShip = GameObject.Find("/Environment/HangarShip/");
                shipMusicSource.clip = musicClip;
            }
            if(hangarShip != null && shipMusicSource.clip != null)
            {
                shipMusicSource.Play();
            }
        }*/
    }
}
