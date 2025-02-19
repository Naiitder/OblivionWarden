using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemyWave
{
    public string waveName;
    public List<EnemyGroup> enemyGroups;
    public float timeBetweenGroups = 2f;
    public float timeBeforeNextWave = 5f;
}

[System.Serializable]
public class EnemyGroup
{
    public GameObject enemyPrefab;
    public int count;
    public float timeBetweenSpawns = 0.5f;
}

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<EnemyWave> waves;
    [SerializeField] private float minSpawnDistance = 10f; 
    [SerializeField] private float maxSpawnDistance = 40f;
    [SerializeField] private bool autoStartWaves = true;
    [SerializeField] private PlayerManager player;

    public int CurrentWave { get; private set; } = 0;
    public bool IsSpawning { get; private set; }
    public int TotalEnemiesAlive { get; private set; }

    private void Start()
    {
        if (autoStartWaves)
        {
            StartNextWave();
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        for (int i = 0; i < 30; i++) 
        {
            Vector2 random2D = Random.insideUnitCircle.normalized *
                Random.Range(minSpawnDistance, maxSpawnDistance);

            Vector3 randomPoint = player.transform.position +
                new Vector3(random2D.x, 0, random2D.y);


            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 5f, UnityEngine.AI.NavMesh.AllAreas))
            {

                if (Vector3.Distance(hit.position, player.transform.position) >= minSpawnDistance)
                {
                    return hit.position;
                }
            }
        }

        return player.transform.position + Vector3.right * minSpawnDistance;
    }

    public void StartNextWave()
    {
        if (IsSpawning)
            return;

        StartCoroutine(SpawnWave(waves[CurrentWave]));
    }

    private IEnumerator SpawnWave(EnemyWave wave)
    {
        IsSpawning = true;

        foreach (EnemyGroup group in wave.enemyGroups)
        {
            yield return StartCoroutine(SpawnEnemyGroup(group));
            yield return new WaitForSeconds(wave.timeBetweenGroups);
        }

        yield return new WaitForSeconds(wave.timeBeforeNextWave);

        CurrentWave = (CurrentWave + 1) % waves.Count;
        IsSpawning = false;

        StartNextWave();
    }

    private IEnumerator SpawnEnemyGroup(EnemyGroup group)
    {
        for (int i = 0; i < group.count; i++)
        {
            SpawnEnemy(group.enemyPrefab);
            yield return new WaitForSeconds(group.timeBetweenSpawns);
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        Vector3 directionToPlayer = (player.transform.position - spawnPosition).normalized;
        Quaternion rotation = Quaternion.LookRotation(directionToPlayer);

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, rotation);

        CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
        if (enemyStats != null)
        {
            TotalEnemiesAlive++;
            enemyStats.OnDeath += () => OnEnemyDeath();
        }
    }

    private void OnEnemyDeath()
    {
        TotalEnemiesAlive--;
    }
}
