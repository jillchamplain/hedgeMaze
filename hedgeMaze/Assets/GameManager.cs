using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager instance;
    [SerializeField] public int flowersWatered = 0;
    [SerializeField] public int totalFlowers;
    public void WaterFlower()
    {
        flowersWatered++;
    }
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

    private void Update()
    {
        if(flowersWatered >= totalFlowers)
        {
            Debug.Log("You win!");
            SceneManager.LoadScene(0);
        }
    }

}
