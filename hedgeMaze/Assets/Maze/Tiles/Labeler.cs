using TMPro;
using UnityEditor;
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

    private void Update()
    {
        DisplayLabel();
    }



    void DisplayLabel()
    {
        if (!Application.isPlaying)
        {
            label.alpha = 1f;
        }
        else if(Application.isPlaying)
        {
            label.alpha = 0f;
        }

        if (!gridManager)
            return;
        label.text = $"{node.coords.x}, {node.coords.y}";
    }
}
