using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Processors;

public class SpawnerController : MonoBehaviour
{
    //configurable
    [SerializeField] protected int maxEnemySpawn = 5;
    [SerializeField] protected GameObject enemyPrefab;
    [SerializeField] protected float spawnRadius=4f;
    [SerializeField] protected float spawnInterval = 3f;
    //values
    protected float spawnTimer = 0f;
    protected int currentNumberOfEnemySpawn = 0;

    protected Transform target;  //player
    protected Transform EnemySpawnPointsParent; //hierarchy parent
    protected Transform EnemyParent; //hierarchy parent
    //Event
    protected UnityAction<GameObject> onDeathCallback;

    protected SpriteRenderer _spriteRenderer;

    protected List<GameObject> spawned = new List<GameObject>();

    protected bool dead = false;

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
    public virtual void SpawnerDeath(GameObject dead_)
    {
        dead = true;
        Destroy(gameObject);
    }

    /// <summary>
    /// Update after a certain amount of time and if didnt run out of capacity spawns enemies
    /// </summary>
    void Update()
    {
        if(currentNumberOfEnemySpawn<maxEnemySpawn && !dead)
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
        Vector2 randomPoint;
        Vector3 spawnPosition = new Vector3();
        int maxAttempts = 10; 
        int attempts = 0;

        while (attempts < maxAttempts){
            randomPoint = Random.insideUnitCircle * spawnRadius;
            spawnPosition = new Vector3(randomPoint.x, randomPoint.y, 0) + transform.position;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, (spawnPosition - transform.position).normalized, Vector2.Distance(transform.position, spawnPosition));


            //lastValidSpawn = spawnPosition;
            //attemptedSpawns.Add(spawnPosition);

            if (hit.collider == null || hit.collider.gameObject == gameObject || hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Spawner"))
            {
                break;
            }

            attempts++;

            /*if(attempts>=maxAttempts)
            {
                Debug.Log(hit.collider.tag);
            }*/
        }


        if (attempts == maxAttempts)
        {
            Debug.LogWarning("Could not find a valid spawn position after multiple attempts.");
            //return;
        }

        //Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
        //Vector3 spawnPosition = new Vector3(randomPoint.x, randomPoint.y, 0) + transform.position;
        var newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        newEnemy.GetComponent<BasicEnemy>().Initialize(target, EnemySpawnPointsParent, EnemyParent, gameObject, onDeathCallback);

        newEnemy.transform.localScale = Vector3.zero;

        newEnemy.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);

        spawned.Add(newEnemy);
    }


    /*
    private List<Vector3> attemptedSpawns = new List<Vector3>();
    private Vector3 lastValidSpawn = Vector3.zero;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; // Yellow for all attempts
        foreach (Vector3 pos in attemptedSpawns)
        {
            Gizmos.DrawSphere(pos, 0.2f);
        }

        Gizmos.color = Color.green; // Green for successful spawns
        Gizmos.DrawSphere(lastValidSpawn, 0.3f);
    }

    */


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
