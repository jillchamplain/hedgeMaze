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
        //Iterate left, right, up, down and checking "left" or "right" tiles

        //For up (0, 1) check -1 and 1 for x (1,0)
        //For down (0, -1) check -1 and 1 for x

        //For left (-1, 0) check -1 and 1 for y (0,1)
        //For right (1,0) check -1 and 1 for y

        List<Node> spawnableTiles = GetSpawnabelTilesInDirection(Vector2Int.right);
    }

    List<Node> GetSpawnabelTilesInDirection(Vector2Int direction)
    {
        List<Node> spawnNodes = new List<Node>();
        
        Dictionary<Vector2Int, Node> grid = gridManager.Grid;

        //Iterate through dictionary keys in direction
        for(int i = 0; i < gridManager.gridSize.x; i++)
        {
            Node theNode;
            Vector2Int checkLocation = new Vector2Int(direction.x * i, direction.y * i);

            //If within bounds of Grid
            if ((checkLocation.x - 1 >= 0 && checkLocation.x + 1 < gridManager.gridSize.x) && (checkLocation.y - 1 >= 0 && checkLocation.y + 1 < gridManager.gridSize.y))
            {
                theNode = grid[checkLocation];
                if(theNode.type == ENodeType.NO_HEDGE)
                {
                    //Rotate direction by 90 degrees counter clockwise
                    Vector2 perpendicularDirection = Vector2.Perpendicular(direction);
                    Vector2Int rotatedDirection = new Vector2Int(Mathf.RoundToInt(perpendicularDirection.x), Mathf.RoundToInt(perpendicularDirection.y));

                    //Get "left" and "right" nodes using rotated direction
                    Vector2Int leftOffset = new Vector2Int(rotatedDirection.x, rotatedDirection.y);
                    Vector2Int rightOffset = new Vector2Int(-rotatedDirection.x, -rotatedDirection.y);

                    Node leftNode = grid[checkLocation + leftOffset];
                    Node rightNode = grid[checkLocation + rightOffset];

                    Debug.Log($"Left Node at {checkLocation + leftOffset}");
                    Debug.Log($"Right Node at {checkLocation + rightOffset}");
                }
            }
        }

        return spawnNodes;
    }

    bool CheckNode(Node node, Vector2Int direction)
    {
        Vector2Int oppositeDirection = new Vector2Int(-direction.x, -direction.y);

        bool canSpawn = false;

        return canSpawn;
    }
}
