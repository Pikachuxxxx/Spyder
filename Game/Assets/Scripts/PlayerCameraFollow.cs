using UnityEngine;

public class DynamicCameraController : MonoBehaviour
{
    [Header("Camera Offset")]
    public float baseDistance = 10f;
    public float speedEffectMultiplier = 0.5f;
    public float maxDistanceChange = 5f;

    [Header("Smooth Follow Settings")]
    public float followSpeed = 5f;

    private Transform player;
    private PlayerController playerController;
    private Vector3 offset;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        if (player == null || playerController == null)
        {
            Debug.LogError("DynamicCameraController: Player or PlayerController not assigned!");
            return;
        }

        offset = new Vector3(0, baseDistance, -baseDistance);
    }

    void LateUpdate()
    {
        if (player == null || playerController == null)
        {
            return;
        }

        float playerSpeed = playerController.currentSpeed;

        float dynamicDistance = baseDistance + Mathf.Clamp(playerSpeed * speedEffectMultiplier, -maxDistanceChange, maxDistanceChange);
        offset = new Vector3(0, 2.5f, dynamicDistance);

        Vector3 desiredPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        transform.LookAt(player);
    }
}
