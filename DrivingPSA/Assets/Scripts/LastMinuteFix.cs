using System.Collections;
using UnityEngine;

/// <summary>
/// LastMinuteFix: Manages a sequence where alive/dead sprites spawn and cutscenes trigger.
/// 1. Alive sprite spawns at front, camera looks at it
/// 2. After aliveDisplayTime, alive sprite deletes and dead sprite spawns at back
/// 3. After cutsceneDelayAfterDeath, set isCutscene to true and move player to Backview
/// 4. After endSequenceDelay, trigger EndSequenceManager
/// </summary>
public class LastMinuteFix : MonoBehaviour
{
    [Header("Sprites")]
    [Tooltip("Alive sprite to spawn at front.")]
    public Sprite aliveSprite;

    [Tooltip("Dead sprite to spawn at back.")]
    public Sprite deadSprite;

    [Header("Spawn Positions")]
    [Tooltip("GameObject marking front spawn position for alive sprite.")]
    public Transform frontSpawn;

    [Tooltip("GameObject marking back spawn position for dead sprite.")]
    public Transform backSpawn;

    [Header("Timing")]
    [Tooltip("Seconds to display alive sprite before destroying it.")]
    public float aliveDisplayTime = 3f;

    [Tooltip("Seconds after alive sprite destroys before setting isCutscene to true.")]
    public float cutsceneDelayAfterDeath = 1f;

    [Tooltip("Seconds after starting cutscene before triggering end sequence.")]
    public float endSequenceDelay = 3f;

    [Tooltip("Seconds to wait before automatically starting the sequence. Set to 0 to disable auto-start.")]
    public float autoStartDelay = 0f;

    [Header("Camera & Player")]
    [Tooltip("Main camera to look at alive sprite.")]
    public Camera mainCamera;

    [Tooltip("Player transform to move to Backview position.")]
    public GameObject playerTransform;

    [Tooltip("Speed of player movement towards Backview.")]
    public float playerMoveSpeed = 5f;

    [Tooltip("Speed of player rotation towards Backview.")]
    public float playerRotateSpeed = 5f;

    [Header("End Sequence")]
    [Tooltip("EndSequenceManager to trigger when sequence completes.")]
    public EndSequenceManager endSequenceManager;

    [Header("Visual")]
    [Tooltip("Z depth for spawned sprites.")]
    public float spriteZDepth = 0f;

    [Tooltip("Scale of spawned sprites.")]
    public Vector3 spriteScale = Vector3.one;

