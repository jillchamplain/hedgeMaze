using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [HideInInspector] public static CameraEffects instance;
    [Header("Camera Bob")]
    [Range(.001f,.1f)]
    [SerializeField] float amount = .002f;
    [Range(1f, 30f)]
    [SerializeField] float frequency = 10f;
    [Range(10f, 100f)]
    [SerializeField] float smooth = 10f;


    static Vector3 startPos = Vector3.zero;

    private void Awake()
    {
        if(instance == null)
            instance = this;

    }
    void Start()
    {
        startPos = Camera.main.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Bob(float speed)
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency * speed) * amount;
        Camera.main.transform.localPosition = startPos + pos;
    }

    public void StopBob()
    {
        Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, startPos, 1 * Time.deltaTime);
    }
}
