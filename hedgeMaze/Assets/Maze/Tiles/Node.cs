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
    GridManager gridManager;
    public Node(Vector2Int coords)
    {
        this.coords = coords;
    }

    private void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        UpdateCoords();
    }

    private void Update()
    {
        if(transform.position != lastPos)
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
    }
}
