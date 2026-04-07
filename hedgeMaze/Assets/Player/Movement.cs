using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float drag;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        SpeedControl();

        //Handle Drag
        rb.linearDamping = drag;

    }

    private void FixedUpdate()
    {
        Move();
    }

    void MyInput()
    {
        //Debug.Log("Getting input");
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput > 0 || verticalInput > 0)
            CameraEffects.instance.Bob();
        else
            CameraEffects.instance.StopBob();
    }

    void Move()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * speed * Time.deltaTime * 10f, ForceMode.Force);
    }
    void SpeedControl()
    {
        Vector3 velocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        //Limit velocity
        if(velocity.magnitude > speed)
        {
            Vector3 limitedVelocity = velocity.normalized * speed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }
    }
    
}
