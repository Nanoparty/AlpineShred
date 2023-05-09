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

    public static List<(string score, string time)> Scores;
    public static (string score, string time) TopScore;

    public static List<(string, double)> CloseOnlineScores;
    public static List<(string, double)> BestOnlineScores;

    public static bool HasChanged = false;

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

    public static void SaveScores()
    {
        ES3.Save("Scores", Scores);
        ES3.Save("TopScore", TopScore);
    }

    public static void LoadScores()
    {
        if (ES3.FileExists("SaveFile.es3")) {
            Scores = (List<(string, string)>)ES3.Load("Scores");
            TopScore =((string, string))ES3.Load("TopScore");
        }
    }
}
