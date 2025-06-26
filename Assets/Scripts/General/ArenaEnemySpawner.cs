using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public ArenaManager arenaManager;
    public Vector2 spawnAreaSize = new Vector2(10f, 10f);
    public Transform arenaCenter;

    private bool roundInProgress = false;
    private ElementalAffinity.States currentElement;
    public bool specialRound = false; // Toggle for special rounds
    public bool playerIsHere = false;

    void Start()
    {
        CleanupEnemies();
        StartCoroutine(WaitForPlayerThenStart());
    }

    IEnumerator WaitForPlayerThenStart()
    {
        // Wait until a GameObject tagged "Player" is found in the scene
        while (GameObject.FindGameObjectWithTag("Player") == null)
        {
            yield return null; // wait for next frame
        }

        playerIsHere = true;
        Debug.Log("Player found! Starting RoundLoop.");
        StartCoroutine(RoundLoop());
    }

    IEnumerator RoundLoop()
    {
        while (true)
        {
            if (!roundInProgress)
            {
                roundInProgress = true;
            
                yield return new WaitForSeconds(2f);  // Initial round start delay
                CleanupEnemies();
                ElementalRounds();

                int enemiesToSpawn = Random.Range(1, 10) + arenaManager.RoundCount;
                float startDelay = 0.1f; // First enemy spawns almost instantly
                float delayIncrement = 0.3f; // Each next enemy spawns slower

                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    float currentDelay = startDelay + (i * delayIncrement);

                    // Optionally, cap the delay so it doesn't get too long
                    currentDelay = Mathf.Min(currentDelay, 5f); // Max 5 seconds between spawns

                    yield return new WaitForSeconds(currentDelay); // Apply dynamic delay

                    Vector3 spawnPos = GetRandomSpawnPosition();
                    GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

                    var affinity = enemy.GetComponent<ElementalAffinity>();
                    if (affinity != null)
                    {
                        affinity.Element = specialRound ? GetRandomElement() : currentElement;

                        var colorChange = enemy.GetComponent<ColorChange>();
                        if (colorChange != null) colorChange.RefreshColor();
                        else Debug.LogWarning("Enemy missing ColorChange component");
                    }
                    else Debug.LogWarning("Enemy missing ElementalAffinity component");

                    Debug.Log($"Spawned enemy {i + 1}/{enemiesToSpawn} (delay: {currentDelay}s)");
                }
            }

            // End-of-round check
            yield return new WaitForSeconds(1f);
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 &&
                GameObject.FindGameObjectsWithTag("Boss").Length == 0)
            {
                arenaManager.AddRound();
                roundInProgress = false;
            }
        }
    }


    void CleanupEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Debug.Log("Cleaning up lingering enemy: " + enemy.name);
            Destroy(enemy);
        }

        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        foreach (var boss in bosses)
        {
            Debug.Log("Cleaning up lingering boss: " + boss.name);
            Destroy(boss);
        }
    }

    void ElementalRounds()
    {
        if (!specialRound)
        {
            currentElement = GetRandomElement();
            Debug.Log("Round Element: " + currentElement);
        }
        else
        {
            Debug.Log("Special Round: Random Elements Per Enemy");
        }
    }

    ElementalAffinity.States GetRandomElement()
    {
        int count = System.Enum.GetValues(typeof(ElementalAffinity.States)).Length;
        return (ElementalAffinity.States)Random.Range(0, count);
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 basePos = arenaCenter != null ? arenaCenter.position : Vector3.zero;
        Vector3 spawnPos;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float minDistanceFromPlayer = 2f;

        int attempts = 0;
        do
        {
            Vector2 offset = new Vector2(
                Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
                Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f)
            );
            spawnPos = new Vector3(basePos.x + offset.x, basePos.y, basePos.z + offset.y);

            attempts++;
            if (attempts > 20) break;
        }
        while (player != null && Vector3.Distance(spawnPos, player.transform.position) < minDistanceFromPlayer);

        return spawnPos;
    }
}
