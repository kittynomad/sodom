/*****************************************************************************
// File Name : FMODAudioManager.cs
// Author : Arcadia Koederitz
// Creation Date : 8/30/2026
// Last Modified : 8/30/2026
//
// Brief Description : Handles playing audio through the FMOD runtime manager.
*****************************************************************************/
using FMOD.Studio;
using FMODUnity;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Timeline.AnimationPlayableAsset;

namespace TFOOL.Audio
{
    public class FMODAudioPlayer : MonoBehaviour
    {
        [SerializeField] private FMODSound[] sounds;

        private readonly Dictionary<string, FMODSound> soundLookup = new Dictionary<string, FMODSound>();

        #region Nested
        private enum SoundType
        {
            EventReference,
            EventEmitter
        }
        [System.Serializable]
        private struct FMODSound
        {
            [SerializeField] internal string soundName;
            [SerializeField, Tooltip("Controls what type of sound this is.  Use EventEmitter for spatial sounds.")] 
            internal SoundType soundType;
            [SerializeField, AllowNesting, ShowIf("soundType", SoundType.EventReference)] internal EventReference eventReference;
            [SerializeField, AllowNesting, ShowIf("soundType", SoundType.EventEmitter)] internal StudioEventEmitter eventEmitter;

            internal EventInstance instance;
            internal bool isPlaying;
        }
        #endregion

        private void Awake()
        {
            // Convert the sounds array to a dictionary for faster lookup.
            foreach(var sound in sounds)
            {
               if (!soundLookup.ContainsKey(sound.soundName))
               {
                    soundLookup.Add(sound.soundName, sound);
               }
            }
        }

        private FMODSound GetSound(string soundName)
        {
            if (soundName == null) 
            {
                Debug.LogWarning($"AudioPlayer on {name} has no sound named {soundName}");
                return default; 
            }
            if (soundLookup.ContainsKey(soundName))
            {
                return soundLookup[soundName];
            }
            Debug.LogWarning($"AudioPlayer on {name} has no sound named {soundName}");
            return default;
        }

        #region One-Shots
        /// <summary>
        /// Plays a sound as a one-shot sound.
        /// </summary>
        /// <param name="soundName">The name of the sound to play.</param>
        public void PlayOneShot(string soundName)
        {
            FMODSound sound = GetSound(soundName);
            switch(sound.soundType)
            {
                case SoundType.EventReference:
                    RuntimeManager.PlayOneShot(sound.eventReference);
                    break;
                case SoundType.EventEmitter:
                    RuntimeManager.PlayOneShot(sound.eventEmitter.EventReference);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Plays a one shot sound at the position of this game object.
        /// </summary>
        /// <param name="soundName"></param>
        public void PlayOneShotAtPosition(string soundName)
        {
            PlayOneShotAtPosition(soundName, transform.position);
        }
        /// <summary>
        /// Plays a one shot sound at a given position.
        /// </summary>
        /// <param name="soundName"></param>
        /// <param name="position"></param>
        public void PlayOneShotAtPosition(string soundName, Vector3 position)
        {
            FMODSound sound = GetSound(soundName);
            switch (sound.soundType)
            {
                case SoundType.EventReference:
                    RuntimeManager.PlayOneShot(sound.eventReference, position);
                    break;
                case SoundType.EventEmitter:
                    RuntimeManager.PlayOneShot(sound.eventEmitter.EventReference, position);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Persistent Sounds
        /// <summary>
        /// Starts playing a sound over time.
        /// </summary>
        /// <param name="soundName"></param>
        public void StartSound(string soundName)
        {
            FMODSound sound = GetSound(soundName);
            switch (sound.soundType)
            {
                case SoundType.EventReference:
                    if (sound.isPlaying)
                    {
                        Debug.LogWarning($"Sound {soundName} is already playing.");
                        return;
                    }
                    sound.instance = RuntimeManager.CreateInstance(sound.eventReference);
                    sound.instance.start();
                    break;
                case SoundType.EventEmitter:
                    if (sound.eventEmitter.IsPlaying())
                    {
                        Debug.LogWarning($"Sound {soundName} is already playing.");
                        return;
                    }
                    sound.eventEmitter.Play();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Stops a sound that is currently playing.
        /// </summary>
        /// <param name="soundName"></param>
        /// <param name="stopMode"></param>
        public void StopSound(string soundName, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.IMMEDIATE)
        {
            FMODSound sound = GetSound(soundName);
            switch (sound.soundType)
            {
                case SoundType.EventReference:
                    if (!sound.isPlaying)
                    {
                        Debug.LogWarning($"Sound {soundName} is not currently playing.");
                        return;
                    }
                    sound.instance.stop(stopMode);
                    sound.instance.release();
                    break;
                case SoundType.EventEmitter:
                    if (!sound.eventEmitter.IsPlaying())
                    {
                        Debug.LogWarning($"Sound {soundName} is not currently playing.");
                        return;
                    }
                    sound.eventEmitter.Stop();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sets a parameter of a currently playing sound.
        /// </summary>
        /// <param name="soundName"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        public void SetParameter(string soundName, string parameterName, float parameterValue)
        {
            FMODSound sound = GetSound(soundName);
            switch (sound.soundType)
            {
                case SoundType.EventReference:
                    if (!sound.isPlaying)
                    {
                        Debug.LogWarning($"Sound {soundName} is not currently playing.");
                        return;
                    }
                    sound.instance.setParameterByName(parameterName, parameterValue);
                    break;
                case SoundType.EventEmitter:
                    if (!sound.eventEmitter.IsPlaying())
                    {
                        Debug.LogWarning($"Sound {soundName} is not currently playing.");
                        return;
                    }
                    sound.eventEmitter.SetParameter(parameterName, parameterValue);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}