using TMPro;
using UnityEngine;
[ExecuteAlways]
public class Labeler : MonoBehaviour
{
    TextMeshPro label;
    GridManager gridManager;
    Node node;

    private void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        label = GetComponentInChildren<TextMeshPro>();
        node = GetComponent<Node>();
    }

    private void Start()
    {
        label.alpha = 0f;
    }

    private void Update()
    {
        DisplayLabel();
    }

    void DisplayLabel()
    {
        if (!gridManager)
            return;
        label.text = $"{node.coords.x}, {node.coords.y}";
    }
}
