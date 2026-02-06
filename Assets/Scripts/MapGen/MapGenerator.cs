using UnityEngine;
using UnityEngine.Tilemaps;

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
    
    //Testing
    [SerializeField] private GameObject testTile;
    
    [Header("Player Settings")] //Remove when done testing
    [SerializeField] private int minY = 5;
    [SerializeField] private int maxY = 7;
    [SerializeField] private int maxX = 6;


    void Start()
    {
        //GenerateTower();
        Generate();
    }
    
    
    
    public void Generate()
    {
        tilemap.ClearAllTiles();
        
        Vector3Int currentPos = new Vector3Int(0, startY, 0);

        for (int i = 0; i < mapHeight/6; i++)
        {
            int drop = Random.Range(minY, maxY);
            currentPos.y -= drop;
            
            int moveDistance;
            if (currentPos.x < -3) 
            {
                moveDistance = Random.Range(minPlatformX, maxPlatformX);
            }
            else if (currentPos.x > 3) 
            {
                moveDistance = Random.Range(-maxX, -minHorizontalMove);
            }
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
        }

        DrawBorders(10, -(mapHeight));
        DrawBackground(mapWidth, mapHeight);
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
            for (int t = 0; t < 2; t++) // 벽 두께만큼 반복
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
            for (int j = 0; j < height+10; j++)
            {
                int x = i - halfWidth;
                int y = j - height;

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
    }
}
