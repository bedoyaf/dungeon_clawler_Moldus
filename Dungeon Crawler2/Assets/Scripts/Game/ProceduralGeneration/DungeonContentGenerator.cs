using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.CompositeCollider2D;
using Pathfinding;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Generator of the contents of the dungeon that are not tiles, like spawners of enemies
/// </summary>
public class DungeonContentGenerator : MonoBehaviour
{
    //prefabs
    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject redEnemy, purpleEnemy, greenEnemy;
    [SerializeField] private GameObject start, end; //of levels

    [SerializeField] private GameObject parentSpawner; //for hierarchy
    [SerializeField] private GameObject EnemySpawnPointsParent, EnemyParent; //for hierarchy

    [SerializeField] private AstarPath pathfinding;
    [SerializeField] private GameObject statsCounter; //just here to send the incrementation function to the enemy death event

    /// <summary>
    /// Just destroys all spawners
    /// </summary>
    public void DestroySpawners()
    {
        foreach (Transform child in parentSpawner.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Just destroys all enemis with theyr spawnpoints
    /// </summary>
    public void DestroyEnemies()
    {
        foreach (Transform child in EnemyParent.transform)
        {
            if (child.CompareTag("Enemy"))
            {
                Destroy(child.gameObject);
            }
        }
        foreach (Transform child in EnemySpawnPointsParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Scans the map to make a pathfinding area, it waits a bit so it is sure the map is generated
    /// </summary>
    public void ScanAreaForPathFinding()
    {
        StartCoroutine(ScanAfterDelay(0.01f));
    }

    private IEnumerator ScanAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        pathfinding.Scan(); // Perform the scan
    }

    /// <summary>
    /// All the setup for spawning an Enemy
    /// </summary>


    /// <summary>
    /// Just places the start and end of the level
    /// </summary>
    public void PlaceStartAndEnd(Vector2Int startPos, Vector2Int endPos, TileMapVisualizer tileMapVisualizer)
    {
        start.transform.position = tileMapVisualizer.GetRealCoordsFromFloorTileMap(startPos);
        end.transform.position = tileMapVisualizer.GetRealCoordsFromFloorTileMap(endPos);
        Player.SetActive(true);
        Player.transform.position = start.transform.position;

    }
    /// <summary>
    /// Deletes old spawners and spawns the new ones with already generated coords
    /// </summary>
    public void PlaceSpawners(List<List<Vector2Int>> spawnersForEachRoom, List<ColorEnemy> colors, TileMapVisualizer tileMapVisualizer)
    {
        EnemyParent.SetActive(true);
        DestroySpawners();

        for (int i = 0; i < colors.Count; i++)
        {
            GameObject enemyprefab;
            switch (colors[i])
            {
                case ColorEnemy.Purple:
                    enemyprefab = purpleEnemy;
                    break;
                case ColorEnemy.Red:
                    enemyprefab = redEnemy;
                    break;
                case ColorEnemy.Green:
                    enemyprefab = greenEnemy;
                    break;
                default:
                    enemyprefab = purpleEnemy;
                    break;
            }
            foreach (var spawnerPos in spawnersForEachRoom[i])
            {
                GameObject newSpawner = Instantiate(spawner, tileMapVisualizer.GetRealCoordsFromFloorTileMap(spawnerPos), Quaternion.identity, parentSpawner.transform);
                newSpawner.GetComponent<SpawnerController>().Initialize(enemyprefab, Player.transform, EnemySpawnPointsParent.transform, EnemyParent.transform, statsCounter.GetComponent<EnemyKillCountController>().OnEnemyDeath);
                SpriteRenderer spriteRenderer = newSpawner.GetComponent<SpriteRenderer>();
                spriteRenderer.color = GetColorFromEnum(colors[i]);

            }
        }
    }
    /// <summary>
    /// Figures out the color from the enum for the spawners
    /// </summary>
    private UnityEngine.Color GetColorFromEnum(ColorEnemy colorEnemy)
    {
        switch (colorEnemy)
        {
            case ColorEnemy.Red:
                return UnityEngine.Color.red;
            case ColorEnemy.Green:
                return UnityEngine.Color.green;
            case ColorEnemy.Purple:
                return new UnityEngine.Color(0.5f, 0, 0.5f); // Purple
            default:
                return UnityEngine.Color.white;
        }
    }
}
