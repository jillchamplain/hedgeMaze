using UnityEngine;

public class WateringCan : Tool
{
    [SerializeField] Movement playerMovement;
    [SerializeField] Animator animator;
    [SerializeField] float curWaterAmount;
    public float GetWaterAmount() {  return curWaterAmount; }
    [SerializeField] float maxWaterAmount;
    public float GetMaxWaterAmount() { return maxWaterAmount; }
    [SerializeField] float waterDepleteAmount;
    [SerializeField] float waterGivenAmount;
    [SerializeField] float passiveWaterDepleteAmount;
    [SerializeField] float sprintWaterDepleteAmount;
    [SerializeField] ParticleSystem waterParticleSystem;
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
        //Debug.Log(hitObject);
        if (hitObject.GetComponent<Flower>())
        {
            isWatering = true;
            Water();
            if(curWaterAmount > 0)
                hitObject.GetComponent<Flower>().Water(waterGivenAmount);
            hitObject.GetComponentInParent<FlowerPatch>().CheckIfWatered();
        }

        else if(hitObject.GetComponentInParent<Fountain>())
        {
            Refill(hitObject.GetComponentInParent<Fountain>().refillAmount);
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

    public void Refill(float refillAmount)
    {
        curWaterAmount += refillAmount;
        if (curWaterAmount > maxWaterAmount)
            curWaterAmount = maxWaterAmount;
    }
}
