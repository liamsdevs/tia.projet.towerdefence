using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int playerLives = 10;
    public int playerScore = 0;

    public int playerMoney = 100;

    [SerializeField] private WavesManager wavesManager;

    [SerializeField] private GameObject moneyText;
    [SerializeField] private GameObject livesText;

    [SerializeField] private GameObject gameOverText;

    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject ath;
    [SerializeField] private Vector2Int spawnCoordinates;
    [SerializeField] private Vector2Int baseCoordinates;

    private bool isInEditorMode = false;
    public GameObject gridReference;
    private GridGenerator grid;
    private bool gameOver = false;
    private GameObject selectedTile;
    private GameObject selectedTowerPrefab;

    public GridGenerator GetGrid()
    {
        return grid;
    }
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
        SetMoneyText();
    }

    public void StartGame()
    {
        ath.SetActive(true);
        Invoke("StartGameCountdown", 10f);
    }

    void StartGameCountdown()
    {
        wavesManager.StartWaves();
    }


    private void SetHealthText()
    {
        livesText.GetComponent<TextMeshProUGUI>().text = playerLives.ToString();
    }

    private void SetMoneyText()
    {
        moneyText.GetComponent<TextMeshProUGUI>().text = playerMoney.ToString();
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
        gameOverText.SetActive(true);
        Invoke("Restart", 10f);
    }

    void Win()
    {
        gameOver = true;
        winText.SetActive(true);
        Invoke("Restart", 10f);
    }

    void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void GainMoney(int amount)
    {
        playerMoney += amount;
        SetMoneyText();
    }

    public List<Vector2> GetWorldPath()
    {
        return grid.GetWorldPath();
    }

    public Vector3 GetTileLocalPosition(int x, int y)
    {
        return grid.GetTileLocalPosition(x, y);
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

    public Vector2Int GetSpawnCoordinates()
    {
        return spawnCoordinates;
    }

    public Vector2Int GetBaseCoordinates()
    {
        return baseCoordinates;
    }

}