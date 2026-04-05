using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class Inventory : MonoBehaviour
{
    [SerializeField] List<Tool> tools = new List<Tool>();
    Tool curToolEquipped;

    void UnEquipTool()
    {
        foreach(Tool tool in tools)
        {
            tool.isEquipped = false;
        }
        curToolEquipped = null;
    }
    void EquipTool(string nameOfTool)
    {
        foreach(Tool tool in tools)
        {
            if(tool.name == nameOfTool)
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
}
