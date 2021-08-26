using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    [SerializeField] GameObject CellPrefab;
    [SerializeField] Vector2Int GridDimensions = new Vector2Int(100, 100);
    [SerializeField] Vector2Int CellDimensions = Vector2Int.one;

    [SerializeField] Vector2 HeightNoiseScale = new Vector2(128f, 128f);

    [SerializeField] float MinHeight = 1f;
    [SerializeField] float MaxHeight = 5f;
    [SerializeField] float RandomNoise = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        // spawn the grid cells
        for (int row = 0; row < GridDimensions.y; ++row)
        {
            for (int column = 0; column < GridDimensions.x; ++column)
            {
                var newCell = Instantiate(CellPrefab, new Vector3(column * CellDimensions.x, 0f, row * CellDimensions.y), Quaternion.identity, transform);
                newCell.name = "Cell_" + row + "_" + column;

                // pure random is very chaotic - not ideal for a walkable terrain
                //newCell.transform.localScale = new Vector3(1, Random.Range(MinHeight, MaxHeight), 1);

                float heightPercentage = Mathf.PerlinNoise(column / HeightNoiseScale.x, row / HeightNoiseScale.y);
                heightPercentage *= 1f + Random.Range(-RandomNoise, RandomNoise);
                newCell.transform.localScale = new Vector3(1, Mathf.Lerp(MinHeight, MaxHeight, heightPercentage), 1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
