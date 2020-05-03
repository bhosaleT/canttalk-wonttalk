using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField]
    AudioClip fightTheme;

    [SerializeField]
    AudioClip menuTheme;

    [SerializeField]
    AudioSource actualAudioSource;

    void Awake() {
        actualAudioSource = this.GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayFightTheme() {
        Debug.Log("Play fight theme");
        actualAudioSource.clip = fightTheme;
        actualAudioSource.Play();

    }

    public void PlayMenuTheme() {

        actualAudioSource.clip = menuTheme;
        actualAudioSource.Play();
    }

}