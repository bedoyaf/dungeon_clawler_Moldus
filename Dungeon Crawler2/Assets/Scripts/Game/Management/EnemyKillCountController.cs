using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyKillCountController : MonoBehaviour
{
    //enemyTypes
    [SerializeField] GameObject enemyTypeRed;
    [SerializeField] GameObject enemyTypePurple;
    [SerializeField] GameObject enemyTypeGreen;
    //Points text
    [SerializeField] TextMeshProUGUI textRedPoints;
    [SerializeField] TextMeshProUGUI textPurplePoints;
    [SerializeField] TextMeshProUGUI textGreenPoints;
    //Points
    [SerializeField] public int RedPoints  = 0;
    [SerializeField] public int PurplePoints  = 0;
    [SerializeField] public int GreenPoints  = 0;

    void Start()
    {
        enemyTypeRed.GetComponent<HealthController>().onDeathEvent.AddListener(OnEnemyDeath);
        enemyTypeGreen.GetComponent<HealthController>().onDeathEvent.AddListener(OnEnemyDeath);
        enemyTypePurple.GetComponent<HealthController>().onDeathEvent.AddListener(OnEnemyDeath);
    }

    /// <summary>
    /// Loads the saved points
    /// </summary>
    public void LoadPoints(SaveData saveData)
    {
        RedPoints = saveData.RedPoints;
        GreenPoints = saveData.GreenPoints;
        PurplePoints = saveData.PurplePoints;
        UpdateTextPoints();
    }
    /// <summary>
    /// Increments the points if an enemy died
    /// </summary>
    public void OnEnemyDeath(GameObject enemy)
    {
        ColorEnemy enemyType = enemy.GetComponent<BasicEnemy>().colorOfEnemy;
        switch (enemyType)
        {
            case ColorEnemy.Purple:
                PurplePoints++;
                break;
            case ColorEnemy.Green:
                GreenPoints++;
                break;
            case ColorEnemy.Red:
                RedPoints++;
                break;
        }
        UpdateTextPoints();
    }
    /// <summary>
    /// Just resets the points
    /// </summary>
    public void ResetPoints()
    {
        RedPoints= 0;
        GreenPoints= 0;
        PurplePoints= 0;
        UpdateTextPoints();
    }

    /// <summary>
    /// Texts care of the UI text being up to date
    /// </summary>
    public void UpdateTextPoints()
    {
        textRedPoints.text = "R: " + RedPoints;
        textGreenPoints.text = "G: " + GreenPoints;
        textPurplePoints.text = "P: " + PurplePoints;
    }
}
