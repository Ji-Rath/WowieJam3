using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject followTarget;
    public float Torque = 2.0f;
    public Transform camTransform;
    public float jumpForce = 100f;

    public float groundDistance = 10f;

    Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody body = gameObject.GetComponent<Rigidbody>();
        body.maxAngularVelocity = 25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Rigidbody body = gameObject.GetComponent<Rigidbody>();
            body.AddForce(0f, jumpForce, 0f);
        }
    }

    private void CalculateMovement()
    {
        float horizonalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Rigidbody body = gameObject.GetComponent<Rigidbody>();
        Vector3 moveDirection = new Vector3(verticalInput, 0.0f, -horizonalInput).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;

            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            body.AddTorque(moveDirection * Torque * Time.fixedDeltaTime);
        }
    }

    void FixedUpdate()
    {
        CalculateMovement();

        // We need to cache velocity here since on collision, the velocity is near 0
        velocity = GetComponent<Rigidbody>().velocity;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public bool IsGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        return Physics.Raycast(ray, groundDistance);
    }
}
