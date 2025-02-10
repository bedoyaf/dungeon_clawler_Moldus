using DG.Tweening;
using UnityEngine;

public class RedBoss : BasicEnemy
{
    //weapons
    [SerializeField] GameObject bombPrefab;
    //configurable for bomb
//   [SerializeField] float bombPlantingDistance = 2f;
    // [SerializeField] float explosionCooldown = 2.5f; // Cooldown between bomb planting

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private BulletData bulletData;  //so its projectile is unique to the others

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

    public void setupRedBoss(Vector2 minteleport, Vector2 maxteleport)
    {
        teleportBoundsMin = minteleport;
        teleportBoundsMax = maxteleport;
    }


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
            .Append(transform.DOScale(1f, teleportDuration / 2).SetEase(Ease.OutBack))
            .OnComplete(() =>
            {
                teleportTween = null;
            })
            .OnKill(() =>
            {
                teleportTween = null;
            });
    }

    /// <summary>
    /// could be the same as Shoot, but decided to make it more exact
    /// </summary>
    public override void Attack()
    {
        ThrowBomb();
    }
    
    private void On_Death()
    {
        dead = true;
        teleportTween?.Kill();

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
