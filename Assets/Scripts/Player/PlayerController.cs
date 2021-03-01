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

    public float WorldKillY = -25f;

    Vector3 velocity = Vector3.zero;

    AudioSource audioSource;
    public AudioClip ballRollSound;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody body = gameObject.GetComponent<Rigidbody>();
        body.maxAngularVelocity = 20f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        audioSource = gameObject.AddComponent<AudioSource>();
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
        Vector3 moveDirection = new Vector3(verticalInput, 0.0f, horizonalInput).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            float targetAngle;
            if (IsGrounded())
            {
                // Use torque to move the player around
                targetAngle = Mathf.Atan2(moveDirection.x, -moveDirection.z) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
                moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                body.AddTorque(moveDirection * Torque * Time.fixedDeltaTime);
            }
            else
            {
                // Give the player some air control
                targetAngle = Mathf.Atan2(moveDirection.z, moveDirection.x) * Mathf.Rad2Deg + camTransform.eulerAngles.y;
                moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                body.AddForce(moveDirection * Torque * Time.fixedDeltaTime);
            }
        }
    }

    void FixedUpdate()
    {
        CalculateMovement();

        //Prevent soft lock when falling off map
        if (transform.position.y < WorldKillY)
        {
            GetComponent<PlayerStats>().TakeDamage(new DamageInfo(10000, 0));
        }

        // We need to cache velocity here since on collision, the velocity is near 0
        velocity = GetComponent<Rigidbody>().velocity;

        if (IsGrounded() && velocity.magnitude > 0)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = ballRollSound;
                audioSource.Play();
            }
            else
            {
                audioSource.volume = velocity.magnitude / 20;
            }
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.volume = 1f;
        }
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public bool IsGrounded(out RaycastHit hit)
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        return Physics.Raycast(ray, out hit, groundDistance);
    }

    public bool IsGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        return Physics.Raycast(ray, groundDistance);
    }
}
