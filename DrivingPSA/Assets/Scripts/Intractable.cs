using UnityEngine;
using UnityEngine.Events;

public class Intractable : MonoBehaviour
{
    public UnityEvent OnLookEnter;
    public UnityEvent OnLookExit;

    public UnityEvent OnFirstClickPress;
    public UnityEvent OnFirstClickRelease;

    public UnityEvent OnAnyClickPress;
    public UnityEvent OnAnyClickRelease;

    void Awake()
    {
        // Layer used by the Raycast to ensure we only ever hit Intractable objects
        gameObject.layer = LayerMask.NameToLayer("Interact");
    }
}
