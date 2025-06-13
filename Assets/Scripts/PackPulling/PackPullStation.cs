using UnityEngine;
using TMPro;

public class PackPullStation : MonoBehaviour
{
    private PlayerStats stats;
    private PackPulls pack;
    private bool playerInRange = false;

    [SerializeField] private TextMeshProUGUI interactionText;

    void Start()
    {
        stats = FindObjectOfType<PlayerStats>();
        pack = FindObjectOfType<PackPulls>();

        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Interaction Text is not assigned!");
        }
    }

    void Update()
    {
        if (playerInRange && interactionText != null)
        {
            interactionText.text = $"Press [Enter] to refine souls ({Mathf.FloorToInt(stats.Souls)}/100)";
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.Return))
        {
            if (stats.Souls >= 100)
            {
                pack.RefineSouls();
            }
            else
            {
                Debug.Log("Not enough souls to refine.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && interactionText != null)
        {
            playerInRange = true;
            interactionText.gameObject.SetActive(true);
            interactionText.text = $"Press [Enter] to refine souls ({Mathf.FloorToInt(stats.Souls)}/100)";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && interactionText != null)
        {
            playerInRange = false;
            interactionText.gameObject.SetActive(false);
        }
    }
}
