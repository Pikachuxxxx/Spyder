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
    public float fadeDistance = 5f;
    public float minAlpha = 0.1f;
    public float fadeSpeed = 2f;


    private Transform player;
    private Camera mainCamera;

    private Rigidbody rb;
    private Renderer objectRenderer;

    private Color originalColor;
    private bool isTransparent = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player").transform;
        mainCamera = Camera.main;
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, mainCamera.transform.position);
        float targetAlpha = distance <= fadeDistance ? minAlpha : 1f;

        Color currentColor = objectRenderer.material.color;
        float newAlpha = Mathf.Lerp(currentColor.a, targetAlpha, Time.deltaTime * fadeSpeed);
        objectRenderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);
  
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
}
