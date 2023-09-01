using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource sfxSource;

    // TODO: For sound effects, create audio source for each necessary object (asteroid, enemy, player, laser) and
    // TODO: handle playing sound effect there 
    public void PlaySoundEffect(AudioClip audioClip, float volume = 0.25f) {
        sfxSource.PlayOneShot(audioClip, volume);
    }
}
