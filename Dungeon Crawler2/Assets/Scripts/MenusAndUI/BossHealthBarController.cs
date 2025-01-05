using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
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

    public void setupHelathController(HealthController newhealthController)
    {
        healthBar = GetComponent<Slider>();
        healthController = newhealthController;
        healthController.onTakeDamage.AddListener(UpdateHealthBar);
        SetMaxHealth(healthController.maxHealth);
        healthBar.value = maxHealth;
        whiteHealthBar.value = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void SetMaxHealth(int maxHealth_)
    {
        maxHealth = maxHealth_;
        healthBar.maxValue = maxHealth_;
        whiteHealthBar.maxValue = maxHealth_;
    }
    public void UpdateHealthBar(int damage)
    {
        if (healthBar != null)
        {
            healthBar.value = healthController.currentHealth;
            float targetValue = healthController.currentHealth;
            healthBar.DOValue(targetValue, 0.5f).SetEase(Ease.OutQuad);


            whiteHealthBar.DOValue(targetValue, 1.5f).SetEase(Ease.OutQuad).SetDelay(0.5f);
        }
    }
}
