using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PillarController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private List<Sprite> sprites;
    private SpriteRenderer spriteRenderer;
    private HealthController healthController;

    private int size_stage = 0;
    void Start()
    {
        healthController = GetComponent<HealthController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameObject.layer = LayerMask.NameToLayer("Environment");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnDamage()
    {
        transform.DOShakePosition(0.2f, 0.2f, 10, 90);
        if (healthController.currentHealth<(healthController.maxHealth*2)/3 && size_stage == 0)
        {
            size_stage = 1;
            spriteRenderer.sprite = sprites[1];
        }
        else if(healthController.currentHealth < (healthController.maxHealth * 1) / 3 && size_stage == 1)
        {
            size_stage = 2;
            spriteRenderer.sprite = sprites[2];
        }
    }

    public void OnDeath()
    {
        spriteRenderer.DOFade(0f, 0.5f)
        .OnComplete(() => Destroy(gameObject));
    }
}
