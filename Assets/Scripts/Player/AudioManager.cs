using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    private AudioSource audioSource;
    [SerializeField] private AudioSource _musicSource;

    [SerializeField] private List<ClipAndEnum> _sounds;
    [SerializeField] private AudioClip[] _musicClips;


    public static AudioManager Instance { get => instance; set => instance = value; }
    public List<ClipAndEnum> Sounds { get => _sounds; set => _sounds = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public static void PlaySound(string soundID)
    {
        PlaySound(soundID, 1f);
    }

    public static void PlaySound(string soundID, float volume = 1f)
    {
        instance.audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        instance.audioSource.PlayOneShot(instance.GetAudioClip(soundID));
    }

    public static void PlaySoundAtPosition(string soundID, Vector2 position)
    {
        PlaySoundAtPosition(soundID, position, 1f);
    }
    public static void PlaySoundAtPosition(string soundID, Vector2 position, float volume)
    {
        instance.transform.position = position;
        PlaySound(soundID, volume);
    }

    public AudioClip GetAudioClip(string soundID)
    {
        foreach (ClipAndEnum c in _sounds)
        {
            if (c.ClipName == soundID)
            {
                return c.ChooseAtRandom ? c.Clips[UnityEngine.Random.Range(0, c.Clips.Length)] : c.Clips[0];
            }
        }
        Debug.LogError("No sound of ID " + soundID + " found in AudioManager!");
        return null;
    }
}

[System.Serializable]
public struct ClipAndEnum
{
    [SerializeField] private string _clipName;
    [SerializeField] private AudioClip[] _clips;
    [SerializeField] private bool _chooseAtRandom;

    public string ClipName { get => _clipName; set => _clipName = value; }
    public AudioClip[] Clips { get => _clips; set => _clips = value; }
    public bool ChooseAtRandom { get => _chooseAtRandom; set => _chooseAtRandom = value; }
}
