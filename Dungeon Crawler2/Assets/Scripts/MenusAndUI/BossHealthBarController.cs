using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Handles the bosses health bar UI
/// </summary>
public class BossHealthBarController : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider whiteHealthBar;
    private HealthController healthController;

    private int maxHealth = 100;

    void Start()
    {
        healthBar = GetComponent<Slider>();
    }
    /// <summary>
    /// Updates it with a new healthController from the new boss
    /// </summary>
    /// <param name="newhealthController">bosses health controller</param>
    public void setupHelathController(HealthController newhealthController)
    {
        healthBar = GetComponent<Slider>();
        healthController = newhealthController;
        healthController.onTakeDamage.AddListener(UpdateHealthBar);
        SetMaxHealth(healthController.maxHealth);
        healthBar.value = maxHealth;
        whiteHealthBar.value = maxHealth;
    }

    /// <summary>
    /// updates the max health from the boss
    /// </summary>
    /// <param name="maxHealth_">the bosses maxhealth</param>
    public void SetMaxHealth(int maxHealth_)
    {
        maxHealth = maxHealth_;
        healthBar.maxValue = maxHealth_;
        whiteHealthBar.maxValue = maxHealth_;
    }

    /// <summary>
    /// Updates the health bar UI with visual tween
    /// </summary>
    /// <param name="damage">damage received</param>
    public void UpdateHealthBar(int damage)
    {
        healthBar.DOKill();
        whiteHealthBar.DOKill();
        if (healthBar != null)
        {
            //healthBar.value = healthController.currentHealth;
            float targetValue = healthController.currentHealth;
            healthBar.DOValue(targetValue, 0.5f).SetEase(Ease.OutQuad);


            whiteHealthBar.DOValue(targetValue, 1.5f).SetEase(Ease.OutQuad).SetDelay(0.5f);
        }
    }
}
