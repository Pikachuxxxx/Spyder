using UnityEngine;

public class ProceduralObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] prefabs;
    [Range(0f, 1f)] public float[] probabilities;

    public int numberOfObjects = 10;
    public float spawnRangeX = 10f;
    public float spawnRangeZ = 50f;
    public float minSpacing = 5f;

    [Header("Difficulty Settings")]
    public float difficultyMultiplier = 1f;

    private float PREFAB_SPAWN_Y_OFFSET = 0.3f;
    private float PREFAB_SPAWN_Y_ROTATION = 90.0f;

    private void Start()
    {
        ValidateProbabilities();
        NormalizeProbabilities();
        GenerateObjects();
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

    void GenerateObjects()
    {
        float currentZ = 0f;

        for (int i = 0; i < numberOfObjects; i++)
        {
            GameObject prefabToSpawn = ChoosePrefab();
            if (prefabToSpawn == null) continue;

            float randomX = Random.Range(-spawnRangeX, spawnRangeX);
            Vector3 spawnPosition = new Vector3(randomX, PREFAB_SPAWN_Y_OFFSET, currentZ);

            Instantiate(prefabToSpawn, spawnPosition, Quaternion.Euler(0, PREFAB_SPAWN_Y_ROTATION, 0));

            currentZ += Random.Range(minSpacing, spawnRangeZ) / difficultyMultiplier;
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
}
