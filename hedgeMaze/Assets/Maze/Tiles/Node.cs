using UnityEngine;

public enum ENodeType
{
    NO_HEDGE,
    HEDGE,
    NUM_TYPES
}


[ExecuteAlways]
public class Node : MonoBehaviour 
{
    [SerializeField] public Vector2Int coords = Vector2Int.zero;
    Vector3 lastPos;
    [SerializeField] ENodeType type;
    public void ChangeTypeTo(ENodeType newType)
    {
        type = newType;
        switch (type)
        {
            case ENodeType.NO_HEDGE:
                hedge.SetActive(false);
                break;
            case ENodeType.HEDGE:
                hedge.SetActive(true);
                break;
        }

    }
    GameObject hedge;
    GridManager gridManager;
    public Node(Vector2Int coords)
    {
        this.coords = coords;
    }

    private void OnValidate()
    {
        ChangeTypeTo(type);
    }

    private void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        hedge = GameObject.Find("Hedge");
        UpdateCoords();
    }

    private void Update()
    {
            UpdateCoords();
    }

    private void LateUpdate()
    {
        lastPos = transform.position;
    }

    void UpdateCoords()
    {
        coords.x = Mathf.RoundToInt(transform.position.x / gridManager.unityGridSize);
        coords.y = Mathf.RoundToInt(transform.position.z / gridManager.unityGridSize);
        gameObject.name = $"Tile {coords.x}_{coords.y}";
    }
}
