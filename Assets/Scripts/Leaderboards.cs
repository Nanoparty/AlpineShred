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
    public string Username = "";
    public string PlayerId = "";

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

        if (AuthenticationService.Instance.IsAuthorized) return;

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
        PlayerId = AuthenticationService.Instance.PlayerId;
        //Debug.Log(JsonConvert.SerializeObject(playerInfoResponse));

        //GetPlayerInfoAsync();

        //await AuthenticationService.Instance.UpdatePlayerNameAsync("bill");

        //Username = await AuthenticationService.Instance.GetPlayerNameAsync();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            GetScores();
        }
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
            if (results[i].PlayerId == PlayerId) Username = results[i].PlayerName;
            Debug.Log("RESULT " + i + ": => " + results[i].PlayerName);
            Data.BestOnlineScores.Add((results[i].PlayerName, results[i].Score));
        }
            
    }

    public async void LoadRangeScores()
    {
        //Debug.Log("Loading Online Scores");
        //Data.CloseOnlineScores = new List<(string, double)>();
        var rangeLimit = 5;
        var scoresResponse = await LeaderboardsService.Instance
            .GetPlayerRangeAsync(LeaderboardId,
                new GetPlayerRangeOptions { RangeLimit = rangeLimit }
            );

        if (Data.CloseOnlineScores == null) Data.CloseOnlineScores = new List<(string, double)>();

        var results = scoresResponse.Results;
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].PlayerId == PlayerId) Username = results[i].PlayerName;
            Debug.Log("RESULT " + i + ": => " + results[i].PlayerName);
            Data.CloseOnlineScores.Add((results[i].PlayerName, results[i].Score));
        }
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
