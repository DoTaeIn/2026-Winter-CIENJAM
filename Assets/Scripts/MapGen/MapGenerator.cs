using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using UnityEngine.Events; // 리스트 사용을 위해 필수

public class MapGenerator : MonoBehaviour
{
    [Header("Map Settings")] 
    [SerializeField] private int mapWidth = 20;
    [SerializeField] private int mapHeight = 100;
    [SerializeField] private int startY = 0;
    [SerializeField] private int minPlatformX = 3;
    [SerializeField] private int maxPlatformX = 5;
    [SerializeField] private int maxPlatformY = 3;
    
    [Header("Platform Settings")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase ruleTile;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private Tilemap backTilemap;
    [SerializeField] private int minHorizontalMove = 3;
    public Vector2 startPoint;
    
    [Header("Monster Settings")]
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private int monsterCount = 5;
    [SerializeField] private Transform monsterParent;
    
    private List<Vector3> validSpawnPoints = new List<Vector3>();

    [Header("Chest Settings")]
    public GameObject chestPrefab;
    public List<GameObject> chestItems;
    public int chestCount = 3;
    
    [Header("Player Settings")] 
    [SerializeField] private int minY = 5;
    [SerializeField] private int maxY = 7;
    [SerializeField] private int maxX = 6;

    public UnityEvent onMapGenerated;

    void Start()
    {
        Generate();
    }
    
    public void Generate()
    {
        MapTotalReset();
        
        Vector3Int currentPos = new Vector3Int(0, startY, 0);

        for (int i = 0; i < mapHeight/6; i++)
        {
            int drop = Random.Range(minY, maxY);
            currentPos.y -= drop;
            
            int moveDistance;
            if (currentPos.x < -3) moveDistance = Random.Range(minPlatformX, maxPlatformX);
            else if (currentPos.x > 3) moveDistance = Random.Range(-maxX, -minHorizontalMove);
            else 
            {
                moveDistance = Random.Range(minHorizontalMove, maxX);
                if (Random.value > 0.5f) moveDistance *= -1; 
            }

            currentPos.x += moveDistance;
            currentPos.x = Mathf.Clamp(currentPos.x, -8, 8);

            int width = Random.Range(minPlatformX, maxPlatformX);
            int height = Random.Range(1, maxPlatformY);
            
            DrawThickPlatform(currentPos, width, height);

            Vector3 spawnPos = tilemap.GetCellCenterWorld(currentPos) + Vector3.up; 
            if(i == 0) startPoint = spawnPos;
            else validSpawnPoints.Add(spawnPos);
        }

        DrawBorders(10, -(mapHeight));
        DrawBackground(mapWidth, mapHeight);
        
        SpawnMonsters();
        onMapGenerated.Invoke();
    }
    
    void SpawnMonsters()
    {
        List<Enemy> enemies = new List<Enemy>();
        if (validSpawnPoints.Count == 0) return;

        int spawnLimit = Mathf.Min(monsterCount, validSpawnPoints.Count);

        for (int i = 0; i < spawnLimit - chestCount; i++)
        {
            int randomIndex = Random.Range(0, validSpawnPoints.Count);
            
            Vector3 spawnPos = validSpawnPoints[randomIndex];
            
            if (monsterPrefab != null)
            {
                GameObject gm = Instantiate(monsterPrefab, spawnPos, Quaternion.identity, monsterParent);
                if(gm.GetComponent<Enemy>() != null)
                    enemies.Add(gm.GetComponent<Enemy>());
            }
            
            validSpawnPoints.RemoveAt(randomIndex);
        }

        for (int i = 0; i < chestCount; i++)
        {
            Vector3 spawnPos = validSpawnPoints[i];
            if (chestPrefab != null) Instantiate(chestPrefab, spawnPos, Quaternion.identity);
        }
        
        GameSystem gameSystem = FindFirstObjectByType<GameSystem>();
        gameSystem.enemies = enemies;
    }

    void DrawThickPlatform(Vector3Int topCenterPos, int width, int height)
    {
        int halfWidth = width / 2;
        
        for (int x = -halfWidth; x <= halfWidth; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePos = new Vector3Int(topCenterPos.x + x, topCenterPos.y - y, 0);
                tilemap.SetTile(tilePos, ruleTile);
            }
        }
    }

    void DrawBorders(int topY, int bottomY)
    {
        wallTilemap.ClearAllTiles();
        int wallBorder = mapWidth / 2;
        
        for (int x = -wallBorder; x <= wallBorder; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                wallTilemap.SetTile(new Vector3Int(x, topY - y, 0), ruleTile);
            }
        }
        
        for (int y = topY; y >= bottomY; y--)
        {
            for (int t = 0; t < 2; t++) 
            {
                wallTilemap.SetTile(new Vector3Int(-wallBorder - t, y, 0), ruleTile);
                wallTilemap.SetTile(new Vector3Int(wallBorder + t, y, 0), ruleTile);
            }
        }
        
        for (int x = -wallBorder; x <= wallBorder; x++)
        {
            for (int y = 0; y < 2; y++)
            {
                wallTilemap.SetTile(new Vector3Int(x, bottomY + y, 0), ruleTile);
            }
        }
    }

    void DrawBackground(int width, int height)
    {
        int halfWidth = width / 2;
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height + 10; j++)
            {
                int x = i - halfWidth;
                int y = 10 - j;

                Vector3Int tilePos = new Vector3Int(x, y, 0);
                backTilemap.SetTile(tilePos, wallTile);
            }
        }
    }

    public void MapTotalReset()
    {
        tilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
        backTilemap.ClearAllTiles();
        
        validSpawnPoints.Clear();
        if (monsterParent != null)
        {
            foreach (Transform child in monsterParent)
            {
                Destroy(child.gameObject);
            }
        }
    }
}