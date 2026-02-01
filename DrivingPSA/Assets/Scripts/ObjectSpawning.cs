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

    [Tooltip("If true, spawn objects rotated 180 degrees around the Y axis.")]
    public bool rotate180 = false;

    // internal state
    float timer = 0f;
    GameObject lastSpawned;
    [Tooltip("Optional: set to a layer index (0-31) to place spawned objects on. If set, spawned objects on this layer will ignore collisions with each other.")]
    public int spawnLayer = -1;

    bool ignoredLayerCollisionSet = false;

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
        Quaternion spawnRot = rotate180 ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity;
        lastSpawned = Instantiate(spawnPrefab, spawnPos, spawnRot);

        // Ensure the spawned object has a Rigidbody so trigger collisions fire reliably.
        // We set it kinematic so movement via transform is still allowed.
        if (lastSpawned.GetComponent<Rigidbody>() == null)
        {
            var rb = lastSpawned.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // If spawnLayer is set, assign the spawned object and its children to that layer
        // and ensure objects on that layer ignore collisions with each other.
        if (spawnLayer >= 0 && spawnLayer <= 31)
        {
            SetLayerRecursively(lastSpawned.transform, spawnLayer);
            if (!ignoredLayerCollisionSet)
            {
                Physics.IgnoreLayerCollision(spawnLayer, spawnLayer, true);
                ignoredLayerCollisionSet = true;
            }
        }
    }

    void SetLayerRecursively(Transform root, int layer)
    {
        root.gameObject.layer = layer;
        for (int i = 0; i < root.childCount; i++)
        {
            SetLayerRecursively(root.GetChild(i), layer);
        }
    }
}
