using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
public class FlowerPatch : MonoBehaviour
{
    List<Flower> flowers = new List<Flower>();

    private void Awake()
    {
        ManageFlowers();
    }
    private void OnValidate()
    {
        
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
