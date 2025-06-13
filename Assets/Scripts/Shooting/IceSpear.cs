using UnityEngine;

public class IceSpear : MonoBehaviour
{
    public GameObject IcePrefab;
    public Transform StartLocation;
    public Transform arm;

    public void IceShot(float damage)
    {
        if (IcePrefab != null && StartLocation != null)
        {
            // Adjust the rotation by -90 degrees on the Z-axis
            Quaternion rotation = StartLocation.rotation * Quaternion.Euler(0, 0, -90);

            // Instantiate the ice bullet at the start location with the adjusted rotation
            GameObject bullet = Instantiate(IcePrefab, StartLocation.position, rotation);

            // Determine the shoot direction based on the arm rotation
            Vector2 shootDirection = arm != null ? arm.up : Vector2.up;

            // Access the IceSpearBullet script on the instantiated bullet
            IceSpearBullet bulletScript = bullet.GetComponent<IceSpearBullet>();

            // Access the ElementalAffinity component from the enemy
            ElementalAffinity enemyElementalAffinity = bullet.GetComponent<ElementalAffinity>();

            // Set the direction and damage for the bullet, using the enemy's element
            if (enemyElementalAffinity != null)
            {
                bulletScript.SetDirection(shootDirection);
                bulletScript.SetDamage(damage, enemyElementalAffinity.Element);
            }
            else
            {
                // If there's no ElementalAffinity on the enemy, apply regular damage
                bulletScript.SetDirection(shootDirection);
                bulletScript.SetDamage(damage, ElementalAffinity.States.Necrotic);
            }
        }
    }
}
