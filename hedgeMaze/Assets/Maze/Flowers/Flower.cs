using UnityEditor.SceneManagement;
using UnityEngine;

public class Flower : MonoBehaviour
{
    bool isWatered;
    [SerializeField] float waterValue;
    [SerializeField] float waterRequirement;
    FlowerPatch flowerPatch;

    private void Awake()
    {
        flowerPatch = GetComponentInParent<FlowerPatch>();

    }

    private void OnDestroy()
    {
        flowerPatch.ManageFlowers();
    }

    void Water()
    {
        
    }
}
