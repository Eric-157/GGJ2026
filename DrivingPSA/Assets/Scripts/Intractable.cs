using UnityEngine;
using UnityEngine.Events;

public class Intractable : MonoBehaviour
{
    void Awake()
    {
        // Layer used by the Raycast to ensure we only ever hit Intractable objects
        gameObject.layer = LayerMask.NameToLayer("Interact");
    }

    public UnityEvent OnLookEnter;
    public UnityEvent OnLookExit;
}
