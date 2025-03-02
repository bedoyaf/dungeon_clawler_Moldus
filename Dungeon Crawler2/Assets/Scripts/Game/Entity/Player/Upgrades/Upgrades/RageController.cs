using System.Collections;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UI;


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

    private Image rageImage;

    void Start()
    {
        healthController = GetComponent<HealthController>();
        shootingController = GetComponent<PlayerShootingController>();
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
        TriggerRageFlash();
        rage = true;
        shootingController.UpdateDamageModifiersRage(damageModifierFromRage);
    }
    private void DeactivateRage()
    {
        rage =false;
        shootingController.UpdateDamageModifiersRage(1f);
    }


    public void TriggerRageFlash()
    {
        if (rageImage == null)
        {
            CreateRageImage();
        }
        StartCoroutine(FlashRoutine());
    }

    private void CreateRageImage()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("No Canvas found in the scene!");
            return;
        }

        GameObject rageImageGO = new GameObject("RageOverlay");
        rageImageGO.transform.SetParent(canvas.transform, false);

        rageImage = rageImageGO.AddComponent<Image>();
        rageImage.color = new Color(1, 0, 0, 0);

        RectTransform rt = rageImage.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    private IEnumerator FlashRoutine()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration / 2)
        {
            rageImage.color = new Color(1, 0, 0, Mathf.Lerp(0, 1, elapsedTime / (duration / 2)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        elapsedTime = 0f;
        while (elapsedTime < duration / 2)
        {
            rageImage.color = new Color(1, 0, 0, Mathf.Lerp(1, 0, elapsedTime / (duration / 2)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rageImage.color = new Color(1, 0, 0, 0);
    }

    public void DestroyRageImage()
    {
        Destroy(rageImage);
    }
}
