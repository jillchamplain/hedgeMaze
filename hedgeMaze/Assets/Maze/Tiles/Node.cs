using UnityEngine;

public class Node 
{
    [SerializeField] Vector2Int coords;
    public Node(Vector2Int coords)
    {
        this.coords = coords;
    }
}
