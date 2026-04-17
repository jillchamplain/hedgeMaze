using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] WateringCan wateringCan;
    [SerializeField] Slider waterSlider;
    [SerializeField] TextMeshProUGUI flowersWatered;
    [SerializeField] TextMeshProUGUI totalFlowers;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WaterUI();
        FlowerUI();
    }

    void WaterUI()
    {
        waterSlider.value = (wateringCan.GetWaterAmount() / wateringCan.GetMaxWaterAmount());
    }

    void FlowerUI()
    {
        flowersWatered.text = GameManager.instance.flowersWatered.ToString();
        totalFlowers.text = GameManager.instance.totalFlowers.ToString(); 
    }
}
