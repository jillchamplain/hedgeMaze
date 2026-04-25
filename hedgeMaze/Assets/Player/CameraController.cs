using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [SerializeField] float sensX;
    [SerializeField] float sensY;

    [SerializeField] Transform orientation;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform cameraPosition;

    float xRotation;
    float yRotation;

    float mouseX;
    public float mouseY;


    public bool canLook = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse input
         mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
         mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
        if (!canLook)
        {
            return;
        }

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        //Rotate camera and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

    }

    private void LateUpdate()
    {
    }
}
