using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TMP_Text TimeText;

    private long startingTime;
    private float totalTime;

    public int totalSeconds;

    private void Start()
    {
        startingTime = 0;
    }

    private void Update()
    {
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

        //Debug.Log($"Frequency= 1 / ( 5 * {totalSeconds}) = {newFrequency}");
        ObstacleSpawner spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<ObstacleSpawner>();
        spawner.SpawnFrequency = newFrequency;

        float speedIncrease = 0.2f * Mathf.Log(totalSeconds);
        spawner.SpeedIncrease = speedIncrease;
    }
}
