using Ubisoft.Systems.Audio;
using UnityEngine;

public class WateringCan : Tool
{
    [SerializeField] float curWaterAmount;
    public float GetWaterAmount() {  return curWaterAmount; }
    [SerializeField] float maxWaterAmount;
    public float GetMaxWaterAmount() { return maxWaterAmount; }
    [SerializeField] float waterDepleteAmount;
    [SerializeField] float waterGivenAmount;
    [SerializeField] float passiveWaterDepleteAmount;
    [SerializeField] float sprintWaterDepleteAmount;
    [Header("References")]
    [SerializeField] Movement playerMovement;
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem waterParticleSystem;
    [SerializeField] SoundStreamSO wateringNoise;
    [SerializeField] GameObject refillCollider;
    
    public bool isRefilling = false;
    bool isWatering = false;
    bool shouldSprintDeplete = false;
    void SetSprintDeplete(bool newSprintDeplete) {  shouldSprintDeplete = newSprintDeplete;}

    void Start()
    {
        curWaterAmount = maxWaterAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        animator.SetBool("isWatering", isWatering);
        animator.SetBool("isRefilling", isRefilling);
    }

    private void FixedUpdate()
    {
        if (!playerMovement.GetSprinting())
        {
            PassiveDepleteWater();
        }
        else
        {
            SprintDepleteWater();
        }
    }

    public override void AddedToInventory()
    {
        refillCollider.SetActive(false);
        isRefilling = false;
        isWatering = false;
    }

    public override void RemovedFromInventory()
    {
        refillCollider.SetActive(true);
        isWatering = false;
    }

    public override void Equip()
    {

    }

    public override void UnEquip()
    {
        isWatering = false;
        waterParticleSystem.Stop();
    }

    public override void StopUse()
    {
        //Debug.Log("Stopping particle system");
        isWatering = false;
        waterParticleSystem.Stop();
    }

    public override void Use(GameObject hitObject)
    {
        //If we hit a flower, water
        if (hitObject.GetComponent<Flower>())
        {
            if (!isWatering)
            {
                AudioManager.instance.PlayAudio(new AudioRequest(wateringNoise).SetLooping(true));
                Debug.Log("Play my audios");
            }


            isWatering = true;
            Water();
            if(curWaterAmount > 0)
                hitObject.GetComponent<Flower>().Water(waterGivenAmount);
            hitObject.GetComponentInParent<FlowerPatch>().CheckIfWatered();
        }

        //If we hit a fountain, place it there
        else if(hitObject.GetComponentInParent<Fountain>())
        {
            Refill(hitObject.GetComponentInParent<Fountain>());
        }
    }

    public void Water()
    {
        //Debug.Log($"water particle system {waterParticleSystem.isPlaying}");
        if (curWaterAmount > 0)
        {
            Debug.Log("Playing particle system");
            waterParticleSystem.Play();
        }

        curWaterAmount -= waterDepleteAmount * Time.deltaTime;
        if (curWaterAmount < 0)
        {
            waterParticleSystem.Stop();
            curWaterAmount = 0;
        }

    }

    public void PassiveDepleteWater()
    {
        curWaterAmount -= passiveWaterDepleteAmount * Time.deltaTime;
        if (curWaterAmount < 0)
        {
            curWaterAmount = 0;
            waterParticleSystem.Stop();
        }
    }

    public void SprintDepleteWater()
    {
        curWaterAmount -= sprintWaterDepleteAmount * Time.deltaTime;
        if (curWaterAmount < 0)
        {
            curWaterAmount = 0;
            waterParticleSystem.Stop();
        }
    }

    public void Refill(Fountain theFountain)
    {
        Debug.Log("Trying to refill");
        if (isRefilling)
            return;

        isRefilling = true;
        
        //Parent to the fountain and let the player interact with it to pick it up again
        this.transform.parent = theFountain.wateringCanHolder.transform;
        this.transform.position = theFountain.wateringCanHolder.transform.position;
        refillCollider.SetActive(true);

        curWaterAmount += theFountain.refillAmount * Time.deltaTime;
        if (curWaterAmount > maxWaterAmount)
            curWaterAmount = maxWaterAmount;


        //Remove tool from tool list so player has to grab it to reequip
        Inventory.instance.RemoveTool(this);
    }

    public void StopRefill()
    {
        isRefilling = false;
        refillCollider.SetActive(false);
        Inventory.instance.EquipTool("WateringCan");
    }

}
