using UnityEngine;

public class Shears : Tool
{
    [SerializeField] float timeToCut;
    [SerializeField] Animator animator;
    bool isCutting;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isCutting", isCutting);
    }

    private void LateUpdate()
    {
        isCutting = false;
    }

    public override void Equip() { }
    public override void UnEquip() { }
    public override void Use(GameObject hitObject)
    {
        //Debug.Log(hitObject);
        if(hitObject != null && hitObject.GetComponentInParent<Node>())
        {
            isCutting = true;
            Node theNode = hitObject.GetComponentInParent<Node>();
            theNode.Cut();
        }
    }

}
