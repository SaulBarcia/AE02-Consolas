using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float movementSpeed = 10f;
    public float mouseSensitivity = 2f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        // Ocultar y bloquear el cursor al centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 rot = transform.localEulerAngles;
        rotationX = rot.y;
        rotationY = rot.x;
    }

    void Update()
    {
        // 1. Rotación con el ratón
        rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);

        // 2. Multiplicador de velocidad si se mantiene presionado Shift
        float currentSpeed = movementSpeed;

        // 3. Movimiento con WASD + Q/E para subir y bajar
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");     
        float upward = 0f;

        if (Input.GetKey(KeyCode.E)) upward += 1f;
        if (Input.GetKey(KeyCode.Q)) upward -= 1f;

        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical + transform.up * upward;
        transform.position += moveDirection * currentSpeed * Time.deltaTime;
    }
}

