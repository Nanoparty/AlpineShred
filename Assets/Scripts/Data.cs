using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static bool Music = true;
    public static float MusicVolume = 0.5f;

    public static bool Sound = true;
    public static float SoundVolume = 0.5f;

    public static void LoadPrefs()
    {
        Music = PlayerPrefs.GetInt("Music") == 1;
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        Sound = PlayerPrefs.GetInt("Sound") == 1;
        SoundVolume = PlayerPrefs.GetFloat("SoundVolume");
    }

    public static void SavePrefs()
    {
        PlayerPrefs.SetInt("Music", Convert.ToInt32(Music));
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetInt("Sound", Convert.ToInt32(Sound));
        PlayerPrefs.SetFloat("SoundVolume", SoundVolume);
    }
}
