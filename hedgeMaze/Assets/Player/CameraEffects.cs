using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [HideInInspector] public static CameraEffects instance;
    [Header("Camera Bob")]
    [Range(.001f,.1f)]
    [SerializeField] float amount = .002f;
    [Range(1f, 30f)]
    [SerializeField] float frequency = 10f;
    [Range(1f, 100f)]
    [SerializeField] float returnSpeed = 4f;


    static Vector3 startPos = Vector3.zero;
    Vector3 pos = Vector3.zero;
    float time = 0;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }

    }
    void Start()
    {
        startPos = Camera.main.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.localPosition = startPos + pos;
        pos.y = Mathf.Sin(time) * amount;
    }

    public void Bob(float speed)
    {
        time += Time.deltaTime * speed * frequency;

        if (time >= 2 * Mathf.PI) { time = 0; }

    }

    public void StopBob()
    {
        if (time > 3 * Mathf.PI / 2)
        {
            time = Mathf.Lerp(time, 2 * Mathf.PI, Time.deltaTime * returnSpeed);
        }
        else if (time > Mathf.PI / 2)
        {
            time = Mathf.Lerp(time, Mathf.PI, Time.deltaTime * returnSpeed);
        }
        else
        {
            time = Mathf.Lerp(time, 0, Time.deltaTime * returnSpeed);
        }
    }
}
