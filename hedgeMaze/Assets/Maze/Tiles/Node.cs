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

    [Header("Type Models")]
    [SerializeField] GameObject hedge;
    [SerializeField] GameObject root;

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
    public void ChangeTypeTo(ENodeType newType)
    {
        type = newType;
        //If not in Prefab Mode
        hedge.SetActive(false);
        root.SetActive(false);

        switch (type)
        {
            case ENodeType.NONE:
                break;
            case ENodeType.HEDGE:
                hedge.SetActive(true);
                break;
            case ENodeType.ROOT:
                root.SetActive(true);
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
}
