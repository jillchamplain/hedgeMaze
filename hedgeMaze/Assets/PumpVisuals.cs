using UnityEngine;

[ExecuteInEditMode]
public class PumpVisuals : MonoBehaviour
{
    [SerializeField] float minAngle;
    [SerializeField] float maxAngle;
    [SerializeField] float startAngle;
    [SerializeField] Transform handle;
    [SerializeField] Transform pusher;
    [SerializeField] Transform pusherTarget;
    [Range(0f, 1f)]
    [SerializeField] public float amount;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Lerp(minAngle, maxAngle, amount);
        handle.transform.localEulerAngles = new Vector3(handle.transform.eulerAngles.x, handle.transform.eulerAngles.y, angle);
        pusher.LookAt(pusherTarget, Vector3.right);
    }
}
