using UnityEngine;

public class Shooting : MonoBehaviour
{
    public NecroBulletShooting Necros;
    public ThunderArm Thunder;
    public FlameBurst Fire;
    public GeyserBlast Water;
    public IceSpear Ice;
    public MainMenu menu;

    public ElementalAffinity Element;
    public GameObject bulletPrefab;
    public Transform bulletSpawner;
    public Transform arm;

    public float fireCooldown = 0.3f;
    private float lastFireTime = 0f;

    public float damage = 10f;
    public float damageModifier = 1.00f;
    public float FinalDamage;

    private PlayerStats stats;

    void Start()
    {
        stats = GetComponentInParent<PlayerStats>();
        if (Element == null)
        {
            Element = GetComponent<ElementalAffinity>();
        }
    }

    void Update()
    {
        // Update FinalDamage every frame to use current PlayerStats value
        if (stats != null)
        {
            FinalDamage = stats.DamageToEnemies;
        }
        else
        {
            FinalDamage = damage * damageModifier; // fallback
        }

        if (Input.GetMouseButtonDown(0) && Time.time >= lastFireTime + fireCooldown)
        {
            if (Element != null)
            {
                switch (Element.Element)
                {
                    case ElementalAffinity.States.Necrotic:
                        if (Necros != null) Necros.NecroShot(FinalDamage, this);
                        break;
                    case ElementalAffinity.States.Lightning:
                        if (Thunder != null) Thunder.LightningShot();
                        break;
                    case ElementalAffinity.States.Fire:
                        if (Fire != null) Fire.flameShot(FinalDamage, this);
                        break;
                    case ElementalAffinity.States.Water:
                        if (Water != null) Water.WaterShot(FinalDamage, this);
                        break;
                    case ElementalAffinity.States.Ice:
                        if (Ice != null) Ice.IceShot(FinalDamage);
                        break;
                }

                lastFireTime = Time.time;
            }
        }

        if (Input.GetMouseButtonUp(0) || menu.MenuActive == true)
        {
            if (Element != null)
            {
                if (Element.Element == ElementalAffinity.States.Lightning && Thunder != null)
                {
                    Thunder.StopLighting();
                }
                else if (Element.Element == ElementalAffinity.States.Fire && Fire != null)
                {
                    Fire.StopFlame();
                }
                else if (Element.Element == ElementalAffinity.States.Water && Water != null)
                {
                    Water.stopWater();
                }
            }
        }
    }
}
