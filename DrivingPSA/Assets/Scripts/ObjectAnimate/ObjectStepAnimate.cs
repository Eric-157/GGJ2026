using UnityEngine;

public class ObjectStepAnimate : MonoBehaviour
{
    public float speed = 1;

    [Header("Position")]
    public Vector3 stepPosition;

    [Header("Rotation")]
    public Vector3 stepRotation;

    private Vector3 targetPosition;
    private Vector3 targetRotation;

    private Vector3 currentPosition;
    private Vector3 currentRotation;


    void Update()
    {
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, speed * Time.deltaTime);
        transform.localPosition = currentPosition;

        // We keep our own rotation values to avoid trying to Lerp with like .Rotate or some shit
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, speed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void StartInteraction()
    {
        targetPosition += stepPosition;
        targetRotation += stepRotation;
    }
}
