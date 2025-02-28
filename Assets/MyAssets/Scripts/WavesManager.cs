using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public struct Enemies
{
    public GameObject lightEnemy;
    public GameObject mediumEnemy;
    public GameObject heavyEnemy;
}
[System.Serializable]
public struct Wave
{
    public int lightEnemies;
    public int mediumEnemies;
    public int heavyEnemies;
}

public class WavesManager : MonoBehaviour
{
    private int currentWave = 0;
    private List<Vector2> path;
    private Vector2Int spawnCoordinates;

    [SerializeField] private GameObject waveText;

    [SerializeField]private GridGenerator grid;
    private int maxWaves;

    // in seconds
    [SerializeField] private int timeBetweenWaves = 30;

    [SerializeField] private Wave[] waves;
    [SerializeField] private Enemies enemyPrefabs;


    public void StartWaves()
    {
        Debug.Log("Starting waves");
        maxWaves = waves.Length;
        path = ConvertPathToLocalPositions();
        spawnCoordinates = GameManager.instance.GetSpawnCoordinates();
        StartCoroutine(SpawnWaves());
    }

    public void Stop(){
        StopAllCoroutines();
    }

    private void SetWaveText()
    {
        waveText.GetComponent<TextMeshProUGUI>().text = (currentWave + 1) + "/" + maxWaves;
    }

    private IEnumerator SpawnWaves()
    {
        while (currentWave < maxWaves)
        {
            SetWaveText();
            SpawnWave(waves[currentWave]);
            yield return new WaitForSeconds(timeBetweenWaves);
            currentWave++;
        }
        Debug.Log("No more waves");
    }

    private void SpawnWave(Wave wave)
{
    Debug.Log("Spawning wave");
    StartCoroutine(SpawnEnemiesWithDelay(wave));
}

private IEnumerator SpawnEnemiesWithDelay(Wave wave)
{
    for (int i = 0; i < wave.lightEnemies; i++)
    {
        SpawnEnemy(enemyPrefabs.lightEnemy);
        yield return new WaitForSeconds(1f); 
    }
    for (int i = 0; i < wave.mediumEnemies; i++)
    {
        SpawnEnemy(enemyPrefabs.mediumEnemy);
        yield return new WaitForSeconds(1f);
    }
    for (int i = 0; i < wave.heavyEnemies; i++)
    {
        SpawnEnemy(enemyPrefabs.heavyEnemy);
        yield return new WaitForSeconds(1f);
    }
}

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Debug.Log("Spawning enemy");
        GameObject spawnTile = grid.GetTile(spawnCoordinates.x, spawnCoordinates.y);
        Transform spawnTransform = spawnTile.GetComponent<Tile>().GetTowerSocketTransform();
        GameObject enemy = Instantiate(enemyPrefab, spawnTransform.position, spawnTransform.rotation, grid.transform);
        enemy.GetComponent<EnemyManager>().SetPath(path);
    }

    private List<Vector2> ConvertPathToLocalPositions()
    {
        List<Vector2> positions = new();
        List<Vector2Int> path = grid.GetPath();
        foreach (Vector2Int tile in path)
        {
            Vector3 localPosition = grid.GetTileLocalPosition(tile.x, tile.y);
            positions.Add(new Vector2(localPosition.x, localPosition.z));
        }
        return positions;
    }
}