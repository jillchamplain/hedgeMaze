using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
public class StalkerSpawner : MonoBehaviour
{
    GridManager gridManager;
    [SerializeField] Vector2Int originPos;

    void Start()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        SpawnStalker();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnStalker()
    {
        Debug.Log("Spawning Stalker...");

        List<Node> spawnableTiles = GetSpawnableTiles();

        int randIndex = Random.Range(0, spawnableTiles.Count);

        Node spawnTile = spawnableTiles[randIndex];
        Vector2Int spawnDirection = GetDirectionOfSpawnableTile(spawnTile);

        Debug.Log($"Spawning Stalker at {spawnableTiles[randIndex].gameObject}");
        Debug.Log($"Spawned in direction {spawnDirection}");
    }

    Vector2Int GetDirectionOfSpawnableTile(Node theTile)
    {
        Vector2Int direction = Vector2Int.zero;

        List<Node> rightTiles = GetSpawnableTilesInDirection(Vector2Int.right);
        foreach (Node node in rightTiles)
        {
            if (node == theTile)
                return Vector2Int.right;
        }

        List<Node> leftTiles = GetSpawnableTilesInDirection(Vector2Int.left);
        foreach(Node node in leftTiles)
        {
            if (node == theTile)
                return Vector2Int.left;
        }

        List<Node> upTiles = GetSpawnableTilesInDirection(Vector2Int.up);
        foreach (Node node in upTiles)
        {
            if (node == theTile)
                return Vector2Int.up;
        }

        List<Node> downTiles = GetSpawnableTilesInDirection(Vector2Int.down);
        foreach( Node node in downTiles)
        {
            if(node == theTile)
                return Vector2Int.down;
        }

        return direction;
    }

    List<Node> GetSpawnableTiles()
    {
        List<Node> spawnableTiles = new List<Node>();
        List<Node> leftSpawnTiles = GetSpawnableTilesInDirection(Vector2Int.left);
        foreach (Node node in leftSpawnTiles)
        {
            spawnableTiles.Add(node);
        }

        List<Node> rightSpawnTiles = GetSpawnableTilesInDirection(Vector2Int.right);
        foreach (Node node in rightSpawnTiles)
        {
            spawnableTiles.Add(node);
        }

        List<Node> upSpawnTiles = GetSpawnableTilesInDirection(Vector2Int.up);
        foreach (Node node in upSpawnTiles)
        {
            spawnableTiles.Add(node);
        }

        List<Node> downSpawnTiles = GetSpawnableTilesInDirection(Vector2Int.down);
        foreach (Node node in downSpawnTiles)
        {
            spawnableTiles.Add(node);
        }

        return spawnableTiles;
    }

    List<Node> GetSpawnableTilesInDirection(Vector2Int direction)
    {
        List<Node> spawnNodes = new List<Node>();
        
        Dictionary<Vector2Int, Node> grid = gridManager.Grid;

        //Iterate through dictionary keys in direction
        for(int i = 1; i < gridManager.gridSize.x; i++)
        {
            Node theNode;

            Vector2Int checkLocationOffset = new Vector2Int(direction.x * i, direction.y * i);
            Vector2Int checkLocation = originPos + checkLocationOffset;
            //Debug.Log(checkLocation);

            //If within bounds of Grid
            if ((checkLocation.x - 1 >= 0 && checkLocation.x + 1 < gridManager.gridSize.x) && (checkLocation.y - 1 >= 0 && checkLocation.y + 1 < gridManager.gridSize.y))
            {
                theNode = grid[checkLocation];
                if (theNode.type == ENodeType.NONE)
                {
                    //Rotate direction by 90 degrees counter clockwise
                    Vector2 perpendicularDirection = Vector2.Perpendicular(direction);
                    Vector2Int rotatedDirection = new Vector2Int(Mathf.RoundToInt(perpendicularDirection.x), Mathf.RoundToInt(perpendicularDirection.y));

                    //Get "left" and "right" nodes using rotated direction
                    Vector2Int leftOffset = new Vector2Int(rotatedDirection.x, rotatedDirection.y);
                    Vector2Int rightOffset = new Vector2Int(-rotatedDirection.x, -rotatedDirection.y);

                    Node leftNode = grid[checkLocation + leftOffset];
                    Node rightNode = grid[checkLocation + rightOffset];

                    if (CheckNodeForSpawn(leftNode, direction))
                        spawnNodes.Add(leftNode);
                    if(CheckNodeForSpawn(rightNode, direction))
                        spawnNodes.Add(rightNode);
                }
                else if (theNode.type == ENodeType.HEDGE)
                    break;
            }
        }

        return spawnNodes;
    }

    bool CheckNodeForSpawn(Node node, Vector2Int direction)
    {
        bool canSpawn = false;

        if (node.type == ENodeType.HEDGE)
            return false;

        Vector2Int oppositeDirection = new Vector2Int(-direction.x, -direction.y);
        Dictionary<Vector2Int, Node> grid = gridManager.Grid;

        if (grid[node.coords + oppositeDirection].type == ENodeType.HEDGE)
        {
            canSpawn = true;
            Debug.Log($"Node at {node.coords}");
        }

        return canSpawn;
    }
}
