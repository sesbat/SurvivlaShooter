using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSound : MonoBehaviour
{
    public SoundData data;
    public float volume {
        get { return audioSource.volume; }
        set { audioSource.volume = value; }
    }
    public bool mute {
        get { return audioSource.mute; }
        set { audioSource.mute = value; }
    }
   AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = data.effectScale;
        audioSource.mute = data.isMute;
    }

    public void PlayOneShot(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
