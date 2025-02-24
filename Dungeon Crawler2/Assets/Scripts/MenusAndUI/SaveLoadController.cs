using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The class that will be serielized
/// </summary>
[System.Serializable]
public class SaveData
{
    public int Health;
    public int DepthAchieved;
    public int RedPoints;
    public int GreenPoints;
    public int PurplePoints;
    public List<UpgradeSO> unlockedUpgrades;
    public SaveData(int currentHealth, int depthAchieved, int redPoints, int greenPoints, int purplePoints, List<UpgradeSO> unlockedUpgrades)
    {
        Health = currentHealth;
        DepthAchieved = depthAchieved;
        RedPoints = redPoints;
        GreenPoints = greenPoints;
        PurplePoints = purplePoints;
        this.unlockedUpgrades = unlockedUpgrades;
    }
}

public class SaveLoadController : MonoBehaviour
{
    private ScoreTrackerController scoreTrackerController;
    private EnemyKillCountController enemyKillCountController;
    [SerializeField] private HealthController healthController;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] public UnityEvent<SaveData> Load;
    [SerializeField] string fileName = "saveData.json";
    private void Start()
    {
        scoreTrackerController = GetComponent<ScoreTrackerController>();
        enemyKillCountController = GetComponent<EnemyKillCountController>();
    }
    /// <summary>
    /// Saves the current points, score and health into a json
    /// </summary>
    public void SaveGame()
    {
        SaveData saveData = new SaveData(healthController.currentHealth, scoreTrackerController.depthAchieved, enemyKillCountController.RedPoints, enemyKillCountController.GreenPoints, enemyKillCountController.PurplePoints, upgradeManager.activatedUpgrades);
        string json = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + fileName, json);
    }
    /// <summary>
    /// loads the data from the json and calss the load event with data so it rewrites
    /// </summary>
    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/" + fileName;
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Load.Invoke(data);
        }
    }
}