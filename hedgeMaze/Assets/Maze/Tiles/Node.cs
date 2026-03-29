using UnityEngine;

<<<<<<< Updated upstream
public class Node 
{
    [SerializeField] Vector2Int coords;
=======
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
    [SerializeField] ENodeType type;
    GridManager gridManager;
>>>>>>> Stashed changes
    public Node(Vector2Int coords)
    {
        this.coords = coords;
    }

    private void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
    }

    private void Update()
    {
        UpdateCoords();
    }

    void UpdateCoords()
    {
        coords.x = Mathf.RoundToInt(transform.position.x / gridManager.unityGridSize);
        coords.y = Mathf.RoundToInt(transform.position.z / gridManager.unityGridSize);
    }
}
