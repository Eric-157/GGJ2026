using UnityEngine;

public class InteractDummy : MonoBehaviour
{
    new public Renderer renderer;

    public void StartInteraction()
    {
        renderer.material.color = Color.blue;
    }

    public void StopInteraction()
    {
        renderer.material.color = Color.white;
    }
}
