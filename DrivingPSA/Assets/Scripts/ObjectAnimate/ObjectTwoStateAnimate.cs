using UnityEngine;

public class ObjectTwoStateAnimate : MonoBehaviour
{
    public float speed = 1;

    [Header("Position")]
    public Vector3 startingPosition;
    public Vector3 endingPosition;
    public float distanceThresholdPosition = 1;

    [Header("Rotation")]
    public Vector3 startingRotation;
    public Vector3 endingRotation;
    public float distanceThresholdRotation = 1;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip audioClipStart;
    public AudioClip audioClipEnd;

    private bool isInteracted = false; // What's our actual, latest interaction state
    private bool isHeadingToEnding = false; // What are we trying to do right now

    private Vector3 currentPosition;
    private Vector3 currentRotation;


    void Start()
    {
        // Make sure our starting values all are consistent
        currentPosition = startingPosition;
        transform.localPosition = startingPosition;

        currentRotation = startingRotation;
        transform.localRotation = Quaternion.Euler(startingRotation);
    }

    void Update()
    {
        var targetPosition = isHeadingToEnding ? endingPosition : startingPosition;
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, speed * Time.deltaTime);
        transform.localPosition = currentPosition;

        // We keep our own rotation values to avoid trying to Lerp with like .Rotate or some shit
        var targetRotation = isHeadingToEnding ? endingRotation : startingRotation;
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, speed * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);

        // Distances and thresholds because of floating point math
        var finishedPosition = Vector3.Distance(currentPosition, targetPosition) <= distanceThresholdPosition;
        var finishedRotation = Vector3.Distance(currentRotation, targetRotation) <= distanceThresholdRotation;

        // If animation has gone far enough, we update what we're trying to do based on our current interaction state
        if (finishedPosition && finishedRotation)
        {
            isHeadingToEnding = isInteracted;
        }
    }

    public void StartInteraction()
    {
        isInteracted = true;
    }

    public void StopInteraction()
    {
        isInteracted = false;
    }

    public void ToggleInteraction()
    {
        isInteracted = !isInteracted;

        if (audioSource == null) return;

        if (isInteracted && audioClipStart != null)
        {
            audioSource.PlayOneShot(audioClipStart);
        }

        if (!isInteracted && audioClipEnd != null)
        {
            audioSource.PlayOneShot(audioClipEnd);
        }
    }
}
