using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] Button PlayButton;
    [SerializeField] Button OptionsButton;
    [SerializeField] Button ScoresButton;
    [SerializeField] Button QuitButton;

    [SerializeField] GameObject OptionsMenu;
    [SerializeField] Toggle Music;
    [SerializeField] Toggle Sound;
    [SerializeField] Slider MusicVolume;
    [SerializeField] Slider SoundVolume;
    [SerializeField] Button ApplyButton;

    [SerializeField] GameObject ScoresMenu;
    [SerializeField] Button CloseButton;

    [SerializeField] Camera MainCamera;
    [SerializeField] CinemachineVirtualCamera MenuCamera;
    [SerializeField] CinemachineVirtualCamera OptionsCamera;
    [SerializeField] CinemachineVirtualCamera ScoresCamera;

    private void Awake()
    {
        Data.LoadPrefs();
    }

    private void Start()
    {
        SoundManager.Instance.PlayTitleMusic();

        OptionsCamera.gameObject.SetActive(false);
        ScoresCamera.gameObject.SetActive(false);

        OptionsMenu.SetActive(false);
        ScoresMenu.SetActive(false);

        PlayButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Mountain", LoadSceneMode.Single);
        });

        OptionsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayClick();
            SoundManager.Instance.PlayWhoosh();

            MainMenu.SetActive(false);
            OptionsMenu.SetActive(true);
            OptionsCamera.gameObject.SetActive(true);
            MenuCamera.gameObject.SetActive(false);

            SetOptionsValues();
        });

        ScoresButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayClick();
            SoundManager.Instance.PlayWhoosh();

            MainMenu.SetActive(false);
            ScoresMenu.SetActive(true);
            ScoresCamera.gameObject.SetActive(true);
            MenuCamera.gameObject.SetActive(false);
        });

        QuitButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayClick();

            Application.Quit();
        });

        Music.onValueChanged.AddListener((value) =>
        {
            Data.Music = Music.isOn;
            SoundManager.Instance.UpdatePlaying();
            SoundManager.Instance.PlayClick();
            SoundManager.Instance.UpdatePlaying();
        });

        Sound.onValueChanged.AddListener((value) =>
        {
            Data.Sound = Sound.isOn;
            SoundManager.Instance.PlayClick();
        });

        MusicVolume.onValueChanged.AddListener((value) =>
        {
            SoundManager.Instance.UpdateMusicVolume(MusicVolume.value);
        });

        SoundVolume.onValueChanged.AddListener((value) =>
        {
            SoundManager.Instance.UpdateSoundVolume(SoundVolume.value);
        });

        ApplyButton.onClick.AddListener(() =>
        {
            SaveOptionsValues();

            SoundManager.Instance.UpdateMusicVolume();
            SoundManager.Instance.UpdateSoundVolume();

            SoundManager.Instance.PlayClick();
            SoundManager.Instance.PlayWhoosh();

            MenuCamera.gameObject.SetActive(true);
            OptionsCamera.gameObject.SetActive(false);
            OptionsMenu.SetActive(false);
            MainMenu.SetActive(true);
        });

        CloseButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayClick();
            SoundManager.Instance.PlayWhoosh();

            MenuCamera.gameObject.SetActive(true);
            ScoresCamera.gameObject.SetActive(false);
            ScoresMenu.SetActive(false);
            MainMenu.SetActive(true);
        });

    }

    private void SetOptionsValues()
    {
        Music.isOn = Data.Music;
        Sound.isOn = Data.Sound;

        MusicVolume.value = Data.MusicVolume;
        SoundVolume.value = Data.SoundVolume;
    }

    private void SaveOptionsValues()
    {
        Data.Music = Music.isOn;
        Data.Sound = Sound.isOn;
        Data.MusicVolume = MusicVolume.value;
        Data.SoundVolume = SoundVolume.value;
        Data.SavePrefs();
    }
}
