using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMP_Text TimeText;

    private long startingTime;
    private float totalTime;

    public int totalSeconds;

    public bool GameOver = false;
    private bool GameOverUI = false;
    public bool paused = false;

    public GameObject GameOverWindow;
    public GameObject PauseWindow;

    private Player player;

    private CircleTransition circleTransition;

    private void Start()
    {
        SoundManager.Instance.PlayGameMusic();

        startingTime = 0;
        GameOverWindow.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        circleTransition = GameObject.FindGameObjectWithTag("Transition").GetComponent<CircleTransition>();
    }

    private void Update()
    {
        if (GameOver)
        {
            if (!GameOverUI)
            {
                GameOverWindow.SetActive(true);
                GameOverUI = true;

                GameOverWindow.transform.Find("Score").GetComponent<TMP_Text>().text = "SCORE: " + player.score;
                int sec = (int)totalSeconds % 60;
                int min = (int)totalSeconds / 60;

                string secText = sec.ToString();
                if (sec < 10) secText = "0" + secText;
                GameOverWindow.transform.Find("Time").GetComponent<TMP_Text>().text = "TIME: " + min+ ":" + secText; 
                GameOverWindow.transform.Find("Menu").GetComponent<Button>().onClick.AddListener(MenuListener);
                GameOverWindow.transform.Find("Retry").GetComponent<Button>().onClick.AddListener(RetryListener);
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            if (paused)
            {
                player.Pause();
                PauseWindow.SetActive(true);
                PauseWindow.transform.Find("Menu").GetComponent<Button>().onClick.AddListener(MenuListener);
                PauseWindow.transform.Find("Retry").GetComponent<Button>().onClick.AddListener(RetryListener);
                PauseWindow.transform.Find("Resume").GetComponent<Button>().onClick.AddListener(ResumeListener);
            }
            else
            {
                player.Unpause();
                PauseWindow.SetActive(false);
            }
        }

        if (paused)
        {
            return;
        }

        totalTime += Time.deltaTime;
        int seconds = (int)totalTime % 60;
        int minutes = (int)totalTime / 60;

        string secondsText = seconds.ToString();
        if (seconds < 10) secondsText = "0" + secondsText;

        TimeText.text = $"{minutes}:{secondsText}";

        totalSeconds = seconds + minutes * 60;
        SetDifficulty();
    }

    private void SetDifficulty()
    {
        if (totalSeconds < 20) return;

        float newFrequency = (1.0f / (5.0f * (float)totalSeconds)) * 100f;

        ObstacleSpawner spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<ObstacleSpawner>();
        spawner.SpawnFrequency = newFrequency;

        float speedIncrease = 0.2f * Mathf.Log(totalSeconds);
        spawner.SpeedIncrease = speedIncrease;
    }

    void MenuListener()
    {
        circleTransition.StartMenuTransition();
        //SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    void RetryListener()
    {
        circleTransition.StartGameTransition();
        //SceneManager.LoadScene("Mountain", LoadSceneMode.Single);
    }

    void ResumeListener()
    {
        player.Unpause();
        paused = false;
        PauseWindow.SetActive(false);
    }
}
