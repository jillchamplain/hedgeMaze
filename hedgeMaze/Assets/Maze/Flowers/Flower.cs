using UnityEditor.SceneManagement;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public bool isWatered;
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

    public void Water(float waterAmount)
    {
        waterValue += waterAmount;
        if (waterValue > waterRequirement)
            isWatered = true;
    }
}
