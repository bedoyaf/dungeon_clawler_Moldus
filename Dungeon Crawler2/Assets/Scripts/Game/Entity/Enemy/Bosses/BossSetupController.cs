using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class BossSetupController : MonoBehaviour
{
    [SerializeField] private Vector2Int roomCoords = new Vector2Int(-200,0);
    [SerializeField] private Vector2Int roomSize = new Vector2Int(300,300);

    [SerializeField] private GameObject DungeonGenerator = null;
    [SerializeField] private GameObject tileMapVisualizer= null;
    [SerializeField] private GameObject DungeonContentGenerator = null;
    [SerializeField] private GameObject statsCounter = null;
    private ProceduralGenerationRoomGenerator dungeonGeneratorScript;
    private TileMapVisualizer tileMapVisualizerScript;
    private DungeonContentGenerator dungeonContentGeneratorScript;


    [SerializeField] private GameObject playerSpawnPoint;
    [SerializeField] private GameObject endSpawnPoint;
    [SerializeField] private GameObject bossSpawnPoint;


    [SerializeField] private GameObject RedBoss;
    [SerializeField] private GameObject PurpleBoss;
    [SerializeField] private GameObject GreenBoss;
    [SerializeField] private GameObject GreenBossMinion;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject Camera;

    [SerializeField] private GameObject healthBar;

    [SerializeField] private ColorEnemy chosenBoss= ColorEnemy.None;

    List<GameObject> minions = new List<GameObject>();

    private GameObject currentBoss;
    private UnityAction<GameObject> onDeathCallback;

    [SerializeField] public UnityEvent OnBossCompleted;

    void Start()
    {
        if (DungeonGenerator != null)
        {
            dungeonGeneratorScript = DungeonGenerator.GetComponent<ProceduralGenerationRoomGenerator>();
        }

        if (tileMapVisualizer != null)
        {
            tileMapVisualizerScript = tileMapVisualizer.GetComponent<TileMapVisualizer>();
        }
        if (DungeonContentGenerator != null)
        {
            dungeonContentGeneratorScript = DungeonContentGenerator.GetComponent<DungeonContentGenerator>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SetupBoss();
        }
    }

    [ContextMenu("Generate Boss Room")]
    public void SetupBoss()
    {
        endSpawnPoint.SetActive(false);
        var room = BasicDungeonGenerationAlgorithms.GenerateSimpleRoom(roomSize, roomCoords);
        var floor = BasicDungeonGenerationAlgorithms.CreateRoomFloor(room);

        tileMapVisualizerScript.ClearArea(room);

        tileMapVisualizerScript.PaintFloorandPerspectiveWallTiles(floor);

        List<ColorEnemy> roomColors = new List<ColorEnemy>();
        dungeonGeneratorScript.GenerateRandomColorForRooms(new List<BoundsInt>{ room }, floor, roomColors);
        //generates walls of rooms
        WallGenerator.CreateWalls(floor, tileMapVisualizerScript);

        PlaceSpawnPoints(room);

        dungeonGeneratorScript.PlacePillars(new List<BoundsInt> { room });

        //scans for pathfinding
        //    dungeonContentGenerator.ScanAreaForPathFinding();

        //     dungeonContentGenerator.DestroyEnemies();
        if(chosenBoss == ColorEnemy.None)
        {
            spawnBoss(roomColors[0]);
        }
        else
        {
            spawnBoss(chosenBoss);
        }
        setupPlayerForBoss();

        setupHealthBar();
    }

    private void setupHealthBar()
    {
        healthBar.SetActive(true);
        var healthController = currentBoss.GetComponent<HealthController>();
        healthBar.GetComponent<BossHealthBarController>().setupHelathController(healthController);
    }

    private void closeHealthBar()
    {
        healthBar.SetActive(false);
    }

    private void KillPreviousBoss()
    {
        Destroy(currentBoss);
    }

    private void PlaceSpawnPoints(BoundsInt room)
    {
        Vector2Int center = new Vector2Int((room.min.x + room.max.x) / 2, (room.min.y + room.max.y) / 2);

        Vector2Int southEdgeCenterTile = new Vector2Int(room.min.x + (room.size.x / 2), room.min.y);

        var roomCenterWorld = tileMapVisualizerScript.GetRealCoordsFromFloorTileMap(center);

        var southEdgeCenterWorld = tileMapVisualizerScript.GetRealCoordsFromFloorTileMap(southEdgeCenterTile);

        if (bossSpawnPoint != null)
        {
            bossSpawnPoint.transform.position = roomCenterWorld;
            endSpawnPoint.transform.position = roomCenterWorld;
        }

        Vector3 playerSpawnTile = new Vector2((roomCenterWorld.x + southEdgeCenterWorld.x) / 2,(roomCenterWorld.y/2 + southEdgeCenterWorld.y) / 2);
        playerSpawnPoint.transform.position = playerSpawnTile;
    }

    private void spawnBoss(ColorEnemy c)//add change based on color
    {   

        if (c == ColorEnemy.Red)
        {
            currentBoss = Instantiate(RedBoss, bossSpawnPoint.transform.position, Quaternion.identity);
            var minOfRoom = tileMapVisualizerScript.GetRealCoordsFromFloorTileMap(roomCoords);
            var maxOfRoom = tileMapVisualizerScript.GetRealCoordsFromFloorTileMap(roomCoords + roomSize);
            currentBoss.GetComponent<HealthController>().onDeathEvent.AddListener(manageBossDeath);
            currentBoss.GetComponent<RedBoss>().Initialize(player.transform, this.transform, this.transform, gameObject, onDeathCallback);
            currentBoss.GetComponent<RedBoss>().setupRedBoss(minOfRoom, maxOfRoom);
        }
        else if (c == ColorEnemy.Purple)
        {
            currentBoss = Instantiate(PurpleBoss, bossSpawnPoint.transform.position, Quaternion.identity);
            currentBoss.GetComponent<HealthController>().onDeathEvent.AddListener(manageBossDeath);
            currentBoss.GetComponent<PurpleBossController>().Initialize(player.transform, this.transform, this.transform, gameObject, onDeathCallback);
        }
        else if(c == ColorEnemy.Green)
        {
            currentBoss = Instantiate(GreenBoss, bossSpawnPoint.transform.position, Quaternion.identity);
            currentBoss.GetComponent<HealthController>().onDeathEvent.AddListener(manageBossDeath);
            currentBoss.GetComponent<GreenEnemyController>().Initialize(player.transform, this.transform, this.transform, null);

            currentBoss.GetComponent<GreenBossControler>().Initialize(GreenBossMinion, player.transform, this.transform, this.transform, statsCounter.GetComponent<EnemyKillCountController>().OnEnemyDeath); // statsCounter.GetComponent<EnemyKillCountController>().OnEnemyDeath
        }
    }

    private void manageBossDeath(GameObject o)
    {
        endSpawnPoint.SetActive(true);
        closeHealthBar();
        Destroy(currentBoss);
    }

    private void setupPlayerForBoss()
    {
        player.transform.position = playerSpawnPoint.transform.position;
        Camera.transform.position = playerSpawnPoint.transform.position;
    }

    public void BossFightCompleted()
    {
        OnBossCompleted.Invoke();
    }

    public void BossFightFailed()
    {
        healthBar.SetActive(false);
    }
}
