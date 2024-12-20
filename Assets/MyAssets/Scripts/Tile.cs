using UnityEngine;

public class Tile : MonoBehaviour
{
    private int row;
    private int col;

    private GameObject tower;
    [SerializeField] private Material pathMaterial;
    [SerializeField] private Material tileMaterial;
    [SerializeField] private Material selectedMaterial;
    [SerializeField] private Transform towerSocket;
    private bool hasTower = false;

    private bool isPath = false;

    private void Start()
    {
    }

    public void SetCoordinates(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    public void SetPath()
    {
        isPath = true;
        GetComponent<Renderer>().material = pathMaterial;
    }

    public void SetTile()
    {
        isPath = false;
        GetComponent<Renderer>().material = tileMaterial;
    }

    public void AddTower(GameObject tower)
    {
        this.tower = tower;
        GameObject instantiatedTower = Instantiate(tower, towerSocket.position, Quaternion.identity);
        instantiatedTower.transform.parent = this.transform; // Set the tower as a child of the tile
        hasTower = true;
    }

    public int GetRow()
    {
        return row;
    }

    public int GetCol()
    {
        return col;
    }

    public Vector2 GetCoordinates()
    {
        return new Vector2(row, col);
    }

    public Transform GetTowerSocketTransform()
    {
        return towerSocket;
    }

    public bool HasTower()
    {
        return hasTower;
    }

    void OnMouseDown()
    {
        Debug.Log($"Tile clicked at row: {row}, col: {col}");
        GameManager.instance.TileClicked(row, col, isPath);
    }

    public void SetTileSelected(bool selected)
    {
        if (selected)
        {
            GetComponent<Renderer>().material = selectedMaterial;
        }
        else
        {
            GetComponent<Renderer>().material = tileMaterial;
        }

    }
}