using UnityEngine;
using System;

public class AnimationSoundPlayer : MonoBehaviour
{
    public void SoundEvent(string s)
    {
        AudioManager.PlaySound(s);
    }
}
