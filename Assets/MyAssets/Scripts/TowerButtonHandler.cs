
using UnityEngine;
using UnityEngine.UI;

public class TowerButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private GameObject towerButton;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
        towerButton.GetComponent<Button>().onClick.AddListener(() => OnTowerButtonClicked(towerPrefab));
    }

    void OnTowerButtonClicked(GameObject towerPrefab)
    {
        Debug.Log("Tower button clicked");
        gameManager.SelectTowerToBuild(towerPrefab);
    }
}