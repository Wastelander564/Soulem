using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserBlast : MonoBehaviour
{
    public GameObject GeyserBlastShot;
    public Transform StartLocation;

    private GameObject spawnedGeyser;

    public void WaterShot(float damage, Shooting shooter)
    {
        if (spawnedGeyser != null) return;

        // Get mouse direction
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - StartLocation.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Determine if we should flip based on mouse position
        bool shouldFlip = mousePos.x < StartLocation.position.x;

        // Spawn flame
        spawnedGeyser = Instantiate(GeyserBlastShot, StartLocation.position, Quaternion.Euler(0f, 0f, angle), StartLocation);

        // Flip flame sprite if needed
        if (shouldFlip)
        {
            Vector3 localScale = spawnedGeyser.transform.localScale;
            localScale.x *= -1;
            spawnedGeyser.transform.localScale = localScale;
        }
    }

    public void stopWater()
    {
        if (spawnedGeyser != null)
        {
            GeyserCollider geyserCollider = spawnedGeyser.GetComponentInChildren<GeyserCollider>();
            if (geyserCollider != null)
            {
                geyserCollider.SetActive(false);
            }

            Animator anim = spawnedGeyser.GetComponentInChildren<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("End");
                DestroyWater();
            }
        }
    }

    // Called from animation event
    public void DestroyWater()
    {
        if (spawnedGeyser != null)
        {
            Destroy(spawnedGeyser);
            spawnedGeyser = null;
        }
    }
}
