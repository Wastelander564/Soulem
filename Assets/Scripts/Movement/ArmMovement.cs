using UnityEngine;

public class ArmRotation : MonoBehaviour
{
    public Transform body; // Assign the body object to flip correctly
    private bool isFlipped = false;

    void Start()
    {
        if (body == null)
        {
            GameObject found = GameObject.Find("SoulGolem");
            if (found != null)
            {
                body = found.transform;
            }
            else
            {
                Debug.LogWarning("SoulGolem not found in the scene!");
            }
        }
    }

    void Update()
    {
        RotateArm();
    }

    void RotateArm()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;

        // Fix the 90-degree offset by adding 90°
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

        // Flip only when the mouse crosses the vertical centerline
        bool shouldFlip = mousePos.x < body.position.x;

        if (shouldFlip && !isFlipped)
        {
            FlipCharacter(true);
            isFlipped = true;
        }
        else if (!shouldFlip && isFlipped)
        {
            FlipCharacter(false);
            isFlipped = false;
        }

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void FlipCharacter(bool flip)
    {
        Vector3 scale = body.localScale;
        scale.x = Mathf.Abs(scale.x) * (flip ? -1 : 1); // Keeps it at 3 or -3
        body.localScale = scale;
    }
}
