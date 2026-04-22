using DG.Tweening;
using Ubisoft.Systems.Audio;
using UnityEngine;

public class Shears : Tool
{
    [SerializeField] float timeToCut;
    [SerializeField] Animator animator;
    bool isCutting;
    Node targetNode;
    [SerializeField] SoundStreamSO cuttingNoise;
    [SerializeField] SoundStreamSO hedgeNoise;

    void Start()
    {
        
    }


    private void LateUpdate()
    {
        if (isActiveAndEnabled)
        {
            animator.SetBool("isCutting", isCutting);
            isCutting = false;
        }
    }

    public override void Equip() { }

    public override void AddedToInventory()
    {
        isCutting = false;
    }

    public override void RemovedFromInventory()
    {
        isCutting = false;
    }
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
        AudioManager.instance.PlayAudio(new AudioRequest(cuttingNoise).SetPoint(targetNode.transform.position));
        AudioManager.instance.PlayAudio(new AudioRequest(hedgeNoise).SetPoint(targetNode.transform.position));

        targetNode.Cut();
    }

}
