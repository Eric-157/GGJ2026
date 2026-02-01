using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLookDetection : MonoBehaviour
{
    new public Camera camera;
    public LayerMask layerMask;

    private Intractable previousTarget;

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
                currentTarget.OnLookEnter.Invoke();
            }
            // We had a previous target and it's different from our current target
            else if (currentTarget.gameObject != previousTarget.gameObject)
            {
                previousTarget.OnLookExit.Invoke();
                currentTarget.OnLookEnter.Invoke();
            }

            previousTarget = currentTarget;
        }

        // Raycast hit nothing, if we had a previous target we're no longer looking at it
        else if (previousTarget != null)
        {
            previousTarget.OnLookExit.Invoke();
            previousTarget = null;
        }
    }
}
