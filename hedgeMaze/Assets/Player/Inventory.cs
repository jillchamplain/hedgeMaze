using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Inventory : MonoBehaviour
{
    [HideInInspector] public static Inventory instance;
    [SerializeField] List<Tool> tools = new List<Tool>();
    [SerializeField] Tool curToolEquipped;
    int curToolIndex = 0;
    public bool usingTool = false;
    public bool tryingToUseTool = false;
    bool canChangeTool = true;

    [SerializeField] float toolChangeBufferTime;
    
    [Header("References")]
    [SerializeField] GameObject toolHolder;
    [SerializeField] Hand handTool;

    private void Start()
    {
        if(instance == null)
            instance = this;
    }

    private void Update()
    {
        UseTool();

        ChangeTool();

        if (tryingToUseTool)
        {
            TryUseCurrentTool(Interaction.instance.CheckRaycast());
        }
    }


    #region Use
    void UseTool()
    {
        if (curToolEquipped) //If has Tool in Hand
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!TryToAddTool(Interaction.instance.CheckRaycast()))
                {
                    tryingToUseTool = true;

                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                curToolEquipped.StopUse();
                usingTool = false;
                tryingToUseTool = false;
            }
        }

        else if(!curToolEquipped)
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryToAddTool(Interaction.instance.CheckRaycast());
            }   
        }
    }

    void TryUseCurrentTool(GameObject hitObject)
    {
        if (curToolEquipped && hitObject != null)
        {
            usingTool = true;
            curToolEquipped.Use(hitObject);
        }
        else if (curToolEquipped && hitObject == null)
        {
            curToolEquipped.StopUse();
            usingTool = false;
        }
    }
    #endregion

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

    public void UnEquipTool()
    {
        foreach (Tool tool in tools)
        {
            tool.isEquipped = false;
        }
        curToolEquipped = null;
        curToolIndex = -1;
    }

    public void RemoveTool(Tool theTool) //When removing a tool from the inventory equip the "Hand Tool"
    {
        int replaceIndex = 0;
        for(int i = 0; i < tools.Count; i++)
        {
            if(tools[i] == theTool)
                replaceIndex = i;
        }
        theTool.RemovedFromInventory();

        tools[replaceIndex] = handTool;
        handTool.replacedTool = theTool;
        handTool.AddedToInventory();
        EquipTool(handTool);
        tools.TrimExcess();
    }

    void AddTool(Tool theTool)
    {
        bool isRepeatTool = false;
        foreach (Tool tool in tools)
        {
            if (tool == theTool)
                isRepeatTool = true;
        }

        if (!isRepeatTool)
        {
            UnEquipTool();
            tools.Add(theTool);
            theTool.AddedToInventory();

            EquipTool(theTool);

            if(handTool.replacedTool == theTool)
            {
                handTool.RemovedFromInventory();
                tools.Remove(handTool);
                tools.TrimExcess();
            }

            theTool.transform.position = toolHolder.transform.position + theTool.toolHoldTransformOffset;
            theTool.transform.parent = toolHolder.transform;
            
            
        }
    }

    bool TryToAddTool(GameObject hitObject)
    {
        //Debug.Log($"Trying to add {hitObject}");
        if (hitObject != null && hitObject.GetComponentInParent<Tool>())
        {
            //Debug.Log($"Trying to add {hitObject}");
            AddTool(hitObject.GetComponentInParent<Tool>());
            return true;
        }
        return false;
    }

    #region Equip
    void EquipTool(Tool theTool)
    {
        curToolEquipped = null;

        foreach(Tool tool in tools)
        {
            if(tool != theTool)
            {
                tool.isEquipped = false;
                tool.model.SetActive(false);
            }
            else
            {
                tool.isEquipped = true;
                curToolEquipped = tool;
                curToolEquipped.model.SetActive(true);
            }
        }
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


    public void EquipTool(string nameOfTool)
    {
        for(int i = 0; i < tools.Count; i++)
        {
            if (tools[i].toolName == nameOfTool)
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
    #endregion

    IEnumerator ChangeToolTimer()
    {
        canChangeTool = false;
        yield return new WaitForSeconds(toolChangeBufferTime);
        canChangeTool = true;
        
    }
}
