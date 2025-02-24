using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTrackerController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    public int depthAchieved { get; private set; } = 0;




    /// <summary>
    /// Just Increments the score
    /// </summary>
    public void IncreaseDepth()
    {
        depthAchieved++;
        UpdateScore();
    }
    /// <summary>
    /// Keeps UI up to date
    /// </summary>
    public void UpdateScore()
    {
        scoreText.text = "Depth: " + depthAchieved;
    }
    /// <summary>
    /// Resets the score
    /// </summary>
    public void ResetScore()
    {
        depthAchieved = 0;
        UpdateScore();
    }
    /// <summary>
    /// Loads from save
    /// </summary>
    public void LoadScore(SaveData saveData)
    {
        depthAchieved = saveData.DepthAchieved;
    }
}
