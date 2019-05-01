using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSource : MonoBehaviour
{
    [SerializeField] AudioClip[] Playlist;

    private AudioSource musicAudioSource;
    //private static MusicSource instance = null;

    private void Awake()
    {
        musicAudioSource = GetComponent<AudioSource>();
        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    void Start()
    {
        musicAudioSource.clip = Playlist[Random.Range(0,Playlist.Length)];
        musicAudioSource.Play();
    }

}
