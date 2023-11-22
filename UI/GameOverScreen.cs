using System.Collections.Generic;
using TMPro;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

public class GameOverScreen : UIScreen
{
    [SerializeField] private float delay = 1.5f;
    [SerializeField] private TMP_Text scoreLabel;
    [SerializeField] private Transform highscoresContainer;
    [SerializeField] private Transform playerRangeContainer;
    [SerializeField] private LeaderboardEntryUI leaderboardEntryUIPrefab;
    [SerializeField] private Color playerColor = UIPalette.yellowDark;
    [SerializeField] private AudioClip winClip;
    [SerializeField][Range(0, 1)] private float winVolume = 1;
    [SerializeField] private AudioClip loseClip;
    [SerializeField][Range(0, 1)] private float loseVolume = 1;

    private void Awake()
    {
        Clear();
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        HighscoreManager.OnHighscoresUpdated += HandleHighscoresUpdated;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
        HighscoreManager.OnHighscoresUpdated -= HandleHighscoresUpdated;
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state.state == GameState.State.Lost) Invoke("SetActive", delay);
        else SetActive(false);
    }

    private void SetActive()
    {
        SetActive(true);
    }

    private void HandleHighscoresUpdated(LeaderboardEntry previousEntry, LeaderboardEntry newEntry, List<LeaderboardEntry> highscores, List<LeaderboardEntry> playerRange)
    {
        Clear();

        if (previousEntry == null || newEntry.Score > previousEntry.Score)
        {
            SFXPlayer.PlayOneshot(winClip, winVolume);
        }
        else
        {
            SFXPlayer.PlayOneshot(loseClip, loseVolume);
        }

        scoreLabel.text = newEntry.Score.ToString();

        foreach (LeaderboardEntry entry in highscores)
        {
            CreateEntryUI(entry, newEntry.PlayerId, highscoresContainer);
        }

        foreach (LeaderboardEntry entry in playerRange)
        {
            CreateEntryUI(entry, newEntry.PlayerId, playerRangeContainer);
        }
    }

    private void CreateEntryUI(LeaderboardEntry entry, string playerId, Transform container)
    {
        LeaderboardEntryUI ui = Instantiate(leaderboardEntryUIPrefab, container);
        ui.SetValues(entry.Rank + 1, entry.PlayerName, (int)entry.Score);

        if (entry.PlayerId == playerId)
        {
            ui.SetColor(playerColor);
        }
    }

    private void Clear()
    {
        foreach (LeaderboardEntryUI ui in highscoresContainer.GetComponentsInChildren<LeaderboardEntryUI>())
        {
            Destroy(ui.gameObject);
        }

        foreach (LeaderboardEntryUI ui in playerRangeContainer.GetComponentsInChildren<LeaderboardEntryUI>())
        {
            Destroy(ui.gameObject);
        }
    }
}
