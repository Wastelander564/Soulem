using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;

    private float damage;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    void Update()
    {
        transform.position += ((Vector3)(direction * speed * Time.deltaTime)*-1);
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
                enemy.TakeDamage(damage);
            }
        }
        
        Destroy(gameObject); // Destroy bullet on hit
        }
    }
}
