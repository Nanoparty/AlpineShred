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

    private void Start()
    {
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
            MainMenu.SetActive(false);
            OptionsMenu.SetActive(true);
            OptionsCamera.gameObject.SetActive(true);
            MenuCamera.gameObject.SetActive(false);
        });

        ScoresButton.onClick.AddListener(() =>
        {
            MainMenu.SetActive(false);
            ScoresMenu.SetActive(true);
            ScoresCamera.gameObject.SetActive(true);
            MenuCamera.gameObject.SetActive(false);
        });

        QuitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        Music.onValueChanged.AddListener((value) =>
        {

        });

        Sound.onValueChanged.AddListener((value) =>
        {

        });

        MusicVolume.onValueChanged.AddListener((value) =>
        {

        });

        SoundVolume.onValueChanged.AddListener((value) =>
        {

        });

        ApplyButton.onClick.AddListener(() =>
        {
            MenuCamera.gameObject.SetActive(true);
            OptionsCamera.gameObject.SetActive(false);
            OptionsMenu.SetActive(false);
            MainMenu.SetActive(true);
        });

        CloseButton.onClick.AddListener(() =>
        {
            MenuCamera.gameObject.SetActive(true);
            ScoresCamera.gameObject.SetActive(false);
            ScoresMenu.SetActive(false);
            MainMenu.SetActive(true);
        });
    }
}
