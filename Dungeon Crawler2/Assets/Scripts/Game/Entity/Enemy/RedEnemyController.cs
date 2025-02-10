using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class RedEnemyController : BasicEnemy
{
    //weapons
    [SerializeField] GameObject bombPrefab;
    //configurable for bomb
    [SerializeField] float bombPlantingDistance = 2f;
    [SerializeField] float safeDistance = 10f; // Distance to run away from player and bomb
    [SerializeField] float explosionCooldown = 2.5f; // Cooldown between bomb planting

    private GameObject runAwaySpot; //runs away after placing the bomb
    private Vector3 lastBombPosition;    
    private bool hasPlantedBomb = false;
    private float explosionTimer = 0f;

    void Start()
    {
        ConfigurateBasicFields();
        setupEnemyDeathEvents();
        StartCoroutine(EnemyBehavior());
        runAwaySpot = new GameObject("RunAwaySpot");
        runAwaySpot.transform.parent = spawnLocationsParent;
        spriteFlipCustomizer = false;
        colorOfEnemy = ColorEnemy.Red;
    }

    protected new void setupEnemyDeathEvents()
    {
        var enemycontroller = GetComponent<HealthController>();
        enemycontroller.onDeathEvent.AddListener(OnBasicEnemyDeathSpawnerUpdate);
        enemycontroller.onDeathEvent.AddListener(DestroyRunAwaySpot);
        enemycontroller.onDeathEvent.AddListener(On_Death);
    }

    private void DestroyRunAwaySpot(GameObject _)
    {
        Destroy(runAwaySpot);
    }

    public override void Attack()
    {
        PlantBomb();
    }

    /// <summary>
    /// Just correctly destroys the enemy
    /// </summary>
  /*  public void On_Death(GameObject dead)
    {
        Destroy(spawnLocation);
        Destroy(gameObject);
        Destroy(runAwaySpot);
    }*/

    protected override IEnumerator EnemyBehavior()
    {
        while (true)
        {
            if (dead) { break; }
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget > EnemyVision)
            {
                destinationSetter.target = spawnLocation.transform;
            }
            else
            {
                if (explosionTimer <= 0f)
                {
                    hasPlantedBomb = false; 
                    explosionTimer = 0; 
                    FlipSprite(target.transform);
                }
                else
                {
                    explosionTimer -= Time.deltaTime; // Decrease the cooldown timer
                }

                if (!hasPlantedBomb)
                {
                    if (!HasObstaclesInFrontOfEnemy())
                    {
                        currentState = EnemyState.Pursue;
                    }
                    if (currentState == EnemyState.Idle)
                    {
                        aiPath.canMove = false;
                    }
                    else
                    {
                        destinationSetter.target = target;
                        aiPath.canMove = true;
                    }

                    // Check if the enemy is close enough to the player to plant the bomb
                    if (distanceToTarget <= bombPlantingDistance)
                    {
                        hasPlantedBomb = true;
                        explosionTimer = explosionCooldown;
                        Attack();
                    }
                }
                // If bomb is already planted, run away
                else
                {
                    runAwayFromBomb();
                }
                float diroflooking = -transform.position.x + target.position.x;
            }
            yield return null;
        }
    }
    private void runAwayFromBomb()
    {
        Vector3 directionToPlayer = (transform.position - target.position).normalized;
        Vector3 directionToBomb = (transform.position - lastBombPosition).normalized;
        Vector3 safeDirection = (directionToPlayer + directionToBomb).normalized;
        Vector3 targetPosition = transform.position + safeDirection * safeDistance;
        runAwaySpot.transform.position = targetPosition;
        destinationSetter.target = runAwaySpot.transform;
        FlipSprite(runAwaySpot.transform);
    }
    /// <summary>
    /// instantiates the bomb
    /// </summary>
    void PlantBomb()
    {
        Vector3 lastBombPosition = GetBombPosition();
        Instantiate(bombPrefab, lastBombPosition, Quaternion.identity);
    }
    /// <summary>
    /// calculates where the bomb will be placed
    /// </summary>
    Vector3 GetBombPosition()
    {
        Vector3 directionToPlayer = (target.position - transform.position).normalized;
        Vector3 bombPosition = target.position + directionToPlayer * -bombPlantingDistance;
        return bombPosition;
    }
}
