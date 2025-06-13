using UnityEngine;

public class FaceFollower : MonoBehaviour
{
    public Transform playerTransform;    // Assign via inspector or find dynamically
    public Transform faceTransform;      // The face part that moves (child of enemy)
    
    public float maxOffsetX = 0.1f;      // Max local movement in X
    public float maxOffsetY = 0.1f;      // Max local movement in Y
    public float moveSpeed = 5f;         // Speed to follow player

    private Vector3 faceStartLocalPos;

    void Start()
    {
        if (faceTransform == null)
            faceTransform = transform;  // fallback: use this object's transform

        if (playerTransform == null)
            playerTransform = GameObject.FindWithTag("Player").transform;

        faceStartLocalPos = faceTransform.localPosition;
    }

    void Update()
    {
        // Calculate direction from enemy to player in world space
        Vector3 directionToPlayer = playerTransform.position - transform.position;

        // Convert direction to local space of enemy
        Vector3 localDir = transform.InverseTransformDirection(directionToPlayer);

        // We only want to move face on X and Y axes in local space
        Vector3 targetLocalPos = faceStartLocalPos + new Vector3(
            Mathf.Clamp(localDir.x, -maxOffsetX, maxOffsetX),
            Mathf.Clamp(localDir.y, -maxOffsetY, maxOffsetY),
            0f
        );

        // Smoothly move face towards target local position
        faceTransform.localPosition = Vector3.Lerp(faceTransform.localPosition, targetLocalPos, Time.deltaTime * moveSpeed);
    }
}
