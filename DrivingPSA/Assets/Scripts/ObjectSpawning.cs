using UnityEngine;

public class ObjectSpawning : MonoBehaviour
{
    [Tooltip("Prefab to spawn; assign your prefab here.")]
    public GameObject spawnPrefab;

    [Tooltip("X range for random offset. 0 = same spot. Random in [-xRange, xRange].")]
    public float xRange = 0f;

    [Tooltip("If true, spawn by timer; if false, spawn when the last spawned object is this Z distance away.")]
    public bool useTimer = true;

    [Tooltip("Seconds between spawns when using timer mode.")]
    public float spawnInterval = 2f;

    [Tooltip("Required Z distance from the last spawned object to spawn the next (distance mode).")]
    public float spawnDistanceZ = 5f;

    // internal state
    float timer = 0f;
    GameObject lastSpawned;

    void Start()
    {
        if (spawnPrefab == null)
        {
            Debug.LogWarning("ObjectSpawning: spawnPrefab is not assigned. Assign a prefab in the Inspector.");
            return;
        }

        // Spawn one at start so there's an initial object, but only if not in a cutscene.
        if (!CutsceneState.isCutscene)
        {
            SpawnOne();
        }
    }

    void Update()
    {
        if (spawnPrefab == null) return;

        // Do nothing while in a cutscene; this also effectively pauses the spawn timer.
        if (CutsceneState.isCutscene) return;

        if (useTimer)
        {
            timer += Time.deltaTime;
            if (timer >= Mathf.Max(0.0001f, spawnInterval))
            {
                SpawnOne();
                timer = 0f;
            }
        }
        else
        {
            // Distance-based spawning: if there's no last spawned object, create one.
            if (lastSpawned == null)
            {
                SpawnOne();
                return;
            }

            // Check Z-distance between last spawned object and this spawner.
            float dz = Mathf.Abs(lastSpawned.transform.position.z - transform.position.z);
            if (dz >= spawnDistanceZ)
            {
                SpawnOne();
            }
        }
    }

    void SpawnOne()
    {
        float offsetX = (xRange == 0f) ? 0f : Random.Range(-xRange, xRange);
        Vector3 spawnPos = transform.position + new Vector3(offsetX, 0f, 0f);
        lastSpawned = Instantiate(spawnPrefab, spawnPos, Quaternion.identity);
    }
}
