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

namespace TFOOL.Audio
{
    public class FMODAudioPlayer : MonoBehaviour
    {
        [SerializeField] private FMODSound[] sounds;

        private readonly Dictionary<string, FMODSound> soundLookup = new Dictionary<string, FMODSound>();

        #region Nested
        private enum SoundType
        {
            EventReference
        }
        [System.Serializable]
        private struct FMODSound
        {
            [SerializeField] internal string soundName;
            [SerializeField, Tooltip("Controls what type of sound this is.  Use EventEmitter for spatial sounds.")] 
            internal SoundType soundType;
            [SerializeField, AllowNesting, ShowIf("soundType", SoundType.EventReference)] internal EventReference eventReference;
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
                default:
                    break;
            }
        }
        #endregion
    }
}