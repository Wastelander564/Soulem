using TMPro;
using UnityEngine;

public class SoulUICounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI soulText;

    private PlayerStats stats;

    void Start()
    {
        stats = FindComponentInScene<PlayerStats>();
        if (soulText == null)
        {
            Debug.LogError("SoulText reference not set in SoulUIUpdater!");
        }
    }

    void Update()
    {
        if (stats != null && soulText != null)
        {
            soulText.text = $"{Mathf.FloorToInt(stats.Souls)}";
        }
    }

    private T FindComponentInScene<T>() where T : MonoBehaviour
    {
        return FindObjectOfType<T>();
    }
}
