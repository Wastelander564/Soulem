using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSpecificPersistance : MonoBehaviour
{
    [SerializeField] private string targetSceneName = "YourSceneNameHere";

    private static SceneSpecificPersistance instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            UpdateActiveState(SceneManager.GetActiveScene().name);
        }
        else
        {
            // Duplicate found, destroy this one
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateActiveState(scene.name);
    }

    private void UpdateActiveState(string currentSceneName)
    {
        if (currentSceneName == targetSceneName)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
}
