using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject followTarget;
    public GameObject mesh;
    public float Torque = 2.0f;
    public Transform camTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float horizonalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Rigidbody body = mesh.GetComponent<Rigidbody>();
        Vector3 moveDirection = new Vector3(verticalInput, 0.0f, horizonalInput).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;

            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            body.AddRelativeTorque(moveDirection * Torque);
        }
    }
}
