using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum ENodeType
{
    NONE,
    HEDGE,
    ROOT,
    FOUNTAIN,
    NUM_TYPES
}


[ExecuteAlways]
public class Node : MonoBehaviour 
{
    [SerializeField] public Vector2Int coords = Vector2Int.zero;
    [SerializeField] public ENodeType type;
    [SerializeField] public List<GameObject> spawnPositions;

    [Header("Type Models")]
    [SerializeField] GameObject hedge;
    [SerializeField] GameObject root;
    [SerializeField] GameObject fountain;
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
        fountain.SetActive(false);

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
            case ENodeType.FOUNTAIN:
                fountain.SetActive(true);
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
