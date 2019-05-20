using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSource : MonoBehaviour
{
    [SerializeField] AudioClip[] Playlist;

    [SerializeField]
    Button soundButton;

    [SerializeField]
    Sprite[] soundSprites;

    private AudioSource musicAudioSource;

    private void Awake()
    {
        musicAudioSource = GetComponent<AudioSource>();
        Random.InitState((int)System.DateTime.Now.Ticks);

        if (PlayerPrefsManager.IsSoundOn())
        {
            soundButton.GetComponent<Image>().sprite = soundSprites[1];
            AudioListener.volume = 1;
        }
        else if (!PlayerPrefsManager.IsSoundOn())
        {
            soundButton.GetComponent<Image>().sprite = soundSprites[0];
            AudioListener.volume = 0;
        }
    }

    void Start()
    {
        musicAudioSource.clip = Playlist[Random.Range(0,Playlist.Length)];
        musicAudioSource.Play();
    }

    public void ToggleSound()
    {
        //if (Application.platform != RuntimePlatform.Android) { return; }

        if (PlayerPrefsManager.IsSoundOn())
        {
            AudioListener.volume = 0;
            soundButton.GetComponent<Image>().sprite = soundSprites[0];
            PlayerPrefsManager.SetSoundOff();
        }
        else if (!PlayerPrefsManager.IsSoundOn())
        {
            AudioListener.volume = 1;
            soundButton.GetComponent<Image>().sprite = soundSprites[1];
            PlayerPrefsManager.SetSoundOn();
        }
    }
}
