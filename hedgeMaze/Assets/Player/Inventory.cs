using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Inventory : MonoBehaviour
{
    [HideInInspector] public static Inventory instance;
    [SerializeField] List<Tool> tools = new List<Tool>();
    [SerializeField] float toolChangeBufferTime;
    [SerializeField] float toolStopBufferTime;
    public bool usingTool = false;
    bool canChangeTool = true;
    [SerializeField] Tool curToolEquipped;
    int curToolIndex = 0;

    private void Start()
    {
        if(instance == null)
            instance = this;
    }

    private void Update()
    {
        UseTool();

        ChangeTool();
    }

    void UseTool()
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
        else if (Input.GetMouseButtonUp(0))
        {
            curToolEquipped.StopUse();
            usingTool = false;

        }
    }

    void ChangeTool()
    {
        if (!canChangeTool)
            return;

        float scrollValue = Input.GetAxis("Mouse ScrollWheel");

        if (scrollValue > 0f)
        {
            if (scrollValue == -1)
            {
                EquipTool(tools.Count - 1);
                StartCoroutine(ChangeToolTimer());
            }
            else if (curToolIndex == tools.Count - 1)
            {
                EquipTool(0);
                StartCoroutine(ChangeToolTimer());
            }
            else
            {
                EquipTool(curToolIndex + 1);
                StartCoroutine(ChangeToolTimer());
            }
        }

        else if (scrollValue < 0f)
        {
            if (scrollValue == -1)
            {
                EquipTool(0);
            }
            else if (curToolIndex == 0)
            {
                EquipTool(tools.Count - 1);
                StartCoroutine(ChangeToolTimer());
            }
            else
            {
                EquipTool(curToolIndex - 1);
                StartCoroutine(ChangeToolTimer());
            }
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
                tools[i].model.SetActive(true);
            }
            else
            {
                tools[i].isEquipped = false;
                tools[i].model.SetActive(false);
                tools[i].UnEquip();
            }
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
                tools[i].model.SetActive(true);
            }
            else
            {
                tools[i].isEquipped = false;
                tools[i].model.SetActive(false);
                tools[i].UnEquip();
            }
        }
    }

    void UseCurrrentTool(GameObject hitObject)
    {
        if (curToolEquipped && hitObject != null)
        {
            usingTool = true;
            curToolEquipped.Use(hitObject);
        }
        else if (hitObject == null)
        {
            curToolEquipped.StopUse();
            usingTool = false;
        }
    }

    IEnumerator ChangeToolTimer()
    {
        canChangeTool = false;
        yield return new WaitForSeconds(toolChangeBufferTime);
        canChangeTool = true;
        
    }

    IEnumerator ToolStopTimer()
    {
        yield return new WaitForSeconds(toolStopBufferTime);

        usingTool = false;
    }
}
