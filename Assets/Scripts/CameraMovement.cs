using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float moveSpeed = 10f;          // Velocidad de movimiento de la cámara
    public float zoomSpeed = 20f;          // Velocidad de zoom
    public float minZoom = 5f;             // Límite inferior de zoom (distancia mínima)
    public float maxZoom = 50f;            // Límite superior de zoom (distancia máxima)
    public float rotationSpeed = 100f;     // Velocidad de rotación de la cámara

    public GameObject targetObject;        // Objeto objetivo al que la cámara se centrará
    private bool isCentered = false;       // Estado de si la cámara está centrada sobre el objetivo
    private Vector3 fixedOffset;           // Offset fijo para mantener una distancia constante al seguir

    void Start()
    {
        // Establecer un offset inicial para mantener una distancia fija cuando se centra en el objetivo
        if (targetObject != null)
        {
            fixedOffset = transform.position - targetObject.transform.position;
        }
    }

    void Update()
    {
        // Alternar centrado de la cámara al presionar la tecla C
        if (Input.GetKeyDown(KeyCode.C) && targetObject != null)
        {
            // Alternar el estado de centrado sin cambiar la posición actual de la cámara
            isCentered = !isCentered;
        }

        // Si está centrada, seguir al objetivo con el offset fijo
        if (isCentered && targetObject != null)
        {
            FollowTargetWithFixedOffset();
        }
        else
        {
            // Movimiento de la cámara con WASD (solo si no está centrada)
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(horizontal, 0, vertical);
            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
        }

        // Zoom con la rueda del ratón
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            float newZoom = Mathf.Clamp(transform.position.y - scroll * zoomSpeed, minZoom, maxZoom);
            transform.position = new Vector3(transform.position.x, newZoom, transform.position.z);
        }

        // Rotar la cámara al pulsar la rueda del ratón
        if (Input.GetMouseButton(2)) // Botón central del ratón
        {
            RotateCamera();
        }
    }

    // Método para seguir al objetivo manteniendo un offset fijo
    void FollowTargetWithFixedOffset()
    {
        // Actualizar la posición de la cámara en función de la posición del objetivo y el offset fijo
        transform.position = targetObject.transform.position + fixedOffset;
        transform.LookAt(targetObject.transform);
    }

    // Método para rotar la cámara
    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotar alrededor del eje Y (horizontal) y X (vertical)
        transform.RotateAround(transform.position, Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
        transform.RotateAround(transform.position, transform.right, -mouseY * rotationSpeed * Time.deltaTime);

        // Mantener el offset actualizado si la cámara está centrada
        if (isCentered && targetObject != null)
        {
            fixedOffset = transform.position - targetObject.transform.position;
        }
    }
}