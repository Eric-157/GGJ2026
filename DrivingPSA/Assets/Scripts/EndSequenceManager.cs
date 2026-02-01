using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

/// <summary>
/// Manages an end-sequence world-space billboard: cycles messages, then displays an end title/options screen with Restart and Quit.
/// Assign UI elements in the Inspector (Canvas should be World Space with a solid color panel background).
/// </summary>
public class EndSequenceManager : MonoBehaviour
{
    [Header("Billboard")]
    public GameObject endBillboard; // parent GameObject containing the world-space Canvas

    [Header("Text Messages")]
    public TextMeshProUGUI messageText; // UI Text element on the billboard
    [TextArea]
    public string[] messages;
    [Tooltip("Seconds each message is shown (unscaled time).")]
    public float messageInterval = 3f;

    [Header("End Screens")]
    public GameObject endTitleScreen; // shown after messages finish
    public GameObject endOptionsScreen; // contains Restart/Quit buttons
    public GameObject BlackScreen; // Basic black screen

    Coroutine sequenceCoroutine;

    void Start()
    {

        if (endBillboard != null) endBillboard.SetActive(false);
        if (endTitleScreen != null) endTitleScreen.SetActive(false);
        if (endOptionsScreen != null) endOptionsScreen.SetActive(false);
        if (BlackScreen != null) BlackScreen.SetActive(false);
    }

    /// <summary>
    /// Call to start the end sequence.
    /// </summary>

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartEndSequence();
        }
    }
    public void StartEndSequence()
    {
        if (endBillboard == null)
        {
            Debug.LogWarning("EndSequenceManager: endBillboard is not assigned.");
            return;
        }

        endBillboard.SetActive(true);
        CutsceneState.isCutscene = true;
        // Pause gameplay but use unscaled waits for the cinematic
        Time.timeScale = 0f;

        sequenceCoroutine = StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        if (BlackScreen != null)
        {
            BlackScreen.SetActive(true);
            yield return new WaitForSecondsRealtime(2f);
            BlackScreen.SetActive(false);
        }
        // show messages using unscaled time so Time.timeScale=0 doesn't pause them
        if (messages != null && messages.Length > 0 && messageText != null)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                messageText.text = messages[i];
                yield return new WaitForSecondsRealtime(messageInterval);
            }
        }
        if (BlackScreen != null)
        {
            BlackScreen.SetActive(true);
            yield return new WaitForSecondsRealtime(2f);
            BlackScreen.SetActive(false);
        }
        // messages done: show title/options
        if (endTitleScreen != null)
        {
            endTitleScreen.SetActive(true);
            yield return new WaitForSecondsRealtime(7f);
        }
        if (BlackScreen != null)
        {
            BlackScreen.SetActive(true);
            yield return new WaitForSecondsRealtime(2f);
            BlackScreen.SetActive(false);
        }
        if (endOptionsScreen != null) endOptionsScreen.SetActive(true);

        // Leave CutsceneState true to keep gameplay disabled; keep Time.timeScale at 0
    }

    /// <summary>
    /// Restart the current scene. Wire this to the Restart button.
    /// </summary>
    public void Restart()
    {
        Time.timeScale = 1f;
        CutsceneState.isCutscene = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Quit the game. Wire this to the Quit button.
    /// </summary>
    public void Quit()
    {
        Time.timeScale = 1f;
        CutsceneState.isCutscene = false;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
