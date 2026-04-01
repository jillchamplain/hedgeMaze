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
    [SerializeField] public List<GameObject> spawnPositions;

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
        switch (type)
        {
            case ENodeType.NONE:
                hedge.SetActive(false);
                root.SetActive(false);
                break;
            case ENodeType.HEDGE:
                hedge.SetActive(true);
                root.SetActive(false);
                break;
            case ENodeType.ROOT:
                root.SetActive(true);
                hedge.SetActive(false);
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
