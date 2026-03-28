using UnityEngine;

public enum ENodetype
{
    HEDGE,
    NO_HEDGE,
    NUM_TYPES
}
public class Node 
{
    [SerializeField] Vector2Int coords;
    [SerializeField] ENodetype type;
    public Node(Vector2Int coords)
    {
        this.coords = coords;
    }
}
