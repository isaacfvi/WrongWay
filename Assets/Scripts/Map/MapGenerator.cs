using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [Header("Tiles Sprites")]
    [Tooltip("Grass Tile")]
    public Tile grassTile;
    [Tooltip("Dirt Tile")]
    public RuleTile dirtTile;
    [Tooltip("Water Tile")]
    public RuleTile waterTile;

    [Header("Grid Objects")]
    [Tooltip("Grass Tilemap Object")]
    public Tilemap grassTilemap;
    [Tooltip("Dirt Tilemap Object")]
    public Tilemap dirtTilemap;
    [Tooltip("Water Tilemap Object")]
    public Tilemap waterTilemap;

    [Header("Map Settings")]
    [Tooltip("Map height")]
    public int height = 20;
    [Tooltip("Map Weight")]
    public int width = 20;
    [Tooltip("Pelin Noise Scale")]
    public float scale = 1;
    
    private Vector2 seed;
    private float waterThreashHold = 0.3f;
    private float grassThreashHold = 0.7f;

    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {   
        if(grassTile == null || dirtTile == null || waterTile == null)
        {
            Debug.Log("tiles are not set");
            return;
        }

        if(grassTilemap == null || dirtTilemap == null || waterTilemap == null)
        {
            Debug.Log("Tilemaps are not set");
            return;
        }

        //Cleaning map
        grassTilemap.ClearAllTiles();
        dirtTilemap.ClearAllTiles();
        waterTilemap.ClearAllTiles();

        seed = new Vector2(Random.Range(50f, 100f), Random.Range(50f, 100f));

        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                float xCoord = (float)i / width * scale * seed.x;
                float yCoord = (float)j / height * scale * seed.y;
                float noiseValue = Mathf.PerlinNoise(xCoord, yCoord);

                Vector3Int position = new Vector3Int(i, j, 0);
                
                SetTile(position, noiseValue);
            }
        }

        FixIsolatedTiles();
        RefreshTiles();
    }

    void SetTile(Vector3Int position, float perlinNoisevalue)
    {
        if(perlinNoisevalue < waterThreashHold){
            waterTilemap.SetTile(position, waterTile);
        }
        else if(perlinNoisevalue < grassThreashHold)
        {
            grassTilemap.SetTile(position, grassTile);
        }
        else
        {
            dirtTilemap.SetTile(position, dirtTile);
        }
    }

    void RefreshTiles()
    {
        // atualiza os tilemaps por conta das rules
        grassTilemap.RefreshAllTiles();
        dirtTilemap.RefreshAllTiles();
        waterTilemap.RefreshAllTiles();
    }

    [ContextMenu("Generate New Map")]
    void GenerateNewMap()
    {
        GenerateMap();
    }

    void FixIsolatedTiles()
{
    for(int i = 0; i < height; i++)
    {
        for(int j = 0; j < width; j++)
        {
            Vector3Int position = new Vector3Int(i, j, 0);
            
            // Verificar se é um Tile isolado e o remove devido a falta de sprites 
            if(dirtTilemap.HasTile(position))
            {
                if(CheckIfIsolated(position, dirtTilemap))
                {
                    dirtTilemap.SetTile(position, null);
                    grassTilemap.SetTile(position, grassTile);
                }
            }
            
            if(waterTilemap.HasTile(position))
            {
                bool isIsolated = CheckIfIsolated(position, waterTilemap);
                if(isIsolated)
                {
                    waterTilemap.SetTile(position, null);
                    grassTilemap.SetTile(position, grassTile);
                }
            }
        }
    }
    
    RefreshTiles();
}

bool CheckIfIsolated(Vector3Int position, Tilemap tilemap)
{
    int adjacentDirtCount = 0;
    Vector3Int[] neighborsPos = {
        new Vector3Int(position.x - 1, position.y),
        new Vector3Int(position.x, position.y - 1),
        new Vector3Int(position.x, position.y + 1),
        new Vector3Int(position.x + 1, position.y)
        };

    foreach (Vector3Int neighborPos in neighborsPos)
    {
        if(tilemap.HasTile(neighborPos))
        {
            adjacentDirtCount++;
        }
    }

    return adjacentDirtCount < 2;
}

}
