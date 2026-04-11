using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float acceleration;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    bool isSprinting;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        SpeedControl();

    }

    private void FixedUpdate()
    {
        Move();
    }

    void MyInput()
    {
        //Debug.Log("Getting input");
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput > 0 || verticalInput > 0)
            CameraEffects.instance.Bob(rb.linearVelocity.magnitude);
        else
            CameraEffects.instance.StopBob();

        isSprinting = Input.GetKey(KeyCode.LeftShift);
    }

    void Move()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        float targetSpeed;

        if (isSprinting)
            targetSpeed = sprintSpeed;
        else
            targetSpeed = speed;

        rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, moveDirection.normalized * targetSpeed, acceleration * Time.deltaTime);
    }
    void SpeedControl()
    {
        //Turn Y to zero since the character does not need to move up or down
        Vector3 velocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
    }
    
}
