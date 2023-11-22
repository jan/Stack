using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using System.Text.RegularExpressions;

public class HighscoreManager : MonoBehaviour
{
    public static event Action<string> OnSignedIn = (name) => { };
    public static event Action<LeaderboardEntry, LeaderboardEntry, List<LeaderboardEntry>, List<LeaderboardEntry>> OnHighscoresUpdated = (previousEntry, newEntry, highscores, playerRange) => { };

    [SerializeField] private string leaderboardId = "highscores";
    [SerializeField] private int highscoresLimit = 5;
    [SerializeField] private int playerRangeLimit = 2;

    public static string PlayerName { get; private set; }

    private async void Awake()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += HandleSignedIn;
        AuthenticationService.Instance.SignInFailed += HandleSignInFailed;
        AuthenticationService.Instance.SignedOut += HandleSignOut;
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        AuthenticationService.Instance.SignedIn -= HandleSignedIn;
        AuthenticationService.Instance.SignInFailed -= HandleSignInFailed;
        AuthenticationService.Instance.SignedOut -= HandleSignOut;
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private async void HandleSignedIn()
    {
        PlayerName = PostprocessName(await AuthenticationService.Instance.GetPlayerNameAsync(true));
        Debug.Log("HandleSignedIn " + PlayerName);
        OnSignedIn(PlayerName);
    }

    private void HandleSignInFailed(RequestFailedException ex)
    {
        Debug.LogError(ex);
    }

    private void HandleSignOut()
    {
        Debug.Log("HandleSignOut");
    }

    private async void HandleGameStateChanged(GameState state)
    {
        if (state.state != GameState.State.Lost) return;

        LeaderboardEntry previousEntry;
        try
        {
            previousEntry = await LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardId);
        }
        catch (Exception ex)
        {
            previousEntry = null;
        }
        LeaderboardEntry newEntry = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, state.score);
        LeaderboardScoresPage highscores = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId, new GetScoresOptions { Limit = highscoresLimit });
        LeaderboardScores playerRange = await LeaderboardsService.Instance.GetPlayerRangeAsync(leaderboardId, new GetPlayerRangeOptions { RangeLimit = playerRangeLimit });

        OnHighscoresUpdated(previousEntry, newEntry, highscores.Results, playerRange.Results);
    }

    public static async Task<string> SetPlayerName(string name)
    {
        Debug.Log("SetPlayerName " + name);

        string preprocessedName = PreprocessPlayerName(name);
        string remoteName = await AuthenticationService.Instance.UpdatePlayerNameAsync(preprocessedName);
        PlayerName = PostprocessName(remoteName);
        return PlayerName;
    }

    public static bool IsNameValid(string name)
    {
        return name.Length > 0;
    }

    private static string PreprocessPlayerName(string name)
    {
        return Regex.Replace(name, "\\s", "");
    }

    public static string PostprocessName(string name)
    {
        return Regex.Replace(name, "#\\d+$", "");
    }
}
