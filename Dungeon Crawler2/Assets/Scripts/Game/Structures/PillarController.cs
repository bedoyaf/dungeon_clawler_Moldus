using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


/// <summary>
/// Handles pillars
/// Those are immovable objects that block pathfinding and projectiles
/// </summary>
public class PillarController : MonoBehaviour
{

    [SerializeField] private List<Sprite> sprites;
    private SpriteRenderer spriteRenderer;
    private HealthController healthController;

    private DungeonContentGenerator dungeonContentGenerator;

    private int size_stage = 0;
    void Start()
    {
        healthController = GetComponent<HealthController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameObject.layer = LayerMask.NameToLayer("Environment");
    }

    void Update()
    {
        
    }


    /// <summary>
    /// Handles damage by adding visuals
    /// makes it change sprite upon losing certain health and shakes it
    /// </summary>
    public void OnDamage()
    {
        var originalPosition = transform.position;
        transform.DOShakePosition(0.2f, 0.1f, 7, 90)
        .OnComplete(() => transform.position = originalPosition);

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


    /// <summary>
    /// handles the pillars destruction by adding visuals and making sure it rescans path finding
    /// </summary>
    public void OnDeath()
    {
        spriteRenderer.DOKill();
        GetComponent<ShadowCaster2D>().enabled =false;
        spriteRenderer.DOFade(0f, 0.5f)
        .OnComplete(() =>
        {
        Destroy(gameObject); 
        dungeonContentGenerator.ScanSpecificAreaForPathfinding(transform.position, 10f);
        });
    }

    public void SetDungeonContentGenerator(DungeonContentGenerator _newDungeonContentGenerator)
    {
        dungeonContentGenerator = _newDungeonContentGenerator;
    }

}
