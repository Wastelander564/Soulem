using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderArm : MonoBehaviour
{
    [Header("Line Visual")]
    public LineRenderer line;
    public GameObject beamStartPoint;

    [Header("Targeting")]
    public float range = 10f;
    public LayerMask enemyLayer;
    public int numSegments = 10;
    public float jaggedness = 0.3f;

    [Header("Damage")]
    public float damageInterval = 0.5f;

    private Transform currentTarget;
    private float damageTimer;
    private bool isFiring;

    private Shooting shootingScript;

    void Start()
    {
        shootingScript = GetComponentInParent<Shooting>();
        if (shootingScript == null)
        {
            Debug.LogError("Shooting script not found in ThunderArm!");
            enabled = false;
            return;
        }

        if (line == null)
        {
            Debug.LogError("LineRenderer not assigned in ThunderArm!");
            enabled = false;
            return;
        }

        line.positionCount = numSegments + 1;
        line.enabled = false;
    }

    void Update()
    {
        if (!isFiring) return;

        FindTarget();

        if (currentTarget != null)
        {
            Vector3 start = beamStartPoint.transform.position;
            Vector3 end = currentTarget.position;
            start.z = -0.1f;
            end.z = -0.1f;

            Vector3[] points = GenerateJaggedPath(start, end);
            line.SetPositions(points);

            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                DamageTarget(currentTarget);
                damageTimer = 0f;
            }
        }
        else
        {
            line.enabled = false;
            damageTimer = 0f;
        }
    }

    public void LightningShot()
    {
        isFiring = true;
        line.enabled = true;
    }

    public void StopLighting()
    {
        isFiring = false;
        line.enabled = false;
    }

    void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);
        float closestDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (var hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = hit.transform;
            }
        }

        currentTarget = closest;
    }

    Vector3[] GenerateJaggedPath(Vector3 start, Vector3 end)
    {
        Vector3[] path = new Vector3[numSegments + 1];
        path[0] = start;
        path[numSegments] = end;

        Vector3 direction = (end - start).normalized;
        float segmentLength = Vector3.Distance(start, end) / numSegments;

        for (int i = 1; i < numSegments; i++)
        {
            Vector3 point = start + direction * i * segmentLength;
            Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward).normalized;
            float offset = Random.Range(-jaggedness, jaggedness);
            point += perpendicular * offset;
            path[i] = point;
        }

        return path;
    }

    void DamageTarget(Transform target)
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy == null || shootingScript == null) return;

        float finalDamage = shootingScript.FinalDamage;

        ElementalAffinity enemyAffinity = enemy.GetComponent<ElementalAffinity>();
        if (enemyAffinity != null)
        {
            switch (enemyAffinity.Element)
            {
                case ElementalAffinity.States.Water:
                    finalDamage *= 1.5f; // Lightning > Water
                    break;
                case ElementalAffinity.States.Ice: // if you later add Earth
                    finalDamage *= 0.5f; // Lightning < Earth
                    break;
                default:
                    break;
            }
        }

        enemy.TakeDamage(finalDamage);
    }
}
