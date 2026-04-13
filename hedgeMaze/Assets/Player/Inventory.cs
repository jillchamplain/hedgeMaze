using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class Inventory : MonoBehaviour
{
    [HideInInspector] public static Inventory instance;
    [SerializeField] List<Tool> tools = new List<Tool>();
    [SerializeField] Tool curToolEquipped;

    private void Start()
    {
        if(instance == null)
            instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UseCurrrentTool(Interaction.instance.CheckRaycast());
        }
    }
    void UnEquipTool()
    {
        foreach (Tool tool in tools)
        {
            tool.isEquipped = false;
        }
        curToolEquipped = null;
    }
    void EquipTool(string nameOfTool)
    {
        foreach (Tool tool in tools)
        {
            if (tool.name == nameOfTool)
            {
                tool.isEquipped = true;
                curToolEquipped = tool;
            }
            else
            {
                tool.isEquipped = false;
            }
        }
    }

    void UseCurrrentTool(GameObject hitObject)
    {
        if (curToolEquipped && hitObject != null)
            curToolEquipped.Use(hitObject);
    }
}
