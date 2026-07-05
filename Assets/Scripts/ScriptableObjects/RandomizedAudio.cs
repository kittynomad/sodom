using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * RandomizedAudio's a very simple class basically for easily storing
 * multiple audio clips in a place where they can be easily reused
 * (mainly meant for banjo-kazooie type voices)
 */
[CreateAssetMenu]
public class RandomizedAudio : ScriptableObject
{
    
    public AudioClip[] clips;
}
