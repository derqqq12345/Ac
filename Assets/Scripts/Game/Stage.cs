using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [Header("Stage Settings")]
    [SerializeField] private int stageNumber;
    [SerializeField] private float stageScrollSpeed = 5f;
    [SerializeField] private float minSpawnInterval = 0.5f;
    [SerializeField] private float maxSpawnInterval = 2f;
    
    [Header("Enemy Settings")]
    [SerializeField] private GameObject[] normalEnemyPrefabs;
    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private Vector2 spawnArea = new Vector2(10f, 5f);
    
    [Header("Debug Info")]
    [SerializeField] private bool showDebugInfo = true;
    [SerializeField] private bool autoStartStage = true;
    
    private bool isBossSpawned = false;
    private float spawnTimer = 0f;
    private float nextSpawnTime = 0f;
    private float bossSpawnDelay = 60f;
    private GameObject currentBoss;
    private StageManager stageManager;
    
    void Awake()
    {
        if (enemyContainer == null)
        {
            enemyContainer = new GameObject("EnemyContainer").transform;
            enemyContainer.SetParent(transform);
        }
    }
    
    void Start()
    {
        stageManager = FindObjectOfType<StageManager>();
        if (stageManager == null)
        {
            Debug.LogError("StageManager not found!");
            return;
        }

        if (autoStartStage)
        {
            StartCoroutine(StageRoutine());
        }
    }

    IEnumerator StageRoutine()
    {
        yield return StartCoroutine(StartStage());
        yield return StartCoroutine(MainStage());
        yield return StartCoroutine(BossStage());
    }

    IEnumerator StartStage()
    {
        if (showDebugInfo)
        {
            Debug.Log($"Starting Stage {stageNumber}");
        }
        yield return new WaitForSeconds(3f);
    }

    IEnumerator MainStage()
    {
        if (showDebugInfo)
        {
            Debug.Log($"Main Stage {stageNumber} Started");
        }
        while (!isBossSpawned)
        {
            yield return null;
        }
    }

    IEnumerator BossStage()
    {
        if (showDebugInfo)
        {
            Debug.Log($"Boss Stage {stageNumber} Started");
        }
        while (currentBoss != null)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(2f);
        
        if (showDebugInfo)
        {
            Debug.Log($"Stage {stageNumber} Complete");
        }
        
        StageComplete();
    }

    void StageComplete()
    {
        if (stageManager != null)
        {
            StartCoroutine(StageCompleteRoutine());
        }
        else
        {
            Debug.LogError("StageManager is null!");
        }
    }

    IEnumerator StageCompleteRoutine()
    {
        if (showDebugInfo)
        {
            Debug.Log($"Starting StageComplete sequence for stage {stageNumber}");
        }

        Enemy[] remainingEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in remainingEnemies)
        {
            Destroy(enemy.gameObject);
        }

        Bullet[] remainingBullets = FindObjectsOfType<Bullet>();
        foreach (Bullet bullet in remainingBullets)
        {
            Destroy(bullet.gameObject);
        }

        yield return new WaitForSeconds(1f);
        stageManager.OnStageComplete(stageNumber);
    }
    
    void Update()
    {
        if (showDebugInfo)
        {
            Debug.Log($"Stage {stageNumber} - Time: {spawnTimer:F2} / Boss Spawn: {bossSpawnDelay:F2}");
        }
        
        transform.Translate(Vector3.forward * stageScrollSpeed * Time.deltaTime);
        
        if (!isBossSpawned)
        {
            spawnTimer += Time.deltaTime;
            
            if (spawnTimer > bossSpawnDelay)
            {
                SpawnBoss();
            }
            else if (Time.time >= nextSpawnTime)
            {
                SpawnEnemies();
                nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
            }
        }
    }
    
    void SpawnEnemies()
    {
        if (normalEnemyPrefabs == null || normalEnemyPrefabs.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs assigned!");
            return;
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();
        int enemyIndex = Random.Range(0, normalEnemyPrefabs.Length);
        GameObject spawnedEnemy = Instantiate(normalEnemyPrefabs[enemyIndex], spawnPosition, Quaternion.identity, enemyContainer);
        
        if (showDebugInfo)
        {
            Debug.Log($"Spawned enemy at position: {spawnPosition}");
        }
        
        if (Random.value < 0.3f && asteroidPrefabs != null && asteroidPrefabs.Length > 0)
        {
            spawnPosition = GetRandomSpawnPosition();
            int asteroidIndex = Random.Range(0, asteroidPrefabs.Length);
            Instantiate(asteroidPrefabs[asteroidIndex], spawnPosition, Quaternion.identity, enemyContainer);
        }
    }
    
    void SpawnBoss()
    {
        if (bossPrefab == null)
        {
            Debug.LogError("Boss prefab not assigned!");
            return;
        }

        isBossSpawned = true;
        Vector3 spawnPosition = new Vector3(0, 0, transform.position.z + 20f);
        currentBoss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        
        if (showDebugInfo)
        {
            Debug.Log($"Boss spawned at stage {stageNumber}");
        }
        
        Enemy[] existingEnemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in existingEnemies)
        {
            if (enemy.gameObject != currentBoss)
            {
                Destroy(enemy.gameObject);
            }
        }
    }
    
    Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(-spawnArea.x, spawnArea.x);
        float y = Random.Range(-spawnArea.y, spawnArea.y);
        return new Vector3(x, y, transform.position.z + 20f);
    }
    
    private void OnDrawGizmos()
    {
        if (showDebugInfo)
        {
            Gizmos.color = Color.yellow;
            Vector3 center = transform.position + Vector3.forward * 20f;
            Vector3 size = new Vector3(spawnArea.x * 2, spawnArea.y * 2, 1f);
            Gizmos.DrawWireCube(center, size);
        }
    }
}