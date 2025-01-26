using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_RigidBody;
    private Camera m_Camera;

    [Header("Movement Settings (m/s)")]
    public float forceMultiplier = 10f;
    public float maxSpeed = 25f;

    [Header("Inertia Settings")]
    public float inertiaDamping = 0.97f;
    public float stoppingResistance = 0.95f;

    [Header("Deceleration Settings")]
    public float decelerationForce = 5f;
    public float minimumSpeedForDeceleration = 0.5f;

    [Header("Uncontrollable Movement Settings")]
    public float uncontrollabilityFactor = 1.5f;
    public float driftFactor = 1.5f;
    public float speedBuildupMultiplier = 1.2f;

    [Header("Speed Boost Settings")]
    public float accelerationRampSpeed = 2f;
    public float boostMultiplier = 2f;
    public float boostDuration = 2f;
    public float boostCooldown = 5f;

    private bool isBoosting = false;
    private float boostEndTime = 0f;
    private float nextBoostTime = 0f;

    [Header("Debug Info")]
    public float currentSpeed = 0f;

    private Vector3 lastInputDirection = Vector3.zero;
    private float currentForceMultiplier;

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Camera = Camera.main;
    }

    void Update()
    {
        if (isBoosting && Time.time >= boostEndTime)
        {
            isBoosting = false; // End the boost effect
        }
    }

    void FixedUpdate()
    {
        float inputX = -Input.GetAxis("Horizontal");
        float inputZ = -Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(inputX, 0, inputZ).normalized;

        if (inputDirection.magnitude > 0)
        {
            float targetMultiplier = isBoosting ? forceMultiplier * boostMultiplier : forceMultiplier;
            currentForceMultiplier = Mathf.Lerp(currentForceMultiplier, targetMultiplier, accelerationRampSpeed * Time.fixedDeltaTime);
            Vector3 driftedDirection = Vector3.Lerp(lastInputDirection, inputDirection, 1f - driftFactor).normalized;

            Vector3 randomForce = new Vector3(
                Random.Range(-uncontrollabilityFactor, uncontrollabilityFactor),
                0,
                Random.Range(-uncontrollabilityFactor, uncontrollabilityFactor)
            );

            if (m_RigidBody.linearVelocity.magnitude < maxSpeed)
            {
                m_RigidBody.AddForce((driftedDirection + randomForce) * currentForceMultiplier, ForceMode.Force);
            }

            lastInputDirection = inputDirection;
        }
        else
        {
            currentForceMultiplier = 0f;
            forceMultiplier = 10f;

            m_RigidBody.linearVelocity *= stoppingResistance;

            if (m_RigidBody.linearVelocity.magnitude > minimumSpeedForDeceleration)
            {
                Vector3 deceleration = -m_RigidBody.linearVelocity.normalized * decelerationForce;
                m_RigidBody.AddForce(deceleration, ForceMode.Force);
            }
        }

        m_RigidBody.linearVelocity *= inertiaDamping;
        currentSpeed = m_RigidBody.linearVelocity.magnitude;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ramp"))
        {
            ActivateSpeedBoost();
        }
    }

    private void ActivateSpeedBoost()
    {
        if (!isBoosting && Time.time >= nextBoostTime)
        {
            isBoosting = true;
            boostEndTime = Time.time + boostDuration;
            nextBoostTime = Time.time + boostCooldown;
        }
    }
}
