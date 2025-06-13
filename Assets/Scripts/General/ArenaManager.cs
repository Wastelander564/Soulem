using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ArenaManager : MonoBehaviour
{
    public int StartCount = 1;
    public int RoundCount;

    [SerializeField] private TextMeshProUGUI counter;
    [SerializeField] private GameObject RoundPanel;

    private bool isPanelActive;
    private SaveManager saveManager;

    void Start()
    {
        saveManager = FindObjectOfType<SaveManager>();
        RoundCount = saveManager != null ? saveManager.LoadRoundCount(StartCount) : StartCount;

        counter.text = $"{RoundCount}";

        RoundPanel.SetActive(false);
        isPanelActive = false;
        Time.timeScale = 1f;
    }

    public void AddRound()
    {
        RoundCount++;
        counter.text = $"{RoundCount}";
        saveManager?.SaveRoundCount(RoundCount);

        if (RoundCount % 10 == 0)
            TriggerMilestonePanel();
    }

    void TriggerMilestonePanel()
    {
        RoundPanel.SetActive(true);
        isPanelActive = true;
        Time.timeScale = 0f;
    }

    public void OnContinue()
    {
        RoundPanel.SetActive(false);
        isPanelActive = false;
        Time.timeScale = 1f;
    }

    public void OnReturnToHub()
    {
        saveManager?.SaveRoundCount(RoundCount);
        Time.timeScale = 1f;
        SceneManager.LoadScene("HubMausoleum");
    }
}
