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

                // 🕒 Wait 2 seconds before starting the round
                yield return new WaitForSeconds(2f);

                // 🧹 Clean up lingering enemies and bosses
                CleanupEnemies();

                // Determine element(s) for this round
                ElementalRounds();

                // Spawn enemies
                int enemiesToSpawn = Random.Range(1, 10) + arenaManager.RoundCount;
                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    Vector3 spawnPos = GetRandomSpawnPosition();

                    if (enemyPrefab == null)
                    {
                        Debug.LogError("Enemy Prefab is null! Assign it in the inspector.");
                        yield break;
                    }

                    GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                    if (enemy == null)
                    {
                        Debug.LogError("Failed to instantiate enemy!");
                        yield break;
                    }

                    Debug.Log($"Spawned enemy: {enemy.name} at {spawnPos}");

                    var affinity = enemy.GetComponent<ElementalAffinity>();
                    if (affinity != null)
                    {
                        affinity.Element = specialRound ? GetRandomElement() : currentElement;

                        var colorChange = enemy.GetComponent<ColorChange>();
                        if (colorChange != null)
                            colorChange.RefreshColor();
                        else
                            Debug.LogWarning("Enemy missing ColorChange component");
                    }
                    else
                    {
                        Debug.LogWarning("Enemy missing ElementalAffinity component");
                    }

                    yield return new WaitForSeconds(0.2f);
                }
            }

            // Check if round is over
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
