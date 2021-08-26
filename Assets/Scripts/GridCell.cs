using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [SerializeField] MeshRenderer CellMesh;
    [SerializeField] Gradient CellColour;

    // Start is called before the first frame update
    void Start()
    {
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor("_Color", CellColour.Evaluate(Random.Range(0f, 1f)));
        CellMesh.SetPropertyBlock(propertyBlock);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
