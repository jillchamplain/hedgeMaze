using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using Ubisoft.Systems.Audio;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
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
    bool isMoving;
    [Header("Tilt")]
    [SerializeField] float rotationAmount;
    [SerializeField] float rotationDuration;
    [SerializeField] float rotateBackDuration;
    [SerializeField] Transform visuals;

    [Header("Movement")]
    [SerializeField] NavMeshAgent agent;
    Volume postProccessingVolume;

    [Header("Despawning")]
    [Tooltip("This is the maximum and starting attention")]
    [SerializeField] float maxAttention;
    [Tooltip("When seeing the player, this is the amount attention drops per second")]
    [SerializeField] float attentionGrowth;
    [Tooltip("When seeing the player, this is the amount attention drops per second")]
    [SerializeField] float attentionGrowthWhenLeaning;
    [Tooltip("When not seeing the player, this is the amount attention drops per second")]
    [SerializeField] float attentionLoss;
    [SerializeField] LayerMask sightLayers;
    float attention;
    [SerializeField] SoundStreamSO chaseNoise;
    [SerializeField] SoundStreamSO spawnNoise;
    [SerializeField] SoundStreamSO footsteps;
    [SerializeField] SoundStreamSO neckCrack;
    [SerializeField] SoundStreamSO riser;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        AudioManager.instance.PlayAudio(new AudioRequest(spawnNoise).SetPoint(transform.position));
        StartCoroutine(Lean());
    }

    IEnumerator Lean()
    {
        yield return null;
        float result = GetLeanResult(directionToPlayer, spawnPosition);

        float elapsed = 0;
        Vector3 endRotation = new Vector3(0, 0, result * rotationAmount);
        while (attention < maxAttention)
        {
            visuals.transform.localEulerAngles = Vector3.Lerp(Vector3.zero, endRotation, attention / maxAttention);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isMoving = true;
        AudioManager.instance.PlayAudio(new AudioRequest(chaseNoise).SetPoint(transform.position));

        elapsed = 0;
        while (elapsed < rotateBackDuration)
        {
            visuals.transform.localEulerAngles = Vector3.Lerp(endRotation, Vector3.zero, elapsed / rotateBackDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        visuals.transform.localEulerAngles = Vector3.zero;
    }

    float GetLeanResult(Vector2 direction, Vector2 spawnPos)
    {
        if (direction.x != 0) return Mathf.Sign(direction.x) * Mathf.Sign(spawnPos.y);
        if (direction.y != 0) return -Mathf.Sign(direction.y) * Mathf.Sign(spawnPos.x);
        return 0;
    }


    public Vector2Int GetDirectionToPlayer() { return directionToPlayer; }
    public void SetDirectionToPlayer(Vector2Int newDirection)
    {
        directionToPlayer = newDirection;

        gameObject.transform.rotation = Quaternion.identity;

        if (directionToPlayer.x == -1)
        {
            gameObject.transform.Rotate(new Vector3(0, -90, 0));
        }
        else if (directionToPlayer.x == 1)
        {
            gameObject.transform.Rotate(new Vector3(0, 90, 0));
        }

        if (directionToPlayer.y == -1)
        {
            gameObject.transform.Rotate(new Vector3(0, 180, 0));
        }
        else if (directionToPlayer.y == 1)
        {
            gameObject.transform.Rotate(new Vector3(0, 0, 0));
        }
    }

    [SerializeField] float footstepCooldown;
    float footstepTimer;
    void Sound()
    {
        footstepTimer += Time.deltaTime;
        if (footstepTimer > footstepCooldown)
        {
            AudioManager.instance.PlayAudio(new AudioRequest(footsteps).SetPoint(transform.position));
            footstepTimer = 0f;
        }
    }


    public void SetSpawnPosition(Vector2Int newSpawnPosition)
    { spawnPosition = newSpawnPosition;
    }

    public void SetGridOffset()
    {
        //Get direction to player 

        //Based off direction, get LEFT OR RIGHT of spawn positon

        if (directionToPlayer == Vector2Int.left || directionToPlayer == Vector2Int.right)
        {
            gridPositionOffset = new Vector2Int(0, spawnPosition.y);
        }
        else if (directionToPlayer == Vector2Int.up || directionToPlayer == Vector2Int.down)
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
        if (GameManager.instance.hasLost)
        {
            return;
        }

        gridPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));

        if (isMoving)
        {
            ManageAttention();
            Move();
            KillPlayer();
        }
        else
        {
            ManageSightWhenLeaning();
        }
        attention = Mathf.Clamp(attention, 0, maxAttention);
    }

    void Move()
    {
        if (isMoving)
            agent.destination = player.position;
        Sound();
    }

    void ManageSightWhenLeaning()
    {
        //Look in opposite direction down grid and check if player position equals any of them

        List<Node> lookNodes = new List<Node>();
        Dictionary<Vector2Int, Node> grid = gridManager.Grid;

        for (int i = 0; i < gridManager.gridSize.x; i++)
        {
            Node theNode;

            Vector2Int lookLocationOffset = new Vector2Int(directionToPlayer.x * i, directionToPlayer.y * i);
            Vector2Int lookLocation = gridPosition + lookLocationOffset + gridPositionOffset;
            

            //If within bounds of grid
            if ((lookLocation.x - 1 >= 0 && lookLocation.x + 1 < gridManager.gridSize.x) && (lookLocation.y - 1 >= 0 && lookLocation.y + 1 < gridManager.gridSize.y))
            {
                theNode = grid[lookLocation];
                if (theNode.coords == gridManager.GetPlayerGridPosition())
                {
                    attention += Time.deltaTime * attentionGrowthWhenLeaning;
                    return;
                }
            }
        }

        RaycastHit hit;
        Vector3 lookDirection = (player.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, lookDirection, out hit, Mathf.Infinity, sightLayers))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                
                attention += Time.deltaTime * attentionGrowthWhenLeaning;
                return;
            }
        }


        attention -= Time.deltaTime * attentionLoss;

        if (attention <= 0)
        {
            Destroy(gameObject);
        }
    }

    void ManageAttention()
    {
        RaycastHit hit;
        Vector3 lookDirection = (player.position - transform.position).normalized;

        if (Physics.Raycast(transform.position, lookDirection, out hit, Mathf.Infinity, sightLayers))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                attention += Time.deltaTime * attentionGrowth;
            }
            else
            {
                attention -= Time.deltaTime * attentionLoss;
            }
        }

        if (attention <= 0)
        {
            isMoving = false;
            Destroy(gameObject);
        }
    }


    void KillPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < 1 && !GameManager.instance.hasLost)
        {
            GameManager.instance.PlayerDeath();
            isMoving = false;
            agent.isStopped = true;
            StartCoroutine(PlayerDeathAnimation());
        }
    }

    IEnumerator MakeRed()
    {
        postProccessingVolume = GameObject.FindGameObjectWithTag("Volume").GetComponent<Volume>();
        postProccessingVolume.profile.TryGet<ColorAdjustments>(out var colorAdjustments);
        float elapsed = 0;
        while (elapsed < 0.8f)
        {
            colorAdjustments.colorFilter.value = Color.Lerp(Color.white, Color.red, elapsed / 0.8f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        colorAdjustments.colorFilter.value = Color.red;

    }

    IEnumerator PlayerDeathAnimation()
    {        
        Transform camera = Camera.main.transform;
        AudioManager.instance.PlayAudio(new AudioRequest(riser).SetPoint(transform.position));

        Vector3 endPosition = transform.position + transform.forward * 0.5f + new Vector3(0, 0.8f, 0);
        Vector3 lookTarget = transform.position + new Vector3(0, 0.8f, 0);

        // Direction as if camera is already at endPosition
        Vector3 directionFromEnd = lookTarget - endPosition;

        Tween raiseTween = camera.DOMove(endPosition, 0.3f);
        Tween twistTween = camera.DOLocalRotate(
            Quaternion.LookRotation(directionFromEnd).eulerAngles,
            0.3f
        );


        yield return twistTween.WaitForCompletion();
        yield return raiseTween.WaitForCompletion();

        transform.DOLookAt(camera.position, 0.1f, AxisConstraint.Y);

        StartCoroutine(MakeRed());

        Tween shakeTween = camera.DOShakePosition(2, new Vector3(0, 0.005f, 0), 28, 90, false, false);
        yield return shakeTween.WaitForCompletion();
        AudioManager.instance.PlayAudio(new AudioRequest(neckCrack).SetPoint(transform.position));
        camera.DOLocalRotate(new Vector3(0, 180, 40) + camera.eulerAngles, 0.2f).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        //Debug.Log("Stalker despawned");

    }
}
