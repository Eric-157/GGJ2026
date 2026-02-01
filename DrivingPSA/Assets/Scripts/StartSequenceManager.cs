using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages a start-sequence world-space billboard: cycles messages at game start, then resumes gameplay.
/// Assign UI elements in the Inspector (Canvas should be World Space with a solid color panel background).
/// </summary>
public class StartSequenceManager : MonoBehaviour
{
    [Header("Billboard")]
    public GameObject startBillboard; // parent GameObject containing the world-space Canvas

    [Header("Text Messages")]
    public TextMeshProUGUI messageText; // UI Text element on the billboard
    [TextArea]
    public string[] messages;
    [Tooltip("Seconds each message is shown (unscaled time).")]
    public float messageInterval = 3f;

    [Tooltip("If true, automatically start the sequence on game start.")]
    public bool autoStartOnGameStart = true;

    Coroutine sequenceCoroutine;

    void Start()
    {
        if (startBillboard != null) startBillboard.SetActive(false);

        if (autoStartOnGameStart)
        {
            StartSequence();
        }
    }

    /// <summary>
    /// Call to start the sequence manually.
    /// </summary>
    public void StartSequence()
    {
        if (startBillboard == null)
        {
            Debug.LogWarning("StartSequenceManager: startBillboard is not assigned.");
            return;
        }

        startBillboard.SetActive(true);
        CutsceneState.isCutscene = true;
        Time.timeScale = 0f; // Pause gameplay

        sequenceCoroutine = StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        // show messages using unscaled time so Time.timeScale=0 doesn't pause them
        if (messages != null && messages.Length > 0 && messageText != null)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                messageText.text = messages[i];
                yield return new WaitForSecondsRealtime(messageInterval);
            }
        }

        // sequence done: resume gameplay
        if (startBillboard != null) startBillboard.SetActive(false);
        CutsceneState.isCutscene = false;
        Time.timeScale = 1f;
    }
}
