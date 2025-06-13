using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public PlayerStats Player;
    public float currentHP;
    public Dash dash;

    [Header("UI")]
    [SerializeField] private Slider hpSlider;

    [Header("Scene Management")]
    [SerializeField] private string youDiedSceneName = "YouDied";

    private float damageCooldown = 1f;
    private float lastDamageTime = -999f;

    private SaveManager saveManager;
    private SceneSwitcher switcher;

    void Start()
    {
        saveManager = FindObjectOfType<SaveManager>();
        switcher = FindObjectOfType<SceneSwitcher>();

        currentHP = Player.MaxHp;

        if (hpSlider != null)
        {
            hpSlider.maxValue = currentHP;
            hpSlider.value = currentHP;
        }

        Debug.Log($"PlayerHP Start - Max HP set to {currentHP}");
    }

    void Update()
    {
        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }

        if (currentHP <= 0)
        {
            Debug.Log("Player died.");

            if (saveManager != null)
                saveManager.ClearSaveOnPlayerDeath();

            if (Player != null)
                Player.ResetSouls();

            if (hpSlider != null)
                Destroy(hpSlider.gameObject);

            if (switcher != null)
            {
                switcher.sceneToLoad = youDiedSceneName;
                switcher.SwitchScene();
            }

            Destroy(gameObject); // Optional
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Enemy") || other.CompareTag("Boss")) && !dash.isDashing)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && enemy.GetCanDamage())
            {
                if (Time.time - lastDamageTime >= damageCooldown)
                {
                    lastDamageTime = Time.time;

                    float damageTaken = enemy.damageAmount * Player.damageTakenMultiplier;
                    currentHP -= damageTaken;
                    currentHP = Mathf.Max(currentHP, 0f);

                    Debug.Log($"Took damage: {damageTaken}. Current HP: {currentHP}");
                }
            }
        }
    }
}
