using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{

    public float movementSpeed = 20;

    public float minHorizontal = -90;
    public float maxHorizontal = 90;

    public float minVertical = -45;
    public float maxVertical = 45;

    private float angleVertical = 0;
    private float angleHorizontal = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        var input = Mouse.current.delta.ReadValue();

        angleVertical += -input.y * movementSpeed * Time.deltaTime;
        angleHorizontal += input.x * movementSpeed * Time.deltaTime;

        angleVertical = Math.Clamp(angleVertical, minVertical, maxVertical);
        angleHorizontal = Math.Clamp(angleHorizontal, minHorizontal, maxHorizontal);

        transform.localRotation = Quaternion.Euler(angleVertical, angleHorizontal, 0);
    }
}
