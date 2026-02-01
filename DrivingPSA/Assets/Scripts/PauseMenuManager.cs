using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles pausing the game with Right Mouse Button and exposes methods for UI buttons.
/// Attach this to a manager GameObject and assign a world-space UI billboard to `pauseMenuBillboard`.
/// </summary>
public class PauseMenuManager : MonoBehaviour
{
    [Tooltip("Assign the world-space pause menu billboard GameObject (contains UI Canvas and Buttons).")]
    public GameObject pauseMenuBillboard;

    public bool isPaused { get; private set; } = false;

    void Start()
    {
        if (pauseMenuBillboard != null) pauseMenuBillboard.SetActive(false);
    }

    void Update()
    {
        // Right mouse button toggles pause
        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        if (pauseMenuBillboard != null) pauseMenuBillboard.SetActive(true);
        CutsceneState.isCutscene = true; // reuse existing flag to pause movement/spawning
        Time.timeScale = 0f; // pause time-based systems
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        isPaused = false;
        if (pauseMenuBillboard != null) pauseMenuBillboard.SetActive(false);
        CutsceneState.isCutscene = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        CutsceneState.isCutscene = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        // In builds this will close the application.
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
