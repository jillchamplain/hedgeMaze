using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    [SerializeField] public int unityGridSize = 1;
    public int UnityGridSize {  get { return unityGridSize; } }
    
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    Dictionary<Vector2Int, Node> Grid {  get { return grid; } }

    private void Awake()
    {
        //CreateGrid();

    }
    void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coords = new Vector2Int(x, y);
                grid.Add(coords, new Node(coords));

                //Grid Visualizer
                /*GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Vector3 pos = new Vector3(coords.x * unityGridSize, 0f, coords.y * unityGridSize);
                cube.transform.position = pos;
                cube.transform.SetParent(transform);*/
            }
        }
    }
}
