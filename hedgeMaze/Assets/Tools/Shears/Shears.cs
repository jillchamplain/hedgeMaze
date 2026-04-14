using DG.Tweening;
using UnityEngine;

public class Shears : Tool
{
    [SerializeField] float timeToCut;
    [SerializeField] Animator animator;
    bool isCutting;
    Node targetNode;
    void Start()
    {
        
    }


    private void LateUpdate()
    {
        animator.SetBool("isCutting", isCutting);
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
            targetNode = hitObject.GetComponentInParent<Node>();
        }
    }

    public override void StopUse()
    {
        
    }

    public void CutEffects()
    {
        targetNode.SpawnLeafParticle();
        targetNode.transform.DOShakePosition(0.15f, 0.02f, 30);
        targetNode.Cut();
    }

}
