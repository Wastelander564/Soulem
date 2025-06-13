using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenLogic : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "StartScene"; // Replace with your actual game scene name

    public void StartNewGame()
    {
        PlayerPrefs.DeleteAll(); // Wipe all saved data
        PlayerPrefs.Save();
        Debug.Log("Starting new game: PlayerPrefs cleared.");
        SceneManager.LoadScene(gameSceneName);
    }

    public void LoadGame()
    {
        if (HasSavedData())
        {
            Debug.Log("Loading saved game...");
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogWarning("No saved data found.");
            // Optional: show a message to the player that no save exists
        }
    }

    private bool HasSavedData()
    {
        // Customize this check depending on what you save
        return PlayerPrefs.HasKey("SoulCardCollection") || PlayerPrefs.HasKey("SavedDeck");
    }
}
