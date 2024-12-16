using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int playerLives = 10;
    public int playerScore = 0;
    public int currentWave = 0;
    public GameObject[] enemyPrefabs;
    

    [SerializeField] private Vector2Int spawnCoordinates;
    [SerializeField] private Vector2Int baseCoordinates;
    public GameObject gridReference;
    private GridGenerator grid;
    public float timeBetweenWaves = 60f;

    private bool gameOver = false;

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
        StartCoroutine(SpawnWaves());
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
        for (int i = 0; i < Random.Range(1,10) ; i++)
        {
            SpawnEnemy();
        }
    
    }

    void SpawnEnemy() {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject spawnTile = grid.GetTile(spawnCoordinates.x, spawnCoordinates.y);
        Transform spawnTransform = spawnTile.GetComponent<Tile>().GetTowerSocketTransform();
        GameObject enemy = Instantiate(enemyPrefabs[randomIndex], spawnTransform.position, Quaternion.identity);
        PathFollowing pathFollowing = enemy.GetComponent<PathFollowing>();
        pathFollowing.path = grid.GetPath();
    }

    public void EnemyReachedEnd()
    {
        playerLives--;
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

    public List<Vector2> GetPath()
    {
        return grid.GetPath();
    }

}