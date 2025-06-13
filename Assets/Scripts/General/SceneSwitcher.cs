using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public string sceneToLoad;

    public void SwitchScene()
    {
        Debug.Log($"Switching to scene: {sceneToLoad}");
        SceneManager.LoadScene(sceneToLoad);
    }
}
