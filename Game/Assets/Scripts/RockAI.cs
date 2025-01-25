using UnityEngine;
using System.Collections;

public class RockAI : MonoBehaviour
{
[Header("Rolling Settings")]
    public float baseForce = 5f;
    public float maxForce = 15f;
    public float catchUpFactor = 1f;
    public float transparentAlpha = 0.2f;
    public float transitionSpeed = 2f;

    private Transform player;

    private Rigidbody rb;
    private Material ballMaterial;

    private Color originalColor;
    private bool isTransparent = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player").transform;
        ballMaterial = GetComponent<Renderer>().material;
        if (ballMaterial != null)
            originalColor = ballMaterial.color;
    }

    void FixedUpdate()
    {
        ApplyForceTowardsPlayer();
    }

    private void ApplyForceTowardsPlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        float targetForce = baseForce + (distanceToPlayer * catchUpFactor);
        targetForce = Mathf.Clamp(targetForce, baseForce, maxForce);

        rb.AddForce(direction * targetForce, ForceMode.Acceleration);
    }

    private void HandleTransparency()
    {
        if (isTransparent && ballMaterial != null)
        {
            Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, transparentAlpha);
            ballMaterial.color = Color.Lerp(ballMaterial.color, transparentColor, Time.deltaTime * transitionSpeed);
        }
        else if (ballMaterial != null)
        {
            ballMaterial.color = Color.Lerp(ballMaterial.color, originalColor, Time.deltaTime * transitionSpeed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            isTransparent = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            isTransparent = false;
        }
    }
}
