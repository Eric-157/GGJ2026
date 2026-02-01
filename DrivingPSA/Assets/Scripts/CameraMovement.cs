using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{

    public bool allowMove;
    public float movementSpeed = 20;

    public float minHorizontal = -90;
    public float maxHorizontal = 90;

    public float minVertical = -45;
    public float maxVertical = 60;

    private float angleVertical = 0;
    private float angleHorizontal = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (allowMove)
        {
            var input = Mouse.current.delta.ReadValue();

            angleVertical += -input.y * movementSpeed * Time.deltaTime;
            angleHorizontal += input.x * movementSpeed * Time.deltaTime;

            angleVertical = Math.Clamp(angleVertical, minVertical, maxVertical);
            angleHorizontal = Math.Clamp(angleHorizontal, minHorizontal, maxHorizontal);

            // We keep track of our own angles because .Rotate and reading euler angles back for clamping is bad
            transform.localRotation = Quaternion.Euler(angleVertical, angleHorizontal, 0);
        }

    }
}
