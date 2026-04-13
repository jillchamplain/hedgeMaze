using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum ENodeType
{
    NONE,
    HEDGE,
    ROOT,
    NUM_TYPES
}


[ExecuteAlways]
public class Node : MonoBehaviour 
{
    [SerializeField] public Vector2Int coords = Vector2Int.zero;
    [SerializeField] public ENodeType type;

    [Serializable]
    public struct SpawnPosition
    {
        [SerializeField] public GameObject gameObject;
        [SerializeField] public Vector2Int direction; 
    }

    [SerializeField] public List<SpawnPosition> spawnPositions;
    public SpawnPosition GetSpawnPositionAt(Vector2Int pos)
    {
        SpawnPosition nullSpawn = new SpawnPosition();
        foreach (SpawnPosition spawnPosition in spawnPositions)
        {
            if(spawnPosition.direction == pos)
                return spawnPosition;
        }
        return nullSpawn;
    }

    [Header("Types")]
    [SerializeField] GameObject hedgeModel;
    [SerializeField] GameObject hedgeParticlePF;
    [SerializeField] GameObject rootModel;

    [Header("Gameplay")]
    [SerializeField] float timeToCut = 8;
    float cutTimer;

    GridManager gridManager;
    public Node(Vector2Int coords)
    {
        this.coords = coords;
    }

    private void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();

        coords.x = Mathf.RoundToInt(transform.position.x / gridManager.unityGridSize);
        coords.y = Mathf.RoundToInt(transform.position.z / gridManager.unityGridSize);
        UpdateCoords();
    }

    private void Update()
    {
        UpdateCoords();
        ChangeTypeTo(type);
    }

    public void EnterNode()
    {

    }

    public void LeaveNode()
    {
        Grow();
    }

    public void Cut()
    {
        if (type == ENodeType.HEDGE)
        {
            cutTimer += Time.deltaTime;
        }

        if (cutTimer > timeToCut)
        {
            ChangeTypeTo(ENodeType.NONE);
            GameObject hedgeParticle = Instantiate(hedgeParticlePF, new Vector3(hedgeModel.transform.position.x, hedgeModel.transform.position.y + 0.4f, hedgeModel.transform.position.z), Quaternion.identity);
            hedgeParticle.GetComponent<Particle>().Play();
            cutTimer = 0;
        }
    }

    public void Grow()
    {
        if(type == ENodeType.ROOT)
            ChangeTypeTo(ENodeType.HEDGE);
    }
    public void ChangeTypeTo(ENodeType newType)
    {
        type = newType;
        //If not in Prefab Mode
        hedgeModel.SetActive(false);
        rootModel.SetActive(false);

        switch (type)
        {
            case ENodeType.NONE:
                break;
            case ENodeType.HEDGE:
                hedgeModel.SetActive(true);
                break;
            case ENodeType.ROOT:
                rootModel.SetActive(true);
                break;
        }
    }

    void UpdateCoords()
    {
        coords.x = Mathf.RoundToInt(transform.position.x / gridManager.unityGridSize);
        coords.y = Mathf.RoundToInt(transform.position.z / gridManager.unityGridSize);
        //If not in Prefab Mode
        if (PrefabStageUtility.GetCurrentPrefabStage() == null)
        {
            gameObject.name = $"Tile {coords.x}_{coords.y}";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(type == ENodeType.ROOT)
        {
            Debug.Log("Root");
        }
    }
}
