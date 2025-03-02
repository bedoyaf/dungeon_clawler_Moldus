using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using DG.Tweening;
using System;

/// <summary>
/// class that sets up functions that are universal for every enemy type, mainly pathfinding
/// </summary>
public abstract class BasicEnemy : MonoBehaviour, IEnemy
{
    //path finding
    [SerializeField]
    protected Transform target;
    [SerializeField]
    protected float EnemyVision = 20f;
    [SerializeField]
    protected int movementSpeed = 5;
    protected AIDestinationSetter destinationSetter;
    protected AIPath aiPath;
    //helpers
    protected SpriteRenderer _spriteRenderer;
    protected GameObject spawnLocation;
    protected bool spriteFlipCustomizer = true;
    protected Rigidbody2D _rigidBody;
    private GameObject spawner;
    [SerializeField] protected Transform spawnLocationsParent, enemyParent;
    //states
    public ColorEnemy colorOfEnemy { get; protected set; }
    protected EnemyState currentState = EnemyState.Idle;

    //attacking
    bool isShooting = false;
    [SerializeField] protected float shootingRange, shootingCooldown;
    protected float shootTimer;

    protected bool dead = false;

    void Start()
    {
        ConfigurateBasicFields();
        StartCoroutine(EnemyBehavior());
        setupEnemyDeathEvents();
    }

