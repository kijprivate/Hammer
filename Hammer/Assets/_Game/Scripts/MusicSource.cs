using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSource : MonoBehaviour
{
    [SerializeField] AudioClip[] Playlist;

    private AudioSource musicAudioSource;
    private static MusicSource instance = null;

    private void Awake()
    {
        //singleton
        if(instance == null)
        { instance = this; }
        else if(instance != this)
        { Destroy(gameObject); }

        musicAudioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        musicAudioSource.clip = Playlist[1];
        musicAudioSource.Play();
    }

}
