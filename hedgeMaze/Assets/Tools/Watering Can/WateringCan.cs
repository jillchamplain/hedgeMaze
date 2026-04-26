using DG.Tweening;
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
    [SerializeField] AudioClip wateringNoise;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject refillCollider;
    [SerializeField] Fountain parentFountain;
    
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
        if (playerMovement.GetSprinting() && isInInventory)
        {
            SprintDepleteWater();
        }
    }

    public override void AddedToInventory()
    {
        isInInventory = true;
        parentFountain.wateringCan = null;
        parentFountain = null;
        refillCollider.SetActive(false);
        isRefilling = false;
        isWatering = false;
    }

    public override void RemovedFromInventory()
    {
        isInInventory = false;
        isEquipped = false;
        refillCollider.SetActive(true);
        isWatering = false;
    }

    public override void Equip()
    {
        isEquipped = true;
    }

    public override void UnEquip()
    {
        isEquipped = false;
        isWatering = false;
        StopEffects();
    }

    public override void StopUse()
    {
        isWatering = false;
        StopEffects();
    }

    void StopEffects()
    {
        audioSource.DOComplete();
        waterParticleSystem.Stop();
        audioSource.DOFade(0, 0.75f);
    }

    public override void Use(GameObject hitObject)
    {
        //If we hit a flower, water
        if (hitObject.GetComponent<Flower>())
        {
            if (!isWatering)
            {
                audioSource.DOComplete();
                audioSource.DOFade(1, 0.5f);
                audioSource.clip = wateringNoise;
                audioSource.loop = true;
                audioSource.Play();
            }

            isWatering = true;
            Water();
            if (curWaterAmount > 0)
                hitObject.GetComponent<Flower>().Water(waterGivenAmount);

            hitObject.GetComponentInParent<FlowerPatch>().CheckIfWatered();
        }

        //If we hit a fountain, place it there IF NOT ALREADY IN FOUNTAIN
        else if(hitObject.GetComponentInParent<Fountain>() && !parentFountain)
        {
            StartRefill(hitObject.GetComponentInParent<Fountain>());
        }

        else if(hitObject.GetComponentInParent<Fountain>() && parentFountain && isRefilling)
        {
            Refill(hitObject.GetComponentInParent<Fountain>());
        }
    }

    public void Water()
    {
        if (curWaterAmount > 0)
        {
            Debug.Log("Playing particle system");
            waterParticleSystem.Play();
        }

        curWaterAmount -= waterDepleteAmount * Time.deltaTime;
        if (curWaterAmount < 0)
        {
            StopEffects();
            curWaterAmount = 0;
        }
    }

    public void PassiveDepleteWater()
    {
        curWaterAmount -= passiveWaterDepleteAmount * Time.deltaTime;
        if (curWaterAmount < 0)
        {
            curWaterAmount = 0;
            StopEffects();
        }
    }

    public void SprintDepleteWater()
    {
        curWaterAmount -= sprintWaterDepleteAmount * Time.deltaTime;
        if (curWaterAmount < 0)
        {
            curWaterAmount = 0;
            StopEffects();
        }
    }

    public void StartRefill(Fountain theFountain)
    {
        Debug.Log("Trying to refill");
        if (isRefilling)
        {
            return;
        }

        isRefilling = true;

        //Parent to the fountain and let the player interact with it to pick it up again
        theFountain.wateringCan = this;
        parentFountain = theFountain;
        this.transform.parent = theFountain.wateringCanHolder.transform;
        this.transform.position = theFountain.wateringCanHolder.transform.position;
        refillCollider.SetActive(true);

        //Remove tool from tool list so player has to grab it to reequip
        Inventory.instance.RemoveTool(this);
    }

    public void Refill(Fountain theFountain)
    {
        Debug.Log("Watering Can is refilling");
        curWaterAmount += theFountain.refillAmount * Time.deltaTime;
        if (curWaterAmount > maxWaterAmount)
            curWaterAmount = maxWaterAmount;
    }

    public void StopRefill()
    {
        isRefilling = false;
        refillCollider.SetActive(false);
        parentFountain.wateringCan = null;
        parentFountain = null;
        Inventory.instance.EquipTool("WateringCan");
    }

}
