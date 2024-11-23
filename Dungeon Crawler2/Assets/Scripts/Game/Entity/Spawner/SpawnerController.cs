using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnerController : MonoBehaviour
{
    //configurable
    [SerializeField] private int maxEnemySpawn = 5;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRadius=2f;
    [SerializeField] private float spawnInterval = 3f;
    //values
    private float spawnTimer = 0f;
    private int currentNumberOfEnemySpawn = 0;

    private Transform target;  //player
    private Transform EnemySpawnPointsParent; //hierarchy parent
    private Transform EnemyParent; //hierarchy parent
    //Event
    private UnityAction<GameObject> onDeathCallback;

    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        var enemycontroller = GetComponent<HealthController>();
        enemycontroller.onDeathEvent.AddListener(SpawnerDeath);
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Initializes the Spawner
    /// </summary>
    /// <param name="_enemyPrefab">prefab of the enemy, will change acording to the type of spawner</param>
    /// <param name="_target">target of the enemies, player</param>
    /// <param name="_enemySpawnPointsParent">the hierarchy parent of the enemy spawn points</param>
    /// <param name="_EnemyParent">the hierarchy parent of the enemy</param>
    ///  /// <param name="_onDeathCallback">Unity Event that gives points upon killing an enemy</param>
    public void Initialize(GameObject _enemyPrefab,Transform _target, Transform _enemySpawnPointsParent, Transform _EnemyParent, UnityAction<GameObject> _onDeathCallback)
    {
        enemyPrefab = _enemyPrefab;
        target = _target;
        EnemySpawnPointsParent = _enemySpawnPointsParent;
        EnemyParent = _EnemyParent;
        onDeathCallback = _onDeathCallback;
    }

    /// <summary>
    /// Just destroys itself
    /// </summary>
    public virtual void SpawnerDeath(GameObject dead)
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Update after a certain amount of time and if didnt run out of capacity spawns enemies
    /// </summary>
    void Update()
    {
        if(currentNumberOfEnemySpawn<maxEnemySpawn)
        {
            spawnTimer += Time.deltaTime; 

            if (spawnTimer >= spawnInterval)
            {
                currentNumberOfEnemySpawn++;
                SpawnEnemy();
                spawnTimer = 0f; 
            }
        }
    }

    /// <summary>
    /// spawn and configurates a new enemy
    /// </summary>
    private void SpawnEnemy()
    {
        Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomPoint.x, randomPoint.y, 0) + transform.position;
        var newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        newEnemy.GetComponent<BasicEnemy>().Initialize(target, EnemySpawnPointsParent, EnemyParent, gameObject, onDeathCallback);
    }


    /// <summary>
    /// decreases the number of current enemies, so it can spawn new ones
    /// </summary>
    public void OneOfOurSpawnedEnemiesDies()
    {
        currentNumberOfEnemySpawn--;
    }
    /*
    public void FlashRed()
    {
        if (DOTween.IsTweening(_spriteRenderer))
        {
            return;
        }

        Color originalColor = _spriteRenderer.color;
        Color changeColor = new Color(1f, 0f, 0f, 0.5f);
        _spriteRenderer.DOColor(changeColor, 0.1f).OnComplete(() =>
        {
            _spriteRenderer.DOColor(originalColor, 0.1f);
        });
    }
    */
}
