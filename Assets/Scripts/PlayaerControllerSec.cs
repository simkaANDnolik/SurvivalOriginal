using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayaerControllerSec : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float cameraSensitivity = 2f;
    [SerializeField] private float cameraDistance = 5f;
    [SerializeField] private float cameraHeight = 2f;
    [SerializeField] private Vector2 verticalClamp = new Vector2(-30f, 60f);

    private CharacterController characterController;
    private float cameraPitch = 0f;
    private float cameraYaw = 0f;
    private Vector3 cameraOffset;
    public static int PLAYER_HP = 100;

    private void Start()
    {

        characterController = GetComponent <CharacterController>();

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        // Инициализация позиции камеры
        cameraOffset = new Vector3(0, cameraHeight, -cameraDistance);

        // Инициализация углов камеры
        Vector3 angles = cameraTransform.eulerAngles;
        cameraYaw = angles.y;
        cameraPitch = angles.x;
    }

    private void Update()
    {
        HandleCameraRotation();
        HandleMovement();
        UpdateCameraPosition();
    }

    private void HandleCameraRotation()
    {
        // Получаем ввод мыши
        float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity;

        // Обновляем углы камеры
        cameraYaw += mouseX;
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, verticalClamp.x, verticalClamp.y);

        // Поворачиваем камеру (но не персонажа)
        Quaternion cameraRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);
        cameraTransform.rotation = cameraRotation;
    }

    private void HandleMovement()
    {
        // Получаем ввод с клавиатуры
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0f || vertical != 0f)
        {
            // Получаем направление камеры (без вертикальной оси)
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            Vector3 cameraRight = cameraTransform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            // Вычисляем направление движения относительно камеры
            Vector3 moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

            // Двигаем персонажа
            characterController.SimpleMove(moveDirection * moveSpeed);

            // Плавно поворачиваем персонаж в направлении движения
            if (moveDirection != Vector3.zero) { 
Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
            rotationSpeed * Time.deltaTime);
        }
    }
}

private void UpdateCameraPosition()
{
    // Позиция камеры с учетом смещения
    Quaternion cameraRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);
    Vector3 desiredPosition = transform.position + cameraRotation * cameraOffset;

    // Плавное перемещение камеры
    cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, 10f * Time.deltaTime);

    // Камера всегда смотрит на персонажа
    cameraTransform.LookAt(transform.position + Vector3.up * cameraHeight * 0.5f);
}

// Метод для получения направления движения относительно камеры (может пригодиться для других скриптов)
public Vector3 GetCameraRelativeMovement(float horizontal, float vertical)
{
    Vector3 cameraForward = cameraTransform.forward;
    cameraForward.y = 0;
    cameraForward.Normalize();

    Vector3 cameraRight = cameraTransform.right;
    cameraRight.y = 0;
    cameraRight.Normalize();

    return (cameraForward * vertical + cameraRight * horizontal).normalized;
}
}