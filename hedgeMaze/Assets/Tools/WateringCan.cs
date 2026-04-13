using UnityEngine;

public class WateringCan : Tool
{
    [SerializeField] Movement playerMovement;
    [SerializeField] float curWaterAmount;
    public float GetWaterAmount() {  return curWaterAmount; }
    [SerializeField] float maxWaterAmount;
    public float GetMaxWaterAmount() { return maxWaterAmount; }
    [SerializeField] float waterDepleteAmount;
    [SerializeField] float passiveWaterDepleteAmount;
    [SerializeField] float sprintWaterDepleteAmount;

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

    }

    public override void Use(GameObject hitObject)
    {
        if (hitObject.GetComponent<Flower>())
        {
            Water();
        }
    }

    public void Water()
    {
        curWaterAmount -= waterDepleteAmount;
        if(curWaterAmount < 0 )
            curWaterAmount = 0;
    }

    public void PassiveDepleteWater()
    {
        curWaterAmount -= passiveWaterDepleteAmount * Time.deltaTime;
        if( curWaterAmount < 0 )
            curWaterAmount = 0;
    }

    public void SprintDepleteWater()
    {
        curWaterAmount -= sprintWaterDepleteAmount * Time.deltaTime;
        if(curWaterAmount < 0 )
            curWaterAmount = 0;
    }

    public void Refill(float refillAmount)
    {
        curWaterAmount += refillAmount;
        if (curWaterAmount > maxWaterAmount)
            curWaterAmount = maxWaterAmount;
    }
}
