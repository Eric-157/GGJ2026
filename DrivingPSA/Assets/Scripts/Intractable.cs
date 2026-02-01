using UnityEngine;
using UnityEngine.Events;

public class Intractable : MonoBehaviour
{
    void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Interact");
    }

    public UnityEvent OnLookEnter;
    public UnityEvent OnLookExit;
}
