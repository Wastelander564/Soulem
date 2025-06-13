using UnityEngine;

public class FlameBurst : MonoBehaviour
{
    public GameObject FlameBurstShot;
    public Transform StartLocation;

    private GameObject spawnedFlame;

    public void flameShot(float damage, Shooting shooter)
    {
        if (spawnedFlame != null) return;

        // Get mouse direction
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - StartLocation.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Determine if we should flip based on mouse position
        bool shouldFlip = mousePos.x < StartLocation.position.x;

        // Spawn flame
        spawnedFlame = Instantiate(FlameBurstShot, StartLocation.position, Quaternion.Euler(0f, 0f, angle), StartLocation);

        // Flip flame sprite if needed
        if (shouldFlip)
        {
            Vector3 localScale = spawnedFlame.transform.localScale;
            localScale.x *= -1;
            spawnedFlame.transform.localScale = localScale;
        }
    }

    public void StopFlame()
    {
        if (spawnedFlame != null)
        {
            FlameCollider flameCollider = spawnedFlame.GetComponentInChildren<FlameCollider>();
            if (flameCollider != null)
            {
                flameCollider.SetActive(false);
            }

            Animator anim = spawnedFlame.GetComponentInChildren<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("End");
                DestroyFlame();
            }
        }
    }

    // Called from animation event
    public void DestroyFlame()
    {
        if (spawnedFlame != null)
        {
            Destroy(spawnedFlame);
            spawnedFlame = null;
        }
    }
}
