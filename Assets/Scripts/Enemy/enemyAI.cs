using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        
        // Disable NavMeshAgent rotation & up axis so movement stays on X/Y plane
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (player != null)
        {
            // Set destination to player's position (keep Z fixed)
            Vector3 targetPos = player.position;
            targetPos.z = transform.position.z; // Lock Z to enemy's Z

            agent.SetDestination(targetPos);
        }
    }
}
