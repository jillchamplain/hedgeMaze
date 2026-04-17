using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] public Vector2Int gridSize;
    [SerializeField] public int unityGridSize = 1;
    [SerializeField] Transform playerTransform;
    [SerializeField] public Vector2Int playerGridPosition;
    [SerializeField] Vector2Int lastPlayerGridPosition;
    public int UnityGridSize { get { return unityGridSize; } }

    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } }

    private void Awake()
    {
        
    }

    private void Start()
    {
        CreateGrid();
        playerGridPosition = GetPlayerGridPosition();
    }

    private void Update()
    {
        if(GetPlayerGridPosition() != playerGridPosition)
        {
            lastPlayerGridPosition = playerGridPosition;
            playerGridPosition = GetPlayerGridPosition();

            grid[lastPlayerGridPosition].LeaveNode();
            grid[playerGridPosition].EnterNode();
        }
    }

    public Vector2Int GetPlayerGridPosition()
    {
        Vector2Int playerPos = new Vector2Int(Mathf.RoundToInt(playerTransform.position.x), Mathf.RoundToInt(playerTransform.position.z));
        return playerPos;
    }

    [ContextMenu("Clear Grid")]
    public void ClearGrid()
    {
        grid.Clear();
    }


    [ContextMenu("Generate Grid and Clear Duplicates")]
    public void CreateGrid()
    {

        Node[] nodes = FindObjectsByType(typeof(Node), FindObjectsSortMode.None) as Node[];
        Dictionary<Vector2Int, Node> nodesToAdd = new Dictionary<Vector2Int, Node>();

        foreach (Node nodeObject in nodes)
        {
            if (grid.TryGetValue(nodeObject.coords, out Node gridNode))
            {
                Debug.LogWarning($"GRID DUPLICATE: {nodeObject.gameObject.name} at {nodeObject.coords} conflicts with existing {gridNode.gameObject.name}", nodeObject);
            }
            else if (nodesToAdd.TryGetValue(nodeObject.coords, out Node addNode))
            {
                // This case was previously silent — now we can see it
                Debug.LogWarning($"PENDING DUPLICATE: {nodeObject.gameObject.name} at {nodeObject.coords} conflicts with pending {addNode.gameObject.name}", nodeObject);
            }
            else
            {
                nodesToAdd.Add(nodeObject.coords, nodeObject);
            }
        }
        //Clean up for null items

        foreach (KeyValuePair<Vector2Int,Node> pair in nodesToAdd)
        {
            grid.Add(pair.Key, pair.Value);
        }
    }

}
