using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Stalker : MonoBehaviour
{
    [SerializeField] GridManager gridManager;
    [SerializeField] Vector2Int directionToPlayer;
    [SerializeField] Vector2Int gridPosition;
    [SerializeField] Vector2Int spawnPosition;
    [SerializeField] Vector2Int gridPositionOffset;
    [SerializeField] Transform hand;
    [SerializeField] float handSpeed;
    Transform player;
    bool canMoveHand;
    [Header("Tilt")]
    [SerializeField] float rotationAmount;
    [SerializeField] float rotationDuration;
    [SerializeField] Transform visuals;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        StartCoroutine(Lean());
    }

    IEnumerator Lean()
    {
        yield return null;
        float result = GetLeanResult(directionToPlayer, spawnPosition);

        float elapsed = 0;
        Vector3 endRotation = new Vector3(0,0, result * rotationAmount);
        while (elapsed < rotationDuration)
        {
            visuals.transform.localEulerAngles = Vector3.Lerp(Vector3.zero, endRotation, elapsed/rotationDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        visuals.transform.localEulerAngles = endRotation;
        canMoveHand = true;
    }

    float GetLeanResult(Vector2 direction, Vector2 spawnPos)
    {
        if (direction.x != 0) return Mathf.Sign(direction.x) * Mathf.Sign(spawnPos.y);
        if (direction.y != 0) return -Mathf.Sign(direction.y) * Mathf.Sign(spawnPos.x);
        return 0;
    }


    public Vector2Int GetDirectionToPlayer() {  return directionToPlayer; }
    public void SetDirectionToPlayer(Vector2Int newDirection)
    {
        directionToPlayer = newDirection;

        gameObject.transform.rotation = Quaternion.identity;

        if(directionToPlayer.x == -1)
        {
            gameObject.transform.Rotate(new Vector3(0, -90, 0));
        }
        else if(directionToPlayer.x == 1)
        {
            gameObject.transform.Rotate(new Vector3(0, 90, 0));
        }

        if(directionToPlayer.y == -1)
        {
            gameObject.transform.Rotate(new Vector3(0, 180, 0));
        }
        else if( directionToPlayer.y == 1)
        {
            gameObject.transform.Rotate(new Vector3(0, 0, 0));
        }
    }

    public void SetSpawnPosition(Vector2Int newSpawnPosition)
    {  spawnPosition = newSpawnPosition;
    }

    public void SetGridOffset()
    {
        //Get direction to player 

        //Based off direction, get LEFT OR RIGHT of spawn positon

        if (directionToPlayer == Vector2Int.left || directionToPlayer == Vector2Int.right)
        {
            gridPositionOffset = new Vector2Int(0, spawnPosition.y);
        }
        else if(directionToPlayer == Vector2Int.up || directionToPlayer == Vector2Int.down)
        {
            gridPositionOffset = new Vector2Int(spawnPosition.x, 0);
        }
        
    }
    void Start()
    {
        gridManager = FindFirstObjectByType<GridManager>();
    }

    // Update is called twice per frame
    void Update()
    {
        gridPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
        LookForPlayer();
        MoveHand();
    }

    void MoveHand()
    {
        if (!canMoveHand) { return; }
        hand.position = Vector3.MoveTowards(hand.position, player.position, handSpeed * Time.deltaTime);
    }

    bool LookForPlayer()
    {
        //Look in opposite direction down grid and check if player position equals any of them

        List<Node> lookNodes = new List<Node>();
        Dictionary<Vector2Int, Node> grid = gridManager.Grid;

        for(int i = 0; i < gridManager.gridSize.x; i++)
        {
            Node theNode; 

            Vector2Int lookLocationOffset = new Vector2Int(directionToPlayer.x * i, directionToPlayer.y * i);
            Vector2Int lookLocation = gridPosition + lookLocationOffset + gridPositionOffset;
            ;

            //If within bounds of grid
            if ((lookLocation.x - 1 >= 0 && lookLocation.x + 1 < gridManager.gridSize.x) && (lookLocation.y - 1 >= 0 && lookLocation.y + 1 < gridManager.gridSize.y))
            {
                theNode = grid[lookLocation];
                if (theNode.coords == gridManager.GetPlayerGridPosition())
                {
                    Debug.Log($"Can see player at {theNode.coords}");
                    return true;
                }
            }
        }
        Debug.Log("Cannot find player");
        Destroy(gameObject);
        return false;
    }
}
