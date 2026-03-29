using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class FlowerPatch : MonoBehaviour
{
    [SerializeField] List<Flower> flowers = new List<Flower>();

    private void Awake()
    {
        Flower[] flowersToAdd = FindObjectsByType(typeof(Flower), FindObjectsSortMode.None) as Flower[];
        foreach(Flower flower in flowersToAdd)
        {
            flowers.Add(flower);
        }    
    }
}
