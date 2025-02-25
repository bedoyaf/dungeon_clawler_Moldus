using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// handles UI of the upgrade card
/// </summary>
public class UpgradeUICardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<UpgradeSO> OnUpgradePicked; 
    [SerializeField] private TextMeshProUGUI UIname;
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    private UpgradeSO currentUpgrade;
    private void Start()
    {
        button.onClick.AddListener(OnCardPicked);
    }

    /// <summary>
    /// updates the visuals of the upgrade card
    /// </summary>
    /// <param name="upgrade">the upgrade to be shown</param>
    public void updateCard(UpgradeSO upgrade)
    {
        currentUpgrade = upgrade;
        UIname.text = upgrade.abilityName;
        image.sprite = upgrade.abilityIcon;
    }
    private void OnCardPicked()
    {
        OnUpgradePicked?.Invoke(currentUpgrade); 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(currentUpgrade != null)
        {
            UpgradeTooltipController.Instance.ShowTooltip(currentUpgrade.description);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UpgradeTooltipController.Instance.HideTooltip();
    }
}
