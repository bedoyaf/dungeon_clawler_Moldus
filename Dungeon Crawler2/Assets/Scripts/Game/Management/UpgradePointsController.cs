using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the number of points needed for upgrade
/// </summary>
public class UpgradePointsController : MonoBehaviour
{

    [SerializeField] public int currentPoints = 0;
    [SerializeField] public int pointsNeededForUpgrade = 6;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private UpgradeManager upgradeManager;

    private int formulaIndex = 0;
    private int startingValueForFormula;

    private bool isFlashing = false;
    private Tween flashTween;

    void Start()
    {
        startingValueForFormula = pointsNeededForUpgrade;
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && currentPoints >= pointsNeededForUpgrade && isFlashing)
        {
            StopFlashing();
            OnReachingEnaughPoints();
        }
    }

    /// <summary>
    /// Adds one point upon destroying spawner
    /// </summary>
    /// <param name="spawner">destroyed spawner</param>
    public void IncrementUpgradePoints(GameObject spawner)
    {
        if (currentPoints < pointsNeededForUpgrade)
        {
            currentPoints++;
        }
        if (currentPoints >= pointsNeededForUpgrade)
        {
            FlashUI();
        }
        UpdateUI();
    }

    /// <summary>
    /// automaticly gets all the points
    /// </summary>
    public void fillPointCounter()
    {
        currentPoints = pointsNeededForUpgrade;
        FlashUI();
        UpdateUI();
    }

    /// <summary>
    /// Adds a flashing to the UI, so the player can see that he has enaugh points
    /// </summary>
    public void FlashUI()
    {
        if (isFlashing) return;  
        isFlashing = true;

        flashTween = textMesh
            .DOFade(0.2f, 0.5f)  
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
    }

    /// <summary>
    /// Stops the visual of flashing
    /// </summary>
    private void StopFlashing()
    {
        isFlashing = false;
        flashTween.Kill(); 
        textMesh.alpha = 1f; 
    }

    /// <summary>
    /// Handles when enaugh points have been collected
    /// </summary>
    private void OnReachingEnaughPoints()
    {
        currentPoints = 0;
        pointsNeededForUpgrade = IncreasePointsNeededForUpgrade();
        upgradeManager.chooseAndDisplayNewUpgrades();
        UpdateUI();
    }

    /// <summary>
    /// Formula point increase
    /// </summary>
    /// <returns>the new value</returns>
    private int IncreasePointsNeededForUpgrade()
    {
        formulaIndex++;
        return Mathf.RoundToInt(startingValueForFormula + 1 * Mathf.Log(formulaIndex + 1, 2));
    }

    private void UpdateUI()
    {
        textMesh.text = $"Spores: {currentPoints}/{pointsNeededForUpgrade}";
    }

    public void ResetPoints()
    {
        StopFlashing();
        currentPoints = 0;
        pointsNeededForUpgrade = startingValueForFormula;
        UpdateUI();
    }

    public void LoadPoints(SaveData saveData)
    {
        currentPoints = saveData.upgradePoints;
        pointsNeededForUpgrade = saveData.pointsNeeded;
        if(currentPoints>=pointsNeededForUpgrade)
        {
            FlashUI();
        }
        UpdateUI();
    }
}
