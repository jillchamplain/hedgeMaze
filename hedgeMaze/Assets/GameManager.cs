using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] GameManager instance;
    [SerializeField] int flowersWatered = 0;
    [SerializeField] int flowersToWater;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        
    }

    
}
