using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int playerLives = 10;
    public int playerScore = 0;

    public int playerMoney = 100;
    public int currentWave = 0;
    public GameObject[] enemyPrefabs;

    [SerializeField] private GameObject moneyText;
    [SerializeField] private GameObject waveText;
    [SerializeField] private GameObject livesText;
    [SerializeField] private Vector2Int spawnCoordinates;
    [SerializeField] private Vector2Int baseCoordinates;

    private bool isInEditorMode = false;
    public GameObject gridReference;
    private GridGenerator grid;
    public float timeBetweenWaves = 60f;
    private bool gameOver = false;
    private GameObject selectedTile;
    private GameObject selectedTowerPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        grid = gridReference.GetComponent<GridGenerator>();
        grid.GenerateGrid(spawnCoordinates, baseCoordinates);
        SetHealthText();
        SetWaveText();
        SetMoneyText();
        StartCoroutine(SpawnWaves());
    }


    private void SetHealthText()
    {
        livesText.GetComponent<TextMeshProUGUI>().text = playerLives.ToString();
    }

    private void SetWaveText()
    {
        waveText.GetComponent<TextMeshProUGUI>().text = currentWave.ToString();
    }

    private void SetMoneyText()
    {
        moneyText.GetComponent<TextMeshProUGUI>().text = playerMoney.ToString();
    }

    IEnumerator SpawnWaves()
    {
        while (!gameOver)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            SpawnWave();
            currentWave++;
        }
    }

    void SpawnWave()
    {
        for (int i = 0; i < Random.Range(1, 10); i++)
        {
            SpawnEnemy();
        }

    }

    void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject spawnTile = grid.GetTile(spawnCoordinates.x, spawnCoordinates.y);
        Transform spawnTransform = spawnTile.GetComponent<Tile>().GetTowerSocketTransform();
        GameObject enemy = Instantiate(enemyPrefabs[randomIndex], spawnTransform.position, spawnTransform.rotation, grid.transform);
        PathFollowing pathFollowing = enemy.GetComponent<PathFollowing>();
        pathFollowing.SetPath(ConvertPathToLocalPositions());
    }

    public void EnemyReachedEnd()
    {
        playerLives--;
        SetHealthText();
        if (playerLives <= 0)
        {
            GameOver();
        }
    }

    public void AddScore(int amount)
    {
        playerScore += amount;
    }

    void GameOver()
    {
        gameOver = true;
        Debug.Log("Game Over!");
    }

    public List<Vector2> GetWorldPath()
    {
        return grid.GetWorldPath();
    }

    public List<Vector2Int> GetPath()
    {
        return grid.GetPath();
    }

    public Vector3 GetTileLocalPosition(int x, int y)
    {
        return grid.GetTileLocalPosition(x, y);
    }

    private List<Vector2> ConvertPathToLocalPositions()
    {
        List<Vector2> positions = new List<Vector2>();
        List<Vector2Int> path = GetPath();
        foreach (Vector2Int tile in path)
        {
            Vector3 localPosition = grid.GetTileLocalPosition(tile.x, tile.y);
            positions.Add(new Vector2(localPosition.x, localPosition.z));
        }
        return positions;
    }

    public void TileClicked(int x, int y, bool isPath)
    {
        if (isPath) return;

        if (!isInEditorMode)
        {
            isInEditorMode = true;
        }
        if (selectedTile != null)
        {
            if (selectedTile.GetComponent<Tile>().GetCoordinates() == new Vector2(x, y))
            {
                selectedTile.GetComponent<Tile>().SetTileSelected(selected: false);
                selectedTile = null;
                isInEditorMode = false;
                return;
            }
            else
            {
                selectedTile.GetComponent<Tile>().SetTileSelected(selected: false);
            }
        }
        selectedTile = grid.GetTile(x, y);
        selectedTile.GetComponent<Tile>().SetTileSelected(selected: true);
    }

    public void SelectTowerToBuild(GameObject towerPrefab)
    {
        if (selectedTile == null)
        {
            Debug.Log("No tile selected");
            return;
        }

        Tower tower = towerPrefab.GetComponent<Tower>();
        if (playerMoney < tower.GetCost())
        {
            Debug.Log("Not enough money");
            return;
        }

        selectedTowerPrefab = towerPrefab;
        PlaceTower();
    }

    public void PlaceTower()
    {
        if (selectedTile == null || selectedTowerPrefab == null) return;
        if (selectedTile.GetComponent<Tile>().HasTower()) return;
        if (playerMoney < selectedTowerPrefab.GetComponent<Tower>().GetCost()) return;
        playerMoney -= selectedTowerPrefab.GetComponent<Tower>().GetCost();
        SetMoneyText();
        selectedTile.GetComponent<Tile>().AddTower(selectedTowerPrefab);
        selectedTile.GetComponent<Tile>().SetTile();
        selectedTile = null;
    }

}