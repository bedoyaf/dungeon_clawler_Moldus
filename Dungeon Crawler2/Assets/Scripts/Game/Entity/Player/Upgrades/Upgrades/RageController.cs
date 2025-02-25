using UnityEngine;

/// <summary>
/// Activates damage modifiers for the player when health is low enaugh 
/// just checks values and activates it
/// </summary>
public class RageController : MonoBehaviour
{
    [SerializeField] private float damageModifierFromRage = 2.0f;
    [SerializeField] private float rageActivationPoint = 0.5f;

    private HealthController healthController;
    private PlayerShootingController shootingController;
    private bool rage = false;

    [SerializeField] private Color rageColor = new Color(1f, 0f, 0f, 0.2f); 
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        healthController = GetComponent<HealthController>();
        shootingController = GetComponent<PlayerShootingController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        if(healthController.currentHealth<=healthController.maxHealth*rageActivationPoint && !rage)
        {
            ActivateRage();
        }
        else if (healthController.currentHealth > healthController.maxHealth * rageActivationPoint && rage)
        {
            DeactivateRage();
        }
    }

    private void ActivateRage()
    {
        rage = true;
        shootingController.UpdateDamageModifiersRage(damageModifierFromRage);
    }
    private void DeactivateRage()
    {
        rage =false;
        shootingController.UpdateDamageModifiersRage(1f);
    }
}
