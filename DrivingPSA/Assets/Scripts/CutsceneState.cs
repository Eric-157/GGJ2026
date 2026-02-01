using UnityEngine;

/// <summary>
/// Simple global cutscene flag. Set `CutsceneState.isCutscene = true` from your cutscene manager to pause object movement.
/// </summary>
public static class CutsceneState
{
    public static bool isCutscene = false;
}
