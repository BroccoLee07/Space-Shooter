using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource sfxSource;

    public void PlaySoundEffect(AudioClip audioClip) { 
        sfxSource.clip = audioClip;
        sfxSource.Play();
    }
}
