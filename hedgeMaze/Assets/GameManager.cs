using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager instance;
    [SerializeField] int flowersWatered = 0;
    [SerializeField] int flowersToWater;

    public bool hasLost;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }


    public void PlayerDeath()
    {
        hasLost = true;
        Camera.main.transform.parent = null;
        GameObject.FindWithTag("Player").SetActive(false);
    }
    
}
