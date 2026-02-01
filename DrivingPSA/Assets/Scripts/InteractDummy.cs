using UnityEngine;

public class InteractDummy : MonoBehaviour
{
    new public Renderer renderer;

    [Header("State Colors")]
    public Color none = Color.white;
    public Color looking = Color.blue;
    public Color firstClick = Color.red;
    public Color otherClick = Color.yellow;

    public void LookEnter()
    {
        renderer.material.color = looking;
    }

    public void LookExit()
    {
        renderer.material.color = none;
    }

    public void AnyClickPressed()
    {
        renderer.material.color = otherClick;
    }

    public void AnyClickReleased()
    {
        renderer.material.color = looking;
    }

    public void FirstClickPressed()
    {
        renderer.material.color = firstClick;
    }

    public void FirstClickReleased()
    {
        renderer.material.color = looking;
    }
}
