using TMPro;
using UnityEngine;

public class LeaderboardEntryUI : MonoBehaviour
{
    [SerializeField] private TMP_Text rankLabel;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private TMP_Text scoreLabel;

    public void SetValues(int rank, string name, int score)
    {
        rankLabel.text = rank.ToString();
        nameLabel.text = HighscoreManager.PostprocessName(name);
        scoreLabel.text = score.ToString();
    }

    public void SetColor(Color color)
    {
        rankLabel.color = color;
        nameLabel.color = color;
        scoreLabel.color = color;
    }
}
