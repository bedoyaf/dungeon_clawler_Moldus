using TMPro;
using UnityEngine;

/// <summary>
/// the tooltip that shows the upgrade description
/// </summary>
public class UpgradeTooltipController : MonoBehaviour
{
    public static UpgradeTooltipController Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI tooltipText;
    private RectTransform rectTransform;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false); 
    }

    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        rectTransform.position = mousePos + new Vector2(10, -10); 
    }

    public void ShowTooltip(string text)
    {
        tooltipText.text = text;
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
