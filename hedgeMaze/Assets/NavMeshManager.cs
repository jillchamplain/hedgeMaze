using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshManager : MonoBehaviour
{
    public static NavMeshManager instance;
    [SerializeField] NavMeshSurface navMeshSurfaceRef;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            BakeNavmesh();
        }
    }
    public void BakeNavmesh()
    {
        navMeshSurfaceRef.BuildNavMesh();
    }
}
