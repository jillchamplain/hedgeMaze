using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [HideInInspector] public static CameraEffects instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Bob()
    {

    }
}
