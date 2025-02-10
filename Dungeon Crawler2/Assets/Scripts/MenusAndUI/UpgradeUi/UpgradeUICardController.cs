using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeUICardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<UpgradeSO> OnUpgradePicked; 
    [SerializeField] private TextMeshProUGUI UIname;
    [SerializeField] private Image image;
    //[SerializeField] private string description="";
    [SerializeField] private Button button;

    private UpgradeSO currentUpgrade;
    private void Start()
    {
        button.onClick.AddListener(OnCardPicked);
    }

    public void updateCard(UpgradeSO upgrade)
    {
        currentUpgrade = upgrade;
        UIname.text = upgrade.abilityName;
        image.sprite = upgrade.abilityIcon;
        //description = upgrade.description;
        //name.text = upgrade.name;
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
