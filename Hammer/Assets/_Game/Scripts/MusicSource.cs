using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSource : MonoBehaviour
{
    [SerializeField] AudioClip[] Playlist;

    private AudioSource musicAudioSource;

    private void Awake()
    {
        musicAudioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        musicAudioSource.clip = Playlist[1];
        musicAudioSource.Play();
    }

}
