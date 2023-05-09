using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Leaderboards;
using Newtonsoft.Json;
using System.Linq;

public class Leaderboards : MonoBehaviour
{
    public static Leaderboards Instance;

    private const string LeaderboardId = "Leaderboard_1";

    private async void Awake()
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

        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    AddScore(15);
        //}
        //if (Input.GetKey(KeyCode.Alpha1))
        //{
        //    GetScores();
        //}
    }

    public async void LoadTopScores()
    {
        var offset = 0;
        var limit = 10;
        var scoresResponse = await LeaderboardsService.Instance
            .GetScoresAsync(LeaderboardId, new GetScoresOptions { Offset = offset, Limit = limit });
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
        if (Data.BestOnlineScores == null) Data.BestOnlineScores = new List<(string, double)>();

        var results = scoresResponse.Results;
        for (int i = 0; i < results.Count; i++)
        {
            Debug.Log("RESULT " + i + ": => " + results[i].PlayerName);
            Data.BestOnlineScores.Add((results[i].PlayerName, results[i].Score));
        }
            
    }

    public async void LoadRangeScores()
    {
        var rangeLimit = 5;
        var scoresResponse = await LeaderboardsService.Instance
            .GetPlayerRangeAsync(LeaderboardId,
                new GetPlayerRangeOptions { RangeLimit = rangeLimit }
            );
    }

    public async void AddScore(double score)
    {
        var playerEntry = await LeaderboardsService.Instance
        .AddPlayerScoreAsync(LeaderboardId, score);
        Debug.Log(JsonConvert.SerializeObject(playerEntry));
    }

    public async void GetScores()
    {
        var scoresResponse = await LeaderboardsService.Instance
            .GetScoresAsync(LeaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPaginatedScores()
    {
        var offset = 10;
        var limit = 10;
        var scoresResponse = await LeaderboardsService.Instance
            .GetScoresAsync(LeaderboardId, new GetScoresOptions { Offset = offset, Limit = limit });
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPlayerRange()
    {
        // Returns a total of 11 entries (the given player plus 5 on either side)
        var rangeLimit = 5;
        var scoresResponse = await LeaderboardsService.Instance
            .GetPlayerRangeAsync(LeaderboardId,
                new GetPlayerRangeOptions { RangeLimit = rangeLimit }
            );
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }

    public async void GetPlayerScore()
    {
        var scoreResponse = await LeaderboardsService.Instance
            .GetPlayerScoreAsync(LeaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }
}
