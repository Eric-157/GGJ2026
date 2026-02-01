using UnityEngine;

public class InteractDummy : MonoBehaviour
{
    new public Renderer renderer;

    public void LookEnter()
    {
        renderer.material.color = Color.blue;
    }

    public void LookExit()
    {
        renderer.material.color = Color.white;
    }
}
