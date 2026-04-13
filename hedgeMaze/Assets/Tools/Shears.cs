using UnityEngine;

public class Shears : Tool
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Equip() { }
    public override void UnEquip() { }
    public override void Use(GameObject hitObject)
    {
        Debug.Log(hitObject);
        if(hitObject != null && hitObject.GetComponentInParent<Node>())
        {
            Node theNode = hitObject.GetComponentInParent<Node>();
            theNode.Cut();
        }
    }

    public void Cut()
    {

    }
}
