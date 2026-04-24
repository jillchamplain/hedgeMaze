using UnityEngine;

public class Hand : Tool
{
    [SerializeField] public Tool replacedTool;
    public override void Use(GameObject hitObject)
    {
        //Debug.Log($"Hand interacted with {hitObject}");
        if(hitObject.GetComponentInParent<Fountain>())
        {
            hitObject.GetComponentInParent<Fountain>().Pump();
        }
    }

    public override void StopUse()
    {
       
    }

    public override void Equip()
    {
            
    }

    public override void UnEquip()
    {
        
    }

    public override void AddedToInventory()
    {
        
    }

    public override void RemovedFromInventory()
    {
        
    }
}
