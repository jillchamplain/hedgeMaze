using TMPro;
using UnityEngine;
[ExecuteAlways]
public class Labeler : MonoBehaviour
{
    TextMeshPro label;
    GridManager gridManager;
    Vector2Int coords = new Vector2Int();

    private void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        label = GetComponentInChildren<TextMeshPro>();
    }

    private void Update()
    {
        DisplayLabel();
    }

    void DisplayLabel()
    {
        if (!gridManager)
            return;
        coords.x = Mathf.RoundToInt(transform.position.x / gridManager.unityGridSize);
        coords.y = Mathf.RoundToInt(transform.position.z / gridManager.unityGridSize);

        label.text = $"{coords.x}, {coords.y}";
    }
}