    protected virtual void ConfigurateBasicFields()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        _rigidBody = GetComponent<Rigidbody2D>();
        destinationSetter.target = target;
       // var enemycontroller = GetComponent<HealthController>();
       // enemycontroller.onDeathEvent.AddListener(OnBasicEnemyDeathSpawnerUpdate);
       // enemycontroller.onDeathEvent.AddListener(On_Death);
    }

    protected void setupEnemyDeathEvents()
    {
        var enemycontroller = GetComponent<HealthController>();
        enemycontroller.onDeathEvent.AddListener(OnBasicEnemyDeathSpawnerUpdate);
        enemycontroller.onDeathEvent.AddListener(On_Death);
    }

    /// <summary>
    /// Initializes the Enemy
    /// </summary>
    /// <param name="newTarget">The enemy target, in this case the player</param>
    /// <param name="spawnLocsParent">the objects whose parent will be in the hierarchie the spawn point</param>
    /// <param name="newEnemyParent">the objects whose parent will be in the hierarchie the enemy</param>
    /// <param name="_mySpawner">The spawner that spawned this enemy</param>
    /// <param name="inCrementationOfPoints">a listener that increments upon the enemys death the rgp points</param>
    public void Initialize(Transform newTarget, Transform spawnLocsParent, Transform newEnemyParent, GameObject _mySpawner, UnityAction<GameObject> inCrementationOfPoints)
    {
        setTarget(newTarget);
        setSpawnLocationsParent(spawnLocsParent);
        setEnemyParent(newEnemyParent);
        if(inCrementationOfPoints != null)
        {
            GetComponent<HealthController>().onDeathEvent.AddListener(inCrementationOfPoints);
        }
        spawner =_mySpawner;
    }

    /// <summary>
    /// Initializes the Enemy
    /// </summary>
    /// <param name="newTarget">The enemy target, in this case the player</param>
    /// <param name="spawnLocsParent">the objects whose parent will be in the hierarchie the spawn point</param>
    /// <param name="newEnemyParent">the objects whose parent will be in the hierarchie the enemy</param>
    /// <param name="_mySpawner">The spawner that spawned this enemy</param>
    /// <param name="inCrementationOfPoints">a listener that increments upon the enemys death the rgp points</param>
    public void Initialize(Transform newTarget, Transform spawnLocsParent, Transform newEnemyParent, GameObject _mySpawner)
    {
        setTarget(newTarget);
        setSpawnLocationsParent(spawnLocsParent);
        setEnemyParent(newEnemyParent);
        spawner = _mySpawner;
    }

    private void setTarget(Transform newTarget)
    {
        target = newTarget;
    }
    /// <summary>
    /// Just sets up the spawn location to be in the hierarchy under the parent
    /// </summary>
    private void setSpawnLocationsParent(Transform spawnLocsparent)
    {
        spawnLocationsParent = spawnLocsparent;
        spawnLocation = new GameObject(gameObject.name + "_SpawnLocation");
        spawnLocation.transform.position = transform.position;
        spawnLocation.transform.parent = spawnLocationsParent;
    }
    /// <summary>
    /// Just sets up the enemy to be in the hierarchy under the parent
    /// </summary>
    private void setEnemyParent(Transform newEnemyParent)
    {
        enemyParent = newEnemyParent;
        gameObject.transform.parent = enemyParent;
    }

    /// <summary>
    /// Enemy death that makes sure it destroys everithing and lets the spawner know it can spawn more enemies
    /// </summary>
   /* public virtual void EnemyDeath(GameObject dead)
    {
        if(spawner!= null)
        {
            spawner.GetComponent<SpawnerController>().OneOfOurSpawnedEnemiesDies();
        }
        Destroy(spawnLocation);

        Destroy(gameObject);
    }*/

    public abstract void Attack();
    /// <summary>
    /// Currently empty, might add strolling around or certain animations
    /// </summary>
    protected void Idle()
    {

    }

    /// <summary>
    /// Checks if the player is close enaugh to pursue if yes, then it checks if it has a line of sight, if yes then pursues
    /// </summary>
    protected virtual IEnumerator EnemyBehavior()
    {
        while (true)
        {
            if (dead) { break; }
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget > EnemyVision)
            {
                destinationSetter.target = spawnLocation.transform;
                isShooting = false;
            }
            else //Player is too far away
            {
                bool hasObstacle = HasObstaclesInFrontOfEnemy();

                if (!hasObstacle)
                {
                    currentState = EnemyState.Pursue;
                }

                if (currentState == EnemyState.Idle)
                {
                    aiPath.canMove = false;
                }
                else
                {
                    Pursue(distanceToTarget, hasObstacle);
                }
            }

            yield return null;
        }
    }
    /// <summary>
    /// if has correct range and line of sight it shoots, if not aproaches
    /// </summary>
    protected void Pursue(float distanceToTarget, bool hasObstacle)
    {
        FlipSprite(target);

        if (distanceToTarget > shootingRange || hasObstacle)
        {
            aiPath.canMove = true;
            destinationSetter.target = target;
            isShooting = false;
        }
        else
        {
            aiPath.canMove = false;
            isShooting = true;
        }

        if (isShooting)
        {
            withConsiderationToTimeAttack();
        }
    }
    /// <summary>
    /// attacks, just checks the cooldown
    /// </summary>
    protected void withConsiderationToTimeAttack()
    {
        if (shootTimer <= 0f)
        {
            Attack();
            shootTimer = shootingCooldown;  // Reset the cooldown timer
        }
        else
        {
            shootTimer -= Time.deltaTime;  // Decrease the cooldown timer
        }
    }

    /// <summary>
    /// Just flips the sprite correspondingly to the target, so it is always facing the player
    /// </summary>
    public void FlipSprite(Transform lookingPoint)
    {
        float horizontal = -transform.position.x + lookingPoint.position.x;
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (horizontal > 0)
        {
            _spriteRenderer.flipX = spriteFlipCustomizer;
        }
        else if (horizontal < 0)
        {
            _spriteRenderer.flipX = !spriteFlipCustomizer;
        }
    }

    /// <summary>
    /// Using Raycast checks for line of sight
    /// </summary>
    public bool HasObstaclesInFrontOfEnemy()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 currentPosition = transform.position;
        currentPosition.z = 0;
        direction.z = 0;
        LayerMask layerMask = LayerMask.GetMask("Player", "Walls");
        RaycastHit2D hit2D = Physics2D.Raycast(currentPosition, direction, 100, layerMask);

        if (hit2D.collider != null && !hit2D.collider.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }

    public void OnBasicEnemyDeathSpawnerUpdate(GameObject a)
    {
        if (spawner != null)
        {
            spawner.GetComponent<SpawnerController>().OneOfOurSpawnedEnemiesDies();
        }
    }

    public void On_Death(GameObject _)
    {
        dead = true;
        Destroy(spawnLocation);

        Sequence deathSequence = DOTween.Sequence();
        deathSequence.Append(transform.DOScale(0f, 0.5f).SetEase(Ease.InBack));
        deathSequence.Join(transform.DORotate(new Vector3(0f, 0f, 360f), 0.5f, RotateMode.FastBeyond360));
        deathSequence.OnComplete(() => Destroy(gameObject));
    }
}
