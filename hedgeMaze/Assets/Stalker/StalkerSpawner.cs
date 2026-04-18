using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
public class StalkerSpawner : MonoBehaviour
{
    GridManager gridManager;

    [SerializeField] List<Vector2> spawnIntervalsPerFlowersWatered = new List<Vector2>();
    float timeUntilStalker;
    float stalkerTimer;
    [SerializeField] float toolActiveMultiplier = 3;
    [Header("References")]
    [SerializeField] Vector2Int originPos;
    [SerializeField] GameObject stalkerPrefab;
    Stalker stalker;
    bool isUsingTool;
    void Start()
    {
        gridManager = FindFirstObjectByType<GridManager>();
    }

    private void OnEnable()
    {
        GameManager.instance.onWaterFlower += GenerateNewStalkerTime;
    }

    private void OnDisable()
    {
        GameManager.instance.onWaterFlower -= GenerateNewStalkerTime;
    }

    void Update()
    {
        originPos = gridManager.GetPlayerGridPosition();

        isUsingTool = Inventory.instance.usingTool;

        if (GameManager.instance.flowersWatered != 0 && stalker == null)
        {
            if (isUsingTool)
            {
                stalkerTimer += Time.deltaTime * toolActiveMultiplier;
            }
            else
            {
                stalkerTimer += Time.deltaTime;
            }


            Debug.Log(stalkerTimer.ToString());

            if (stalkerTimer > timeUntilStalker)
            {
                GenerateNewStalkerTime();
                SpawnStalker();
                stalkerTimer = 0;
            }
        }
    }

    void GenerateNewStalkerTime()
    {
        float percentage;
        if (timeUntilStalker != 0)
        {
            percentage = stalkerTimer / timeUntilStalker;
        }
        else
        {
            percentage = 0;
        }

        // Set stalker time to a new value that is of the same ratio it was away from the previous time until stalker
        timeUntilStalker = Random.Range(spawnIntervalsPerFlowersWatered[GameManager.instance.flowersWatered].x, spawnIntervalsPerFlowersWatered[GameManager.instance.flowersWatered].y);
        stalkerTimer = Mathf.Lerp(0, timeUntilStalker, percentage);
        Debug.Log(timeUntilStalker);
    }


    void SpawnStalker()
    {
        Debug.Log("Spawning stalker...");
        List<Node> spawnableTiles = GetSpawnableTiles();
        
        foreach(Node tile in spawnableTiles)
        {
            //Debug.Log(tile);
        }


        if(spawnableTiles.Count <= 0)
        {
            Debug.Log("Nowhere for stalker to spawn!");
            return;
        }

        int randIndex = Random.Range(0, spawnableTiles.Count);

        Node spawnTile = spawnableTiles[randIndex];
        Vector2Int spawnDirection = GetDirectionOfSpawnableTile(spawnTile);

        Vector2Int newSpawnDirection = new Vector2Int(-spawnDirection.x, -spawnDirection.y); //Reverse direction of search direction
        Vector2Int stalkerDirectionToPlayer = newSpawnDirection;

        Debug.Log($"Spawning stalker at {spawnTile.coords}");

        int leftRightPos = 0;

        //Adjust for if left or right
        if (newSpawnDirection.x == 0)
        {
            if (spawnTile.coords.x < originPos.x)
                leftRightPos = 1;
            else
                leftRightPos = -1;

                newSpawnDirection.x = leftRightPos;
        }
        else if (newSpawnDirection.y == 0)
        {
            if (spawnTile.coords.y < originPos.y)
                leftRightPos = 1;
            else
                leftRightPos = -1;
            newSpawnDirection.y = leftRightPos;
        }

        /*Debug.Log($"Spawning Stalker at {spawnableTiles[randIndex].gameObject}");
        Debug.Log($"Spawned in direction {spawnDirection}");
        Debug.Log($"Spawn position to use is {newSpawnDirection}");*/

        GameObject theStalker = Instantiate(stalkerPrefab, spawnTile.GetSpawnPositionAt(newSpawnDirection).gameObject.transform.position, Quaternion.identity);
        theStalker.GetComponent<Stalker>().SetSpawnPosition(spawnTile.GetSpawnPositionAt(newSpawnDirection).direction);
        theStalker.GetComponent<Stalker>().SetDirectionToPlayer(stalkerDirectionToPlayer);
        theStalker.GetComponent<Stalker>().SetGridOffset();

        stalker = theStalker.GetComponent<Stalker>();
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

        int xStartOffset = 0;
        int yStartOffset = 0;

        /*if (direction.x != 0)
            xStartOffset = direction.x;
        if (direction.y != 0)
            yStartOffset = direction.y;*/

        //Debug.Log($"{xStartOffset}:{yStartOffset}");

        //Iterate through dictionary keys in direction
        for (int i = 1; i < gridManager.gridSize.x; i++)
        {
            Node theNode;
            
            Vector2Int checkLocationOffset = new Vector2Int(direction.x * i + xStartOffset, direction.y * i + yStartOffset);
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

                    if (i == 1)
                        continue;
                    if (CheckNodeForSpawn(leftNode, direction))
                    {
                        spawnNodes.Add(leftNode);
                    }
                    if (CheckNodeForSpawn(rightNode, direction))
                    {
                        spawnNodes.Add(rightNode);
                    }
                }
                else if (theNode.type == ENodeType.HEDGE)
                {
                    //Debug.Log("Breaking for hedge");
                    break;
                }
            }
            else
            {
                //Debug.Log("Breaking for end of grid");
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
            //Debug.Log($"Node at {node.coords}");
        }

        return canSpawn;
    }

}
