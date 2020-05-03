using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenuMusic : MonoBehaviour {

    [SerializeField]
    AudioSource menuMusicSource;

    [SerializeField]
    AudioClip menuClip;

    public void PlayMenuSound() {
        menuMusicSource.Play();
    }

}