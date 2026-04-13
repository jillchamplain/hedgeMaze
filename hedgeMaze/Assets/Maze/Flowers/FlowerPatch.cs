using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
public class FlowerPatch : MonoBehaviour
{
    [SerializeField] List<Flower> flowers = new List<Flower>();
    bool isPatchWatered = false;
    private void Awake()
    {
        ManageFlowers();
    }
    private void OnValidate()
    {
        
    }


    public void CheckIfWatered()
    {
        bool shouldBeWatered = true;

        foreach(Flower flower in flowers)
        {
            if (flower.isWatered == false)
                shouldBeWatered = false;
        }
        isPatchWatered = shouldBeWatered;
    }

    public void ManageFlowers()
    {
        flowers.Clear();
        Flower[] flowersToAdd = GetComponentsInChildren<Flower>();
        foreach(Flower f in flowersToAdd)
        {
            flowers.Add(f);
        }
    }

    public void CleanFlowers()
    {
        List<Flower> flowersToReAdd = new List<Flower>();
        
        foreach(Flower f in flowers)
        {
            if (f.gameObject)
                flowersToReAdd.Add(f);
        }
        flowers = flowersToReAdd;
    }
}
