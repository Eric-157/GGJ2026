using System.Collections;
using UnityEngine;

/// <summary>
/// Spawns a sprite at position 1, waits a duration, deletes it, spawns at position 2.
/// The sprite at position 2 is deleted when isCutscene becomes false.
/// </summary>
public class SpriteSequenceSpawner : MonoBehaviour
{
    [Header("first Cutscene Sprites")]
    [Tooltip("Alive sprite for first cutscene.")]
    public Sprite aliveSpriteFirst;

    [Tooltip("Dead sprite for first cutscene.")]
    public Sprite deadSpriteFirst;

    [Header("second Cutscene Sprites")]
    [Tooltip("Alive sprite for second cutscene.")]
    public Sprite aliveSpriteSecond;

    [Tooltip("Dead sprite for second cutscene.")]
    public Sprite deadSpriteSecond;

    [Header("third Cutscene Sprites")]
    [Tooltip("Alive sprite for third cutscene.")]
    public Sprite aliveSpriteThird;

    [Tooltip("Dead sprite for third cutscene.")]
    public Sprite deadSpriteThird;

    [Tooltip("Position (Transform or world position) for the first sprite.")]
    public Transform position1;

    [Tooltip("Position (Transform or world position) for the second sprite.")]
    public Transform position2;

    [Tooltip("Seconds between spawning sprite 1 and sprite 2.")]
    public float timeBetweenSprites = 2f;

    [Tooltip("Z position for the sprites (depth in 2D).")]
    public float spriteZDepth = 0f;

    [Tooltip("Scale of the spawned sprites.")]
    public Vector3 spriteScale = Vector3.one;

    GameObject currentSprite;
    Coroutine sequenceCoroutine;

    /// <summary>
    /// Start the sprite sequence: spawn alive sprite at pos1, then dead sprite at pos2 after delay.
    /// Sprites are selected based on current CutsceneState.cutsceneType.
    /// </summary>
    public void StartSequence()
    {
        (Sprite alive, Sprite dead) = GetSpritesForCutscene();
        if (alive == null || dead == null)
        {
            //Debug.LogWarning("SpriteSequenceSpawner: No sprites assigned for cutscene type " + CutsceneState.cutsceneType);
            return;
        }

        if (position1 == null || position2 == null)
        {
            Debug.LogWarning("SpriteSequenceSpawner: position1 or position2 is not assigned.");
            return;
        }

        if (sequenceCoroutine != null)
        {
            StopCoroutine(sequenceCoroutine);
        }

        sequenceCoroutine = StartCoroutine(PlaySequence(alive, dead));
    }

    (Sprite, Sprite) GetSpritesForCutscene()
    {
        // switch (CutsceneState.cutsceneType)
        // {
        //     case CutsceneState.CutsceneType.First:
        //         return (aliveSpriteFirst, deadSpriteFirst);
        //     case CutsceneState.CutsceneType.Second:
        //         return (aliveSpriteSecond, deadSpriteSecond);
        //     case CutsceneState.CutsceneType.Third:
        //         return (aliveSpriteThird, deadSpriteThird);
        //     default:
        //         return (null, null);
        // }
        return (null, null);
    }

    IEnumerator PlaySequence(Sprite spriteAlive, Sprite spriteDead)
    {
        // Spawn alive sprite at position 1
        currentSprite = SpawnSpriteAt(position1.position, spriteAlive);

        // Wait for duration
        yield return new WaitForSeconds(timeBetweenSprites);

        // Delete first sprite
        if (currentSprite != null)
        {
            Destroy(currentSprite);
            currentSprite = null;
        }

        // Spawn dead sprite at position 2
        currentSprite = SpawnSpriteAt(position2.position, spriteDead);

        // Wait until isCutscene is false
        while (CutsceneState.isCutscene)
        {
            yield return null;
        }

        // Delete second sprite
        if (currentSprite != null)
        {
            Destroy(currentSprite);
            currentSprite = null;
        }
    }

    GameObject SpawnSpriteAt(Vector3 worldPos, Sprite sprite)
    {
        GameObject spriteGO = new GameObject("Sprite");
        spriteGO.transform.position = new Vector3(worldPos.x, worldPos.y, spriteZDepth);
        spriteGO.transform.localScale = spriteScale;

        SpriteRenderer sr = spriteGO.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;

        return spriteGO;
    }

    /// <summary>
    /// Stop the current sequence and clean up.
    /// </summary>
    public void StopSequence()
    {
        if (sequenceCoroutine != null)
        {
            StopCoroutine(sequenceCoroutine);
            sequenceCoroutine = null;
        }

        if (currentSprite != null)
        {
            Destroy(currentSprite);
            currentSprite = null;
        }
    }
}
