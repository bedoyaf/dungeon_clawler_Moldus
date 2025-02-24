using DG.Tweening;
using TMPro;
using UnityEngine;

public class UpgradePointsController : MonoBehaviour
{

    [SerializeField] public int currentPoints = 0;
    [SerializeField] private int pointsNeededForUpgrade = 6;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private UpgradeManager upgradeManager;

    private int formulaIndex = 0;
    private int startingValueForFormula;

    private bool isFlashing = false;
    private Tween flashTween;

    void Start()
    {
        startingValueForFormula = pointsNeededForUpgrade;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && currentPoints >= pointsNeededForUpgrade && isFlashing)
        {
            StopFlashing();
            OnReachingEnaughPoints();
        }
    }

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

    public void FlashUI()
    {
        if (isFlashing) return;  
        isFlashing = true;

        flashTween = textMesh
            .DOFade(0.2f, 0.5f)  
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
    }
    private void StopFlashing()
    {
        isFlashing = false;
        flashTween.Kill(); 
        textMesh.alpha = 1f; 
    }

    private void OnReachingEnaughPoints()
    {
        currentPoints = 0;
        pointsNeededForUpgrade = IncreasePointsNeededForUpgrade();
        upgradeManager.chooseAndDisplayNewUpgrades();
        UpdateUI();
    }

    private int IncreasePointsNeededForUpgrade()
    {
        formulaIndex++;
        return Mathf.RoundToInt(startingValueForFormula + 4 * Mathf.Log(formulaIndex + 1, 2));
    }

    private void UpdateUI()
    {
        textMesh.text = $"Spores: {currentPoints}/{pointsNeededForUpgrade}";
    }

}
