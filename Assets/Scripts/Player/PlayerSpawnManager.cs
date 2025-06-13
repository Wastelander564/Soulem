using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager Instance;

    [System.Serializable]
    public struct SpawnInfo
    {
        public string sceneName;    // Name of the scene
        public Transform spawnPoint; // Transform of the spawn location for that scene
    }

    [Tooltip("Set spawn points and their corresponding scene names here.")]
    public SpawnInfo[] spawnLocations;

    private GameObject player;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            Debug.LogWarning("Player not found! Make sure your player has the 'Player' tag.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnPlayerAtScene(scene.name);
    }

    private void SpawnPlayerAtScene(string sceneName)
    {
        if (player == null)
        {
            Debug.LogWarning("Player not found to spawn.");
            return;
        }

        foreach (var spawnInfo in spawnLocations)
        {
            if (spawnInfo.sceneName == sceneName)
            {
                if (spawnInfo.spawnPoint != null)
                {
                    player.transform.position = spawnInfo.spawnPoint.position;
                    player.transform.rotation = spawnInfo.spawnPoint.rotation;
                    Debug.Log($"Spawned player at {sceneName} spawn point.");
                }
                else
                {
                    Debug.LogWarning($"Spawn point is null for scene {sceneName}.");
                }
                return; // Spawn found and done
            }
        }

        Debug.LogWarning($"No spawn point assigned for scene {sceneName}. Player position unchanged.");
    }
}
