using UnityEngine;

public class Flower : MonoBehaviour
{
    public bool isWatered;
    [SerializeField] float waterValue;
    [SerializeField] float waterRequirement;
    [SerializeField] float waterHeightIncrease;
    [Header("References")]
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material waterStemMaterial;
    [SerializeField] Material waterBudMaterial;
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
        if (isWatered)
            return;
        waterValue += waterAmount * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y + (waterHeightIncrease * Time.deltaTime), transform.position.z);
        if (waterValue > waterRequirement)
        {
            isWatered = true;
            Material[] newMaterials = new Material[2];
            newMaterials[0] = waterStemMaterial;
            newMaterials[1] = waterBudMaterial;

            meshRenderer.sharedMaterials = newMaterials;

            Debug.Log($"Changing materials {isWatered}");
        }
    }
}
