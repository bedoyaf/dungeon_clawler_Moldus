using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This class juist keeps the Health data on the UI up to date
/// </summary>
public class HealthTextController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] HealthController healthController;

    public void UpdateHealthText(int damage)
    {
        int startValue = int.Parse(textMesh.text.Replace("Health: ", ""));
        int targetValue = healthController.currentHealth;

        DOTween.To(() => startValue, x =>
        {
            startValue = x;
            textMesh.text = $"Health: {startValue}";
        }, targetValue, 0.5f) // Duration of 0.5 seconds
        .SetEase(Ease.OutQuad); // Smooth easing

    }
}
