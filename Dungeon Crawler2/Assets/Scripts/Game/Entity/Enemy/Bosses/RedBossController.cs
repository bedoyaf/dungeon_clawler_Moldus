using DG.Tweening;
using UnityEngine;

/// <summary>
/// Extends the basic enemy but makes it shoot a new type of attack and teleport
/// </summary>
public class RedBoss : BasicEnemy
{
    [SerializeField] GameObject bombPrefab;

    [SerializeField] private float teleportDistance = 5f;
    [SerializeField] private Vector2 teleportBoundsMin;   
    [SerializeField] private Vector2 teleportBoundsMax;
    private float teleportDuration = 1f;

    private Tween teleportTween;
    private bool teleporting = false;

    void Start()
    {
        ConfigurateBasicFields();
        var enemycontroller = GetComponent<HealthController>();
        enemycontroller.onDeathEvent.AddListener(On_Death);
        StartCoroutine(EnemyBehavior());
        spriteFlipCustomizer = false;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, target.position) < teleportDistance && !teleporting)
        {
            Teleport();
        }
    }

    /// <summary>
    /// sets up the red boss
    /// by setting up the bounds for teleportation
    /// </summary>
    /// <param name="minteleport">min of bound for teleportation</param>
    /// <param name="maxteleport">max of bound for teleportation</param>
    public void setupRedBoss(Vector2 minteleport, Vector2 maxteleport)
    {
        teleportBoundsMin = minteleport;
        teleportBoundsMax = maxteleport;
    }

    /// <summary>
    /// changes the bosses location within the room and adds a tween visual
    /// </summary>
    private void Teleport()
    {
        teleporting = true;
        Vector2 randomPosition = new Vector2(
            Random.Range(teleportBoundsMin.x, teleportBoundsMax.x),
            Random.Range(teleportBoundsMin.y, teleportBoundsMax.y)
        );

        teleportTween = DOTween.Sequence()
            .Append(transform.DOScale(0f, teleportDuration / 2).SetEase(Ease.InBack))
            .AppendCallback(() =>
            {
                transform.position = randomPosition;
            })
            .Append(transform.DOScale(1.3f, teleportDuration / 2).SetEase(Ease.OutBack))
            .OnComplete(() =>
            {
                teleportTween = null;
                teleporting = false;
            })
            .OnKill(() =>
            {
                teleportTween = null;
                teleporting = false;
            });
    }

    /// <summary>
    /// could be the same as Shoot, but decided to make it more exact
    /// </summary>
    public override void Attack()
    {
        ThrowBomb();
    }

    /// <summary>
    /// Just makes sure the boss is destroyed safely
    /// </summary>
    private void On_Death()
    {
        dead = true;
        teleportTween?.Kill();
        GetComponent<SpriteRenderer>().DOKill();
        Destroy(spawnLocation);

        Sequence deathSequence = DOTween.Sequence();
        deathSequence.Append(transform.DOScale(0f, 0.5f).SetEase(Ease.InBack));
        deathSequence.Join(transform.DORotate(new Vector3(0f, 0f, 360f), 0.5f, RotateMode.FastBeyond360));
        deathSequence.OnComplete(() => Destroy(gameObject));
    }
    
    private void ThrowBomb()
    {
        var bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        bomb.GetComponent<BossBombController>().flyTowards(target.position);
    }
}
