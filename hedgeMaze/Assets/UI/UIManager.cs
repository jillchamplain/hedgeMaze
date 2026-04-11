using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] WateringCan wateringCan;
    [SerializeField] Slider waterSlider;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WaterUI();
    }

    void WaterUI()
    {
        waterSlider.value = (wateringCan.GetWaterAmount() / wateringCan.GetMaxWaterAmount());
    }
}
