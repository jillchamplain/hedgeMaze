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

    [ContextMenu("Generate Grid and Clear Duplicates")]
    public void CreateGrid()
    {

        Node[] nodes = FindObjectsByType(typeof(Node), FindObjectsSortMode.None) as Node[];
        List<Node> nodesToAdd = new List<Node>();

        foreach (Node nodeObject in nodes)
        {
            // If there is already a tile in this spot, delete the new one to be added
            // Otherwise, add the new one to the grid

            if (grid.TryGetValue(nodeObject.coords, out Node node))
            {
                DestroyImmediate(nodeObject.gameObject);
            }
            else
            {
                grid.Add(nodeObject.coords, nodeObject);
            }
        }

        //Debug.Log($"Grid has {grid.Count} tiles");
    }

}
