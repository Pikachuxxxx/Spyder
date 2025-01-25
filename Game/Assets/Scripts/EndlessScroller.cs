using UnityEngine;
using System.Collections.Generic;

public class EndlessScroller : MonoBehaviour
{
    [Header("Obstacles Spawn Settings")]
    public GameObject[] prefabs;
    [Range(0f, 1f)] public float[] probabilities;

    public int poolSize = 20;
    public float spawnRangeX = 10f;
    public float spawnSpacing = 20f;
    public float despawnDistance = 50f;

    [Header("Difficulty Settings")]
    public float difficultyMultiplier = 1f;

    [Header("Player Settings")]
    public Transform player;
    public float spawnAheadDistance = 100f;

    [Header("Platform Settings")]
    public GameObject[] platformPrefabs;
    public float platformSpawnAheadDistance = 200f;

    private Queue<GameObject> objectPool = new Queue<GameObject>();
    private float nextObstacleSpawnZ = 0.0f;
    private float nextPlatformSpawnZ = 0.0f;
    
    private float PREFAB_SPAWN_Y_OFFSET = 0.3f;
    private float PREFAB_SPAWN_Y_ROTATION = 90.0f;

    private void Start()
    {
        ValidateProbabilities();
        NormalizeProbabilities();
        InitializeObjectPool();
        nextObstacleSpawnZ = player.position.z;
    }

    private void Update()
    {
        while (player.position.z + spawnAheadDistance > nextObstacleSpawnZ)
        {
            SpawnObject();
            nextObstacleSpawnZ += spawnSpacing / difficultyMultiplier;
        }

        if (player.position.z + platformSpawnAheadDistance > nextPlatformSpawnZ)
        {
            SpawnPlatform();
        }

        RecycleObjects();
    }

    void ValidateProbabilities()
    {
        if (probabilities.Length != prefabs.Length)
        {
            Debug.LogWarning("Probabilities array size does not match prefabs array size. Resizing...");
            System.Array.Resize(ref probabilities, prefabs.Length);
        }
    }

    void NormalizeProbabilities()
    {
        float totalProbability = 0f;

        foreach (float prob in probabilities)
        {
            totalProbability += Mathf.Clamp01(prob);
        }

        if (totalProbability == 0f)
        {
            Debug.LogWarning("Total probability is zero. Assigning equal probabilities.");
            float equalProbability = 1f / prefabs.Length;
            for (int i = 0; i < probabilities.Length; i++)
            {
                probabilities[i] = equalProbability;
            }
        }
        else
        {
            for (int i = 0; i < probabilities.Length; i++)
            {
                probabilities[i] = Mathf.Clamp01(probabilities[i]) / totalProbability;
            }
        }
    }

    void InitializeObjectPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject prefabToSpawn = ChoosePrefab();
            GameObject obj = Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
    }

    void SpawnObject()
    {
        GameObject obj = objectPool.Dequeue();

        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPosition = new Vector3(randomX, PREFAB_SPAWN_Y_OFFSET, nextObstacleSpawnZ);

        obj.transform.position = spawnPosition;
        obj.transform.rotation = Quaternion.Euler(0, PREFAB_SPAWN_Y_ROTATION, 0);
        obj.SetActive(true);

        objectPool.Enqueue(obj);
    }

    void RecycleObjects()
    {
        foreach (GameObject obj in objectPool)
        {
            if (obj.activeSelf && obj.transform.position.z < player.position.z - despawnDistance)
            {
                obj.SetActive(false);
            }
        }
    }

    GameObject ChoosePrefab()
    {
        float randomValue = Random.value;
        float cumulativeProbability = 0f;

        for (int i = 0; i < prefabs.Length; i++)
        {
            cumulativeProbability += probabilities[i];
            if (randomValue <= cumulativeProbability)
            {
                return prefabs[i];
            }
        }

        return null;
    }

    void SpawnPlatform()
    {
        GameObject selectedPlatform = platformPrefabs[Random.Range(0, platformPrefabs.Length)];

        float platformLength = selectedPlatform.transform.localScale.z;

        Vector3 spawnPosition = new Vector3(0, 0, nextPlatformSpawnZ);
        // Destroy(Instantiate(selectedPlatform, spawnPosition, Quaternion.identity), 120.0f);
        Instantiate(selectedPlatform, spawnPosition, Quaternion.identity);

        nextPlatformSpawnZ += platformLength;
    }
}