using System;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity = 1.5f; // Camera sensitivity
    [SerializeField] private float mouseSmooth = 20f; // Speed ​​and smoothness of the camera
    [SerializeField] private Transform horizontalTargetBody; // Player body that will be rotated horizontally
    [SerializeField] private bool useSmooth = false; // Enable or disable the camera softness option

    [Header("Clamp Settings")]
    [SerializeField] private float clampMaxAngle = 90f;
    [SerializeField] private float clampMinAngle = -90f;

    private Vector3 _currentMouseRot;

    private void Start()
    {
        LockCursor(true);
    }

    private void Update()
    {
        // Calculate mouse look
        Vector2 mouseInput = new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        float mouseSpeed = useSmooth ? (mouseSmooth * Time.deltaTime) : Mathf.Infinity;

        _currentMouseRot.y += -1 * mouseInput.y * mouseSensitivity;
        _currentMouseRot.x += mouseInput.x * mouseSensitivity;

        // Clamp vertical look rotation
        _currentMouseRot.y = Mathf.Clamp(_currentMouseRot.y, -Mathf.Abs(clampMaxAngle), Mathf.Abs(clampMinAngle));

        // Apply final rotation
        Quaternion vertical = Quaternion.Euler(new(_currentMouseRot.y, 0f, 0f));
        Quaternion horizontal = Quaternion.Euler(new(0f, _currentMouseRot.x, 0f));

        transform.localRotation = Quaternion.Slerp(transform.localRotation, vertical, mouseSpeed);
        horizontalTargetBody.localRotation = Quaternion.Slerp(horizontalTargetBody.localRotation, horizontal, mouseSpeed);
    }

    public void LockCursor(bool locked)
    {
        // Lock cursor on screen
        Cursor.visible = locked;
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
