using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[ExecuteAlways]
public class GridManager : MonoBehaviour
{
    [SerializeField] public Vector2Int gridSize;
    [SerializeField] public int unityGridSize = 1;
    public int UnityGridSize {  get { return unityGridSize; } }
    
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid {  get { return grid; } }

    private void Awake()
    {
        CreateGrid();
    }

    public void CreateGrid()
    {

        Node[] nodes = FindObjectsByType(typeof(Node), FindObjectsSortMode.None) as Node[];
        List<Node> nodesToAdd = new List<Node>();

        foreach (Node nodeObject in nodes)
        {
            grid.Add(nodeObject.coords, nodeObject);
        }

        //Debug.Log($"Grid has {grid.Count} tiles");
    }

}
