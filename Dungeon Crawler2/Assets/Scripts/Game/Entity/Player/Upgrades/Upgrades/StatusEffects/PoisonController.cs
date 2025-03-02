using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PoisonController : StatusEffectAbstract
{
    [SerializeField] private int poisonDamage = 20;
    [SerializeField] private float poisonDuration = 3f;
    [SerializeField] private float tickInterval = 1f;

    private Coroutine poisonCoroutine;

    private HealthController healthController;
    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        color = new Color(0f, 1f, 0f, 1f);
    }

    public override void Apply(GameObject target)
    {
        healthController = target.GetComponent<HealthController>();
        spriteRenderer = target.GetComponent<SpriteRenderer>();
        //spriteRenderer.color = color;

        if (poisonCoroutine != null)
        {
            StopCoroutine(poisonCoroutine); // Restart poison if applied again
        }
        FlashWithTween();
        poisonCoroutine = StartCoroutine(DoPoisonDamage());
    }

    private IEnumerator DoPoisonDamage()
    {
        float elapsedTime = 0f;
        while (elapsedTime < poisonDuration)
        {
            HealthController health = GetComponent<HealthController>();
            if (health != null)
            {
                health.TakeDamage(poisonDamage);
            }
            elapsedTime += tickInterval;
            yield return new WaitForSeconds(tickInterval);
        }
        spriteRenderer.DOKill();
        Destroy(this);
    }

    private void FlashWithTween()
    {
        spriteRenderer.DOColor(color, 0.1f)
            .SetLoops(-1, LoopType.Yoyo) 
            .SetEase(Ease.Linear);    
    }
}
