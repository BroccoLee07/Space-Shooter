using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource sfxSource;

    public void PlaySoundEffect(AudioClip audioClip, float volume = 0.25f) { 
        sfxSource.clip = audioClip;
        sfxSource.volume = volume;
        sfxSource.Play();
    }
}
