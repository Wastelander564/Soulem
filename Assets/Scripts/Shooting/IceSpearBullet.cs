using UnityEngine;

public class IceSpearBullet : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;

    private float damage;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    public void SetDamage(float dmg, ElementalAffinity.States enemyElement)
    {
        if (enemyElement == ElementalAffinity.States.Fire)
        {
            damage = dmg * 0.5f; // Ice vs Fire -> 50% damage
        }
        else if (enemyElement == ElementalAffinity.States.Lightning)
        {
            damage = dmg * 1.5f; // Ice vs Lightning -> 150% damage
        }
        else
        {
            damage = dmg; // Normal damage for other types
        }
    }

    void Update()
    {
        transform.position += ((Vector3)(direction * speed * Time.deltaTime) * -1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    ElementalAffinity enemyElement = enemy.GetComponent<ElementalAffinity>();
                    if (enemyElement != null)
                    {
                        // Get the enemy's element and apply the damage based on their elemental affinity
                        SetDamage(damage, enemyElement.Element);
                    }

                    enemy.TakeDamage(damage); // Apply damage to the enemy
                }

                 
            }
            Destroy(gameObject); // Destroy bullet on hit
        }
    }
}
