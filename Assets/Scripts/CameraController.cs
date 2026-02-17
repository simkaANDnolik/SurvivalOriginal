using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float height = 2f;
    [SerializeField] private float sensitivity = 2f;
    [SerializeField] private float smoothSpeed = 10f;
    [SerializeField] private Vector2 verticalClamp = new Vector2(-30f, 60f);

    private float currentX = 0f;
    private float currentY = 0f;
    private Vector3 cameraOffset;

    private void Start()
    {
        cameraOffset = new Vector3(0, height, -distance);

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        Vector3 angles = transform.eulerAngles;
        currentX = angles.y;
        currentY = angles.x;
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(1)) // Правая кнопка мыши для вращения
        {
            currentX += Input.GetAxis("Mouse X") * sensitivity;
            currentY -= Input.GetAxis("Mouse Y") * sensitivity;
            currentY = Mathf.Clamp(currentY, verticalClamp.x, verticalClamp.y);
        }

        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = target.position + rotation * cameraOffset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * height * 0.5f);
    }
}
