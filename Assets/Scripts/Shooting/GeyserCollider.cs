using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserCollider : MonoBehaviour
{
    private Shooting shootingScript; // Reference to Shooting script
    private float tickRate = 0.1f;  // Damage every 0.1 seconds
    private float tickTimer = 0f;

    void Start()
    {
        // Try to find the Shooting script on the parent
        shootingScript = GetComponentInParent<Shooting>();
        if (shootingScript == null)
        {
            Debug.LogWarning("GeyserCollider: Shooting script not found on parent.");
        }
    }

    void Update()
    {
        // Only update the timer if the collider is active
        if (gameObject.activeSelf)
        {
            tickTimer += Time.deltaTime;  // Count the time for tick damage
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (tickTimer >= tickRate)
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null && shootingScript != null)
                {
                    ElementalAffinity enemyElementalAffinity = enemy.GetComponent<ElementalAffinity>();
                    if (enemyElementalAffinity != null)
                    {
                        // Apply damage based on the element interaction between Water and the enemy's element
                        float adjustedDamage = shootingScript.FinalDamage;

                        if (enemyElementalAffinity.Element == ElementalAffinity.States.Fire)
                        {
                            adjustedDamage *= 1.5f;  // Water vs Fire -> 150% damage
                        }
                        else if (enemyElementalAffinity.Element == ElementalAffinity.States.Lightning)
                        {
                            adjustedDamage *= 0.5f;  // Water vs Ice -> 50% damage
                        }
                        else
                        {
                            adjustedDamage *= 1.0f;  // Water vs Lightning -> Normal damage
                        }

                        // Apply the adjusted damage
                        enemy.TakeDamage(adjustedDamage);
                    }
                    else
                    {
                        // If the enemy has no ElementalAffinity, apply the base damage
                        enemy.TakeDamage(shootingScript.FinalDamage);
                    }

                    tickTimer = 0f;  // Reset timer only after damage is applied
                }
            }
        }
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);

        if (isActive)
        {
            tickTimer = 0f;  // Reset the timer when the collider becomes active
        }
    }
}
