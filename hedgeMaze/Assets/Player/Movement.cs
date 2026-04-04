using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform orientation;

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
    }

    private void FixedUpdate()
    {
        Move();
    }

    void MyInput()
    {
        Debug.Log("Getting input");
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    void Move()
    {
        moveDirection = orientation.forward * -horizontalInput + orientation.right * verticalInput;
        rb.AddForce(moveDirection.normalized * speed * Time.deltaTime * 10f, ForceMode.Force);
    }
    
}
