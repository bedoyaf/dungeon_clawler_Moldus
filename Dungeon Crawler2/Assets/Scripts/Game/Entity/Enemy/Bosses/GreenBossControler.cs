/*using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class GreenBossControler : BasicEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private BulletData bulletData; //so its projectile is unique to the others

    [SerializeField] private GameObject minionprefab;
    [SerializeField] private Vector2 spawnBoundsMin;
    [SerializeField] private Vector2 spawnBoundsMax;

    private UnityAction<GameObject> onDeathCallback;

    void Start()
    {
        ConfigurateBasicFields();
        var enemycontroller = GetComponent<HealthController>();
        enemycontroller.onDeathEvent.AddListener(On_Death);
        StartCoroutine(EnemyBehavior());
        spriteFlipCustomizer = false;
        colorOfEnemy = ColorEnemy.Green;
    }

    public override void Attack()
    {
        Shoot();
    }
    /// <summary>
    /// spawns the bulletPrefab and configurates it
    /// </summary>
    private void Shoot()
    {
        Vector2 shootDirection = (target.position - transform.position).normalized;
        float bulletSpeed = _rigidBody.linearVelocity.magnitude;
        Vector3 currentPosition = transform.position;
        currentPosition.z = 0;
        GameObject bullet = Instantiate(bulletPrefab, currentPosition, Quaternion.identity);
        bullet.GetComponent<BulletController>().setBulletData(bulletData);
        bullet.GetComponent<BulletController>().Initialize(gameObject, shootDirection, currentPosition, bulletSpeed);
    }


    public void setupGreenBoss(Vector2 minteleport, Vector2 maxteleport)
    {
        spawnBoundsMin = minteleport;
        spawnBoundsMax = maxteleport;
    }

    private void SpawnEnemy()
    {
        var minionSpawnPoints= new GameObject("MinionSpawnPoints");
        minionSpawnPoints.transform.SetParent(transform);

        float randomX = Random.Range(spawnBoundsMin.x, spawnBoundsMax.x);
        float randomY = Random.Range(spawnBoundsMin.y, spawnBoundsMax.y);
        Vector2 spawnPosition = new Vector2(randomX, randomY);
        var newEnemy = Instantiate(minionprefab, spawnPosition, Quaternion.identity);
        newEnemy.GetComponent<BasicEnemy>().Initialize(target, minionSpawnPoints.transform, transform, gameObject, onDeathCallback);

        newEnemy.transform.localScale = Vector3.zero;

        newEnemy.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
    }




}
*/