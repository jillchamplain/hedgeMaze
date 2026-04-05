using UnityEngine;

public class WateringCan : Tool
{
    [SerializeField] float curWaterAmount;
    [SerializeField] float maxWaterAmount;
    [SerializeField] float waterDepleteAmount;
    void Start()
    {
        curWaterAmount = maxWaterAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Equip()
    {

    }

    public override void UnEquip()
    {

    }

    public override void Use()
    {

    }

    public void Water()
    {
        curWaterAmount -= waterDepleteAmount;
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
