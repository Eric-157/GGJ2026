using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLookDetection : MonoBehaviour
{
    new public Camera camera;
    public LayerMask layerMask;

    private Intractable previousTarget;
    private FirstClickState firstClickState;

    void Update()
    {
        var mouseScreenPosition = Mouse.current.position.ReadValue();

        var cameraRay = camera.ScreenPointToRay(mouseScreenPosition);

        // We hit a target with Intractable
        if (Physics.Raycast(cameraRay, out var hit, 100, layerMask) &&
            hit.collider.gameObject.TryGetComponent<Intractable>(out var currentTarget))
        {
            // We had no previous target
            if (previousTarget == null)
            {
                HandleLookEnter(currentTarget);
            }
            // We had a previous target and it's different from our current target
            else if (currentTarget.gameObject != previousTarget.gameObject)
            {
                HandleLookExit();
                HandleLookEnter(currentTarget);
            }

            HandleClicking(currentTarget);
        }

        // Raycast hit nothing, if we had a previous target we're no longer looking at it
        else if (previousTarget != null)
        {
            HandleLookExit();
        }
    }

    private void HandleLookEnter(Intractable currentTarget)
    {
        currentTarget.OnLookEnter.Invoke();
        previousTarget = currentTarget;
    }

    private void HandleLookExit()
    {
        firstClickState = FirstClickState.HaveNot;
        previousTarget.OnLookExit.Invoke();
        previousTarget = null;
    }

    private void HandleClicking(Intractable currentTarget)
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            currentTarget.OnAnyClickPress.Invoke();

            if (firstClickState == FirstClickState.HaveNot)
            {
                firstClickState = FirstClickState.CurrentlyPressed;
                currentTarget.OnFirstClickPress.Invoke();
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            currentTarget.OnAnyClickRelease.Invoke();

            if (firstClickState == FirstClickState.CurrentlyPressed)
            {
                firstClickState = FirstClickState.HaveReleased;
                currentTarget.OnFirstClickRelease.Invoke();
            }
        }
    }

    enum FirstClickState
    {
        HaveNot,
        CurrentlyPressed,
        HaveReleased,
    }
}
