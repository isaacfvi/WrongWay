using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using NavMeshPlus.Components;
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
    [Tooltip("NavMesh management surfice controller")]
    public NavMeshSurface navMeshSurfice;

    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        LayoutMapGenerate();
        ForestGenerate();
        FinishMap();
    }

    [ContextMenu("Generate New Map")]
    void GenerateNewMap()
    {
        GenerateMap();
    }


    #region Layout Generation

    private Vector2 seed;
    private float waterThreashHold = 0.2f;
    private float grassThreashHold = 0.7f;

    void LayoutMapGenerate()
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

        seed = new Vector2(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f));

        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                float xCoord = (float)i / width * scale + seed.x;
                float yCoord = (float)j / height * scale + seed.y;
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

    #endregion
    #region Forest Generation

    [Header("Forest Settings")]
    [Tooltip("Tree Prefab")]
    public GameObject[] treePrefabs;
    [Tooltip("Parent Object of all trees")]
    public GameObject treeHolder;
    [Tooltip("Grass Prefab")]
    public GameObject[] grassPrefabs;
    [Tooltip("Grass parent")]
    public GameObject grassHolder;

    public float forestScale = 5f;
    void ForestGenerate()
    {
       seed = new Vector2(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f));

       // Limpando holders 
       CleanHolder();

        bool[,] hasTree = new bool[width, height];

       for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                float xCoord = (float)i / width * forestScale + seed.x;
                float yCoord = (float)j / height * forestScale + seed.y;
                float noiseValue = Mathf.PerlinNoise(xCoord, yCoord);

                Vector3Int position = new Vector3Int(i, j, 0);

                if (!grassTilemap.HasTile(position)) continue;

                if (noiseValue > 0.6f && IsAblePutTree(hasTree, i, j))
                {
                    PutTree(position);
                    hasTree[i,j] = true;
                }
                else if(noiseValue < 0.2f)
                {
                    PutGrass(position);
                }
            }
        } 
    }

    bool IsAblePutTree(bool[,] hasTree, int x, int y)
    {
        int width = hasTree.GetLength(0);
        int height = hasTree.GetLength(1);

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                // meio
                if (dx == 0 && dy == 0) continue;

                int nx = x + dx;
                int ny = y + dy;

                // fora do mapa
                if (nx < 0 || ny < 0 || nx >= width || ny >= height) continue;

                if (hasTree[nx, ny])
                    return false;
            }
        }

        return true;
    }
    void PutTree(Vector3Int position)
    {
        int tree = Random.Range(0, treePrefabs.Length);

        Vector3 worldPos = grassTilemap.GetCellCenterWorld(position);

        Instantiate(treePrefabs[tree], worldPos, Quaternion.identity, treeHolder.transform);
    }

    void PutGrass(Vector3Int position)
    {
        int grass = Random.Range(0, grassPrefabs.Length);

        Vector3 worldPos = grassTilemap.GetCellCenterWorld(position);

        Instantiate(grassPrefabs[grass], worldPos, Quaternion.identity, grassHolder.transform);
    }

    void CleanHolder()
    {
        foreach (Transform child in treeHolder.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in grassHolder.transform)
        {
            Destroy(child.gameObject);
        }
        
    }

    #endregion

    #region Final config

    void FinishMap()
    {
        BakeMap();
    }

    void BakeMap()
    {
        if(navMeshSurfice != null)
        {
            StartCoroutine(BakeNavMesh());
        }
        else
        {
            Debug.LogAssertion("Nav Mesh Surfice not set");
        }
    }

    IEnumerator BakeNavMesh()
    {
        // wait until tilemap colliders and physics update
        yield return new WaitForFixedUpdate();
        yield return new WaitForEndOfFrame();

        navMeshSurfice.RemoveData();
        navMeshSurfice.BuildNavMesh();
        navMeshSurfice.AddData();
    }

    #endregion

}
