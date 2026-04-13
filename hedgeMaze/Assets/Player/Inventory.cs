using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class Inventory : MonoBehaviour
{
    [HideInInspector] public static Inventory instance;
    [SerializeField] List<Tool> tools = new List<Tool>();
    [SerializeField] Tool curToolEquipped = null;
    [SerializeField] int curToolIndex = -1;

    private void Start()
    {
        if(instance == null)
            instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Click");
            UseCurrrentTool(Interaction.instance.CheckRaycast());
        }


        else if (Input.GetMouseButton(0))
        {
            UseCurrrentTool(Interaction.instance.CheckRaycast());
            //Debug.Log("Hold Down");
        }

        float scrollValue = Input.GetAxis("Mouse ScrollWheel");

        if (scrollValue > 0f)
        {
            if (scrollValue == -1)
                EquipTool(tools.Count - 1);
            else if (curToolIndex == tools.Count - 1)
            {
                //Debug.Log("scrolled up on last tool");
                EquipTool(0);
            }
            else
                EquipTool(curToolIndex + 1);
        }

        else if (scrollValue < 0f)
        {
            if (scrollValue == -1)
                EquipTool(0);
            else if (curToolIndex == 0)
            {
                //Debug.Log("scrolled down on first tool");
                EquipTool(tools.Count - 1);
            }
            else
                EquipTool(curToolIndex - 1);
        }
    }

    void UnEquipTool()
    {
        foreach (Tool tool in tools)
        {
            tool.isEquipped = false;
        }
        curToolEquipped = null;
        curToolIndex = -1;
    }

    void EquipTool(int index)
    {
        for(int i =  0; i < tools.Count; i++)
        {
            if (i == index)
            {
                tools[i].isEquipped = true;
                curToolEquipped = tools[i];
                curToolIndex = i;
            }
            else
                tools[i].isEquipped = false;
        }
    }


    void EquipTool(string nameOfTool)
    {
        for(int i = 0; i < tools.Count; i++)
        {
            if (tools[i].name == nameOfTool)
            {
                tools[i].isEquipped = true;
                curToolEquipped = tools[i];
                curToolIndex = i;
            }
        }
    }

    void UseCurrrentTool(GameObject hitObject)
    {
        if (curToolEquipped && hitObject != null)
            curToolEquipped.Use(hitObject);
    }
}
