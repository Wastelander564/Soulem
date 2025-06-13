using UnityEngine;

public class NecroBulletShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawner;
    public Transform arm;

    public float fireCooldown = 0.3f;

    void Start()
    {
        bulletSpawner = transform.Find("bulletSpawner");

        if (bulletSpawner == null)
        {
            Debug.LogWarning("BulletSpawner not found as child of " + gameObject.name);
        }
    }

    public void NecroShot(float damage, Shooting source)
    {
        if (bulletPrefab != null && bulletSpawner != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawner.position, Quaternion.identity);

            Vector2 shootDirection = arm != null ? arm.up : Vector2.up;
            Bullet bulletScript = bullet.GetComponent<Bullet>();

            bulletScript.SetDirection(shootDirection);
            bulletScript.SetDamage(damage);
        }
    }
}
