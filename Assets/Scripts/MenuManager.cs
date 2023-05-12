using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] Button PlayButton;
    [SerializeField] Button OptionsButton;
    [SerializeField] Button CreditsButton;
    [SerializeField] Button ScoresButton;
    [SerializeField] Button QuitButton;

    [SerializeField] GameObject OptionsMenu;
    [SerializeField] Toggle Music;
    [SerializeField] Toggle Sound;
    [SerializeField] Slider MusicVolume;
    [SerializeField] Slider SoundVolume;
    [SerializeField] Button ApplyButton;

    [SerializeField] GameObject ScoresMenu;
    [SerializeField] GameObject ScoresContent;
    [SerializeField] Button LocalButton;
    [SerializeField] Button OnlineButton;
    [SerializeField] Button BestButton;
    [SerializeField] Button CloseButton;

    [SerializeField] GameObject CreditsMenu;
    [SerializeField] Button CreditsBackButton;

    [SerializeField] Camera MainCamera;
    [SerializeField] CinemachineVirtualCamera MenuCamera;
    [SerializeField] CinemachineVirtualCamera OptionsCamera;
    [SerializeField] CinemachineVirtualCamera ScoresCamera;
    [SerializeField] CinemachineVirtualCamera CreditsCamera;

    [SerializeField] GameObject ScorePrefab;
    [SerializeField] GameObject OnlineScorePrefab;
    [SerializeField] GameObject ScoresLoadingPrefab;

    [SerializeField] public Color PlayerScoreColor;

    private bool waitingForOnlineScores = false;
    private bool waitingForBestScores = false;

    private void Awake()
    {
        Data.LoadPrefs();
        Data.LoadScores();
    }

    private void Start()
    {
        SoundManager.Instance.PlayTitleMusic();

        if (Data.Scores == null) Data.Scores = new List<(string score, string time)>();
        //Data.Scores.Add(("5", "1:23"));

        OptionsCamera.gameObject.SetActive(false);
        ScoresCamera.gameObject.SetActive(false);
        CreditsCamera.gameObject.SetActive(false);

        OptionsMenu.SetActive(false);
        ScoresMenu.SetActive(false);
        CreditsMenu.SetActive(false);

        PlayButton.onClick.AddListener(() =>
        {
            //SceneManager.LoadScene("Mountain", LoadSceneMode.Single);
            GameObject.FindGameObjectWithTag("Transition").GetComponent<CircleTransition>().StartGameTransition();
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

        CreditsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayClick();
            SoundManager.Instance.PlayWhoosh();

            MainMenu.SetActive(false);
            CreditsMenu.SetActive(true);
            CreditsCamera.gameObject.SetActive(true);
            MenuCamera.gameObject.SetActive(false);

            //SetOptionsValues();
        });

        ScoresButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayClick();
            SoundManager.Instance.PlayWhoosh();

            MainMenu.SetActive(false);
            ScoresMenu.SetActive(true);
            ScoresCamera.gameObject.SetActive(true);
            MenuCamera.gameObject.SetActive(false);

            PopulateLocalScores();
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

        LocalButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayClick();
            PopulateLocalScores();
        });

        OnlineButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayClick();
            PopulateOnlineScores();
        });

        BestButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayClick();
            PopulateBestScores();
        });

        CreditsBackButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.UpdateMusicVolume();
            SoundManager.Instance.UpdateSoundVolume();

            SoundManager.Instance.PlayClick();
            SoundManager.Instance.PlayWhoosh();

            MenuCamera.gameObject.SetActive(true);
            CreditsCamera.gameObject.SetActive(false);
            CreditsMenu.SetActive(false);
            MainMenu.SetActive(true);
        });

    }

    private void Update()
    {
        if (waitingForOnlineScores)
        {
            Debug.Log("Waiting for scores");
            if (Data.CloseOnlineScores != null)
            {
                Debug.Log("Scores Found");
                waitingForOnlineScores = false;
                PopulateOnlineScores();
            }
        }

        if (waitingForBestScores)
        {
            if (Data.BestOnlineScores != null)
            {
                waitingForBestScores = false;
                PopulateBestScores();
            }
        }
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

    private void DestroyAllChildren(GameObject o)
    {
        foreach (Transform child in o.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void PopulateLocalScores()
    {
        DestroyAllChildren(ScoresContent);

        LocalButton.Select();

        List<(string, string)> allScores = Data.Scores ?? new List<(string score, string time)>();
        var sortedScores = allScores.OrderByDescending(o => int.Parse(o.Item1)).ToList();

        int i = 1;
        foreach(var score in sortedScores)
        {
            GameObject s = Instantiate(ScorePrefab);
            s.transform.SetParent(ScoresContent.transform, false);
            s.GetComponentInChildren<TMP_Text>().text = i +  ". Score: " + score.Item1 + " ---  Time: " + score.Item2;
            i++;
        }
        
    }

    private void PopulateOnlineScores()
    {
        DestroyAllChildren(ScoresContent);

        if (Data.CloseOnlineScores == null)
        {
            Debug.Log("Online Scores Empty");
            Leaderboards.Instance.LoadRangeScores();

            GameObject s = Instantiate(ScoresLoadingPrefab);
            s.transform.SetParent(ScoresContent.transform, false);
            waitingForOnlineScores = true;
        }
        else if (Data.HasChanged)
        {
            Data.HasChanged = false;
            Data.CloseOnlineScores = null;
            Leaderboards.Instance.LoadRangeScores();

            GameObject s = Instantiate(ScoresLoadingPrefab);
            s.transform.SetParent(ScoresContent.transform, false);
            waitingForOnlineScores = true;
        }
        else
        {
            Debug.Log("Online Scores Not Empty");

            List<(string, double)> onlineScores = Data.CloseOnlineScores;
            var sortedScores = onlineScores.OrderByDescending(o => o.Item2).ToList();

            int i = 1;
            foreach (var score in sortedScores)
            {
                GameObject s = Instantiate(OnlineScorePrefab);
                s.transform.SetParent(ScoresContent.transform, false);
                s.transform.GetChild(0).GetComponent<TMP_Text>().text = score.Item1;
                s.transform.GetChild(1).GetComponent<TMP_Text>().text = i + ". SCORE: " + score.Item2;
                Debug.Log($"Comparing {score.Item1} to {Leaderboards.Instance.Username}");
                if (score.Item1 == Leaderboards.Instance.Username) { s.GetComponent<Image>().color = PlayerScoreColor; }
                i++;
            }
        }
        
    }

    private void PopulateBestScores()
    {
        DestroyAllChildren(ScoresContent);

        if (Data.BestOnlineScores == null)
        {
            Leaderboards.Instance.LoadTopScores();

            GameObject s = Instantiate(ScoresLoadingPrefab);
            s.transform.SetParent(ScoresContent.transform, false);
            waitingForBestScores = true;
        }
        else if (Data.HasChanged)
        {
            Data.HasChanged = false;
            Data.BestOnlineScores = null;
            Leaderboards.Instance.LoadTopScores();

            GameObject s = Instantiate(ScoresLoadingPrefab);
            s.transform.SetParent(ScoresContent.transform, false);
            waitingForBestScores = true;
        }
        else
        {
            List<(string, double)> bestScores = Data.BestOnlineScores;
            var sortedScores = bestScores.OrderByDescending(o => o.Item2).ToList();

            int i = 1;
            foreach (var score in sortedScores)
            {
                GameObject s = Instantiate(OnlineScorePrefab);
                s.transform.SetParent(ScoresContent.transform, false);
                s.transform.GetChild(0).GetComponent<TMP_Text>().text = score.Item1;
                s.transform.GetChild(1).GetComponent<TMP_Text>().text = i + ". SCORE: " + score.Item2;
                if (score.Item1 == Leaderboards.Instance.Username) { s.GetComponent<Image>().color = PlayerScoreColor; }
                i++;
            }
        }
    }
}
