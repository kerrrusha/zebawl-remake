using Assets.Scripts.Util;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveForce = 3.0f;
    public float maxSpeed = 3.0f;

    private Transform cameraTransform;
    private Rigidbody rb;
    private Vector3 cameraPlayerPositionDelta;

    void Start()
    {
        cameraTransform = GameObject.FindWithTag("MainCamera").transform;
        CommonUtil.CheckNotNull(cameraTransform, nameof(cameraTransform));

        rb = GetComponent<Rigidbody>();
        cameraPlayerPositionDelta = new Vector3(
            cameraTransform.position.x - transform.position.x,
            cameraTransform.position.y - transform.position.y,
            cameraTransform.position.z - transform.position.z
            );
    }

    void FixedUpdate()
    {
        UpdatePlayer();
        UpdateCamera();
    }

    private void UpdatePlayer()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        Vector3 forceDirection = new Vector3(inputHorizontal, 0.0f, inputVertical);
        rb.AddForce(forceDirection * moveForce);

        LimitSpeed();
    }

    private void LimitSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void UpdateCamera()
    {
        Vector3 cameraPosition = new Vector3(
            transform.position.x + cameraPlayerPositionDelta.x,
            transform.position.y + cameraPlayerPositionDelta.y, 
            transform.position.z + cameraPlayerPositionDelta.z
            );
        cameraTransform.position = cameraPosition;
    }
}
