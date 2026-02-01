using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    [Tooltip("Movement speed in units per second.")]
    public float speed = 5f;

    [Tooltip("Direction of movement in world space. Default moves along -Z.")]
    public Vector3 moveDirection = Vector3.forward;

    void Start()
    {
        // normalize direction to ensure consistent speed
        if (moveDirection == Vector3.zero) moveDirection = Vector3.forward;
        moveDirection = moveDirection.normalized;
    }

    void Update()
    {
        // Only move when not in a cutscene
        if (!CutsceneState.isCutscene)
        {
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Destroy this object if it collides with the trigger object named "ObjectKiller".
        if (other.gameObject.name == "ObjectKiller" || other.CompareTag("ObjectKiller"))
        {
            Debug.Log("ObjectMovement: Collided with ObjectKiller, destroying self.");
            Destroy(gameObject);
        }
    }
}
