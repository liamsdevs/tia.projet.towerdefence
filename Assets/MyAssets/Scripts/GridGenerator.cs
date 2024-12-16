using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
public class GridGenerator : MonoBehaviour
{
    public Vector2Int gridSize = new Vector2Int(10, 10);

    public float gap = .1f ;
    
    [SerializeField] private List<Vector2Int> path = new List<Vector2Int>();
    public GameObject tilePrefab;
    public GameObject groundPrefab;
    public GameObject spawnPrefab;

    public GameObject basePrefab;

    private GameObject[,] tiles;

    private enum Direction
    {
        LEFT,
        RIGHT,
        DOWN,
        UP
    };

    public float tileSize = 1f;

    void Start()
    {
    }

    public GameObject GetTile(int row, int col)
    {
        return tiles[row, col];
    }
    public void GenerateGrid(Vector2Int spawnCoordinates, Vector2Int BaseCoordinates)
    {
        int rows = gridSize.x;
        int columns = gridSize.y;

        print("Path: " + string.Join(", ", path.Select(p => $"({p.x}, {p.y})")));
        // Create the ground
        Vector3 groundSize = new Vector3(columns * (tileSize + gap) + tileSize, tileSize, rows * (tileSize + gap) + tileSize);
        GameObject ground = Instantiate(groundPrefab, groundSize / 2, Quaternion.identity);
        ground.transform.localScale = groundSize;
        ground.transform.localPosition = new Vector3(0, 0, 0);
        tiles = new GameObject[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 position = new Vector3(col * (tileSize + gap), 0.3f, row * (tileSize + gap));
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                tile.transform.localScale = new Vector3(tileSize, .1f, tileSize);
                tile.transform.parent = transform;
                Tile tileScript = tile.GetComponent<Tile>();
                tileScript.SetCoordinates(row, col);
                tiles[row, col] = tile;

                if (row == (int)BaseCoordinates.x && col == (int)BaseCoordinates.y)
                {
                    tileScript.AddTower(basePrefab);
                    tileScript.SetPath();
                }

                if (row == (int)spawnCoordinates.x && col == (int)spawnCoordinates.y)
                {
                    tileScript.AddTower(spawnPrefab);
                    tileScript.SetPath();
                }

                if (path.Contains(new Vector2Int(row, col)))
                {
                    tileScript.SetPath();
                }

            }
        }

        gameObject.transform.position = new Vector3(-(groundSize.x / 2 - tileSize), 0, -(groundSize.z / 2 - tileSize));
    }
    private IEnumerable<Vector2Int> GetNeighbors(Vector2Int tile, bool includeDiagonals = false)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        Vector2Int[] directions = new Vector2Int[] {
            new Vector2Int(0, 1), // right
            new Vector2Int(0, -1), // left
            new Vector2Int(1, 0), // up
            new Vector2Int(-1, 0) // down
        };
        if (includeDiagonals)
        {
            directions = directions.Concat(new Vector2Int[] {
                new Vector2Int(1, 1), // up right
                new Vector2Int(1, -1), // up left
                new Vector2Int(-1, 1), // down right
                new Vector2Int(-1, -1) // down left
            }).ToArray();
        }

        foreach (var direction in directions)
        {
            Vector2Int neighbor = tile + direction;
            if (!isOutOfBounds(neighbor))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    private bool isOutOfBounds(Vector2Int position)
    {
        return position.x < 0 || position.y < 0 || position.x >= gridSize.x || position.y >= gridSize.y;
    }

    private bool IsInGrid(Vector2Int position)
    {
        return position.x >= 0 && position.y >= 0 && position.x < gridSize.x && position.y < gridSize.y;
    }

    public Vector3 GetTileWorldPosition(int x, int y)
    {
        return GetTile(x, y).transform.position;
    }

    public List<Vector2> GetPath()
    {
        List<Vector2> path = new List<Vector2>();
        foreach (Vector2Int tile in this.path)
        {
            Vector3 pos = GetTileWorldPosition(tile.x, tile.y);
            path.Add(new Vector2(pos.x, pos.z));
        }
        return path;
    }

}

