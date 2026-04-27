using UnityEngine;

public class FarmGridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject farmTilePrefab;
    [SerializeField] private int rows = 4;
    [SerializeField] private int columns = 6;
    [SerializeField] private float spacing = 1f;

    void Start()
    {
        SpawnGrid();
    }

    void SpawnGrid()
    {
        // clear any existing tiles first
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 pos = new Vector3(
                    transform.position.x + col * spacing,
                    transform.position.y + row * spacing,
                    0
                );

                Instantiate(farmTilePrefab, pos, Quaternion.identity, transform);
            }
        }

        Debug.Log("Farm grid spawned: " + rows + "x" + columns + " = " + (rows * columns) + " tiles");
    }
}