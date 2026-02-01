// using System.Collections;
// using UnityEngine;

// /// <summary>
// /// Manages sprite sequences for cutscenes with per-cutscene sprites and positions.
// /// Automatically triggers sequences after a delay, then repeats.
// /// </summary>
// public class SpriteSequenceSpawner : MonoBehaviour
// {
//     [System.Serializable]
//     public class CutsceneSprites
//     {
//         public Sprite aliveSprite;
//         public Sprite deadSprite;
//         public Transform position1;
//         public Transform position2;
//     }

//     [Header("Cutscene Configurations")]
//     [SerializeField] CutsceneSprites firstCutscene;
//     [SerializeField] CutsceneSprites secondCutscene;
//     [SerializeField] CutsceneSprites thirdCutscene;

//     [Header("Timing")]
//     [Tooltip("Seconds to wait before starting each sprite sequence.")]
//     public float delayBeforeSequence = 3f;

//     [Tooltip("Seconds between spawning alive sprite and dead sprite.")]
//     public float timeBetweenSprites = 2f;

//     [Header("Visual")]
//     [Tooltip("Z position for the sprites (depth in 2D).")]
//     public float spriteZDepth = 0f;

//     [Tooltip("Scale of the spawned sprites.")]
//     public Vector3 spriteScale = Vector3.one;

//     GameObject currentSprite;
//     Coroutine sequenceCoroutine;
//     Coroutine timerCoroutine;


//     void Start()
//     {
//         StartTimerLoop();
//     }

//     /// <summary>
//     /// Start the timer loop: wait delayBeforeSequence, run sequence, repeat.
//     /// </summary>
//     public void StartTimerLoop()
//     {
//         if (timerCoroutine != null)
//         {
//             StopCoroutine(timerCoroutine);
//         }

//         timerCoroutine = StartCoroutine(TimerLoop());
//     }

//     IEnumerator TimerLoop()
//     {
//         while (true)
//         {
//             // Wait before starting the sequence
//             yield return new WaitForSeconds(delayBeforeSequence);

//             // Get sprites and positions for current cutscene
//             CutsceneSprites config = GetConfigForCutscene();
//             if (config == null || config.aliveSprite == null || config.deadSprite == null)
//             {
//                 yield return null;
//                 continue;
//             }

//             if (config.position1 == null || config.position2 == null)
//             {
//                 Debug.LogWarning("SpriteSequenceSpawner: Missing positions for cutscene type " + CutsceneState.cutsceneType);
//                 yield return null;
//                 continue;
//             }

//             // Run the sequence
//             if (sequenceCoroutine != null)
//             {
//                 StopCoroutine(sequenceCoroutine);
//             }

//             sequenceCoroutine = StartCoroutine(PlaySequence(config.aliveSprite, config.deadSprite, config.position1, config.position2));

//             // Wait for sequence to complete
//             yield return sequenceCoroutine;
//         }
//     }

//     CutsceneSprites GetConfigForCutscene()
//     {
//         switch (CutsceneState.cutsceneType)
//         {
//             case CutsceneState.CutsceneType.LookAt:
//                 return firstCutscene;
//             case CutsceneState.CutsceneType.DoorSlerp:
//                 return secondCutscene;
//             case CutsceneState.CutsceneType.Custom:
//                 return thirdCutscene;
//             default:
//                 return null;
//         }
//     }

//     /// <summary>
//     /// Manually trigger a single sprite sequence (without auto-repeat).
//     /// </summary>
//     public void PlaySingleSequence()
//     {
//         CutsceneSprites config = GetConfigForCutscene();
//         if (config == null || config.aliveSprite == null || config.deadSprite == null)
//         {
//             Debug.LogWarning("SpriteSequenceSpawner: No sprites assigned for cutscene type " + CutsceneState.cutsceneType);
//             return;
//         }

//         if (config.position1 == null || config.position2 == null)
//         {
//             Debug.LogWarning("SpriteSequenceSpawner: position1 or position2 is not assigned.");
//             return;
//         }

//         if (sequenceCoroutine != null)
//         {
//             StopCoroutine(sequenceCoroutine);
//         }

//         sequenceCoroutine = StartCoroutine(PlaySequence(config.aliveSprite, config.deadSprite, config.position1, config.position2));
//     }

//     /// <summary>
//     /// Old method for backward compatibility.
//     /// </summary>
//     public void StartSequence()
//     {
//         PlaySingleSequence();
//     }

//     (Sprite, Sprite) GetSpritesForCutscene()
//     {
//         // Deprecated - kept for backward compatibility
//         return (null, null);
//     }

//     IEnumerator PlaySequence(Sprite spriteAlive, Sprite spriteDead, Transform pos1, Transform pos2)


//     IEnumerator PlaySequence(Sprite spriteAlive, Sprite spriteDead, Transform pos1, Transform pos2)
//     {
//         // Spawn alive sprite at position 1
//         currentSprite = SpawnSpriteAt(pos1.position, spriteAlive);

//         // Wait for duration
//         yield return new WaitForSeconds(timeBetweenSprites);

//         // Delete first sprite
//         if (currentSprite != null)
//         {
//             Destroy(currentSprite);
//             currentSprite = null;
//         }

//         // Spawn dead sprite at position 2
//         currentSprite = SpawnSpriteAt(pos2.position, spriteDead);

//         // Wait until isCutscene is false
//         while (CutsceneState.isCutscene)
//         {
//             yield return null;
//         }

//         // Delete second sprite
//         if (currentSprite != null)
//         {
//             Destroy(currentSprite);
//             currentSprite = null;
//         }
//     }

//     /// <summary>
//     /// Stop the current sequence and timer loop and clean up.
//     /// </summary>
//     public void StopSequence()
//     {
//         if (timerCoroutine != null)
//         {
//             StopCoroutine(timerCoroutine);
//             timerCoroutine = null;
//         }

//         if (sequenceCoroutine != null)
//         {
//             StopCoroutine(sequenceCoroutine);
//             sequenceCoroutine = null;
//         }

//         if (currentSprite != null)
//         {
//             Destroy(currentSprite);
//             currentSprite = null;
//         }
//     }
// }