    GameObject aliveGO;
    GameObject deadGO;
    Coroutine sequenceCoroutine;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        // Auto-start the sequence after a delay if autoStartDelay > 0
        if (autoStartDelay > 0f)
        {
            StartCoroutine(AutoStartSequence());
        }
    }

    IEnumerator AutoStartSequence()
    {
        yield return new WaitForSeconds(autoStartDelay);
        StartSequence();
    }

    /// <summary>
    /// Start the complete LastMinuteFix sequence.
    /// </summary>
    public void StartSequence()
    {
        if (aliveSprite == null || deadSprite == null)
        {
            Debug.LogWarning("LastMinuteFix: Sprites not assigned.");
            return;
        }

        if (frontSpawn == null || backSpawn == null)
        {
            Debug.LogWarning("LastMinuteFix: Spawn positions not assigned.");
            return;
        }

        if (sequenceCoroutine != null)
        {
            StopCoroutine(sequenceCoroutine);
        }

        sequenceCoroutine = StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        // 1. Spawn alive sprite at front
        aliveGO = SpawnSpriteAt(frontSpawn.position, aliveSprite);
        Debug.Log("Alive sprite spawned at front.");

        // 2. Force camera to look at alive sprite
        if (mainCamera != null)
        {
            StartCoroutine(LookAtSprite(aliveGO.transform, 0.5f));
        }

        // 3. Wait for aliveDisplayTime
        yield return new WaitForSeconds(aliveDisplayTime);

        // 4. Destroy alive sprite
        if (aliveGO != null)
        {
            Destroy(aliveGO);
            aliveGO = null;
        }
        Debug.Log("Alive sprite destroyed.");

        // 5. Spawn dead sprite at back
        deadGO = SpawnSpriteAt(backSpawn.position, deadSprite);
        Debug.Log("Dead sprite spawned at back.");

        // 6. Wait for cutsceneDelayAfterDeath
        yield return new WaitForSeconds(cutsceneDelayAfterDeath);

        // 7. Set isCutscene to true
        CutsceneState.isCutscene = true;
        Debug.Log("isCutscene set to true.");

        // 8. Move and rotate player to Backview
        if (playerTransform != null)
        {
            Transform backviewTarget = GameObject.Find("Backview")?.transform;
            if (backviewTarget != null)
            {
                StartCoroutine(MovePlayerToBackview(backviewTarget));
            }
            else
            {
                Debug.LogWarning("LastMinuteFix: Backview GameObject not found.");
            }
        }

        // 9. Wait for endSequenceDelay
        yield return new WaitForSeconds(endSequenceDelay);

        // 10. Trigger end sequence
        if (endSequenceManager != null)
        {
            endSequenceManager.StartEndSequence();
            Debug.Log("End sequence started.");
        }
        else
        {
            Debug.LogWarning("LastMinuteFix: EndSequenceManager not assigned.");
        }
    }

    /// <summary>
    /// Make camera look at a target over a duration.
    /// </summary>
    IEnumerator LookAtSprite(Transform target, float duration)
    {
        float elapsed = 0f;
        Quaternion startRot = mainCamera.transform.rotation;

        while (elapsed < duration && target != null)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            Vector3 dirToTarget = (target.position - mainCamera.transform.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(dirToTarget);
            mainCamera.transform.rotation = Quaternion.Lerp(startRot, targetRot, t);

            yield return null;
        }

        // Keep looking at target
        while (target != null)
        {
            Vector3 dirToTarget = (target.position - mainCamera.transform.position).normalized;
            mainCamera.transform.rotation = Quaternion.LookRotation(dirToTarget);
            yield return null;
        }
    }

    /// <summary>
    /// Move and rotate player to Backview position.
    /// </summary>
    IEnumerator MovePlayerToBackview(Transform backview)
    {
        while (playerTransform != null && Vector3.Distance(playerTransform.transform.position, backview.position) > 0.1f)
        {
            // Move towards backview
            playerTransform.transform.position = Vector3.Lerp(
                playerTransform.transform.position,
                backview.position,
                Time.deltaTime * playerMoveSpeed
            );

            // Rotate towards backview
            playerTransform.transform.rotation = Quaternion.Lerp(
                playerTransform.transform.rotation,
                backview.rotation,
                Time.deltaTime * playerRotateSpeed
            );

            yield return null;
        }

        // Snap to exact position/rotation
        if (playerTransform != null)
        {
            playerTransform.transform.position = backview.position;
            playerTransform.transform.rotation = backview.rotation;
        }

        Debug.Log("Player moved to Backview.");
    }

    GameObject SpawnSpriteAt(Vector3 worldPos, Sprite sprite)
    {
        GameObject spriteGO = new GameObject("Sprite_" + sprite.name);
        spriteGO.transform.position = new Vector3(worldPos.x, worldPos.y, spriteZDepth);
        spriteGO.transform.localScale = spriteScale;

        SpriteRenderer sr = spriteGO.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;

        return spriteGO;
    }

    /// <summary>
    /// Stop the sequence and clean up.
    /// </summary>
    public void StopSequence()
    {
        if (sequenceCoroutine != null)
        {
            StopCoroutine(sequenceCoroutine);
            sequenceCoroutine = null;
        }

        if (aliveGO != null)
        {
            Destroy(aliveGO);
            aliveGO = null;
        }

        if (deadGO != null)
        {
            Destroy(deadGO);
            deadGO = null;
        }

        CutsceneState.isCutscene = false;
    }
}
