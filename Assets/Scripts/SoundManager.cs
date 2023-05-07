using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    AudioSource musicSource;
    AudioSource soundSource;

    public AudioClip menuMusic;

    public AudioClip click;
    public AudioClip whoosh;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        musicSource = GetComponent<AudioSource>();
        soundSource = transform.GetChild(0).GetComponent<AudioSource>();

        musicSource.volume = Data.MusicVolume;
        soundSource.volume = Data.SoundVolume;
    }

    public void UpdateMusicVolume()
    {
        musicSource.volume = Data.MusicVolume * 0.05f;
    }

    public void UpdateSoundVolume()
    {
        soundSource.volume = Data.SoundVolume;
    }

    public void UpdatePlaying()
    {
        if (!Data.Music && musicSource.isPlaying)
        {
            StopMusic();
        }
        if (Data.Music && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void PlayTitleMusic()
    {
        if (!Data.Music) return;
        if (musicSource.clip == menuMusic && musicSource.isPlaying) return;
        musicSource.volume = Data.MusicVolume * 0.05f;
        musicSource.clip = menuMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayClick()
    {
        if (!Data.Sound) return;
        soundSource.PlayOneShot(click);
    }
    public void PlayWhoosh()
    {
        if (!Data.Sound) return;
        soundSource.PlayOneShot(whoosh, 0.5f);
    }

}
