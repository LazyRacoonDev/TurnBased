using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float moveSpeed = 10f;          // Velocidad de movimiento de la cámara
    public float zoomSpeed = 20f;         // Velocidad de zoom
    public float minZoom = 5f;            // Límite inferior de zoom
    public float maxZoom = 50f;           // Límite superior de zoom
    public float rotationSpeed = 100f;    // Velocidad de rotación de la cámara

    private GameObject targetObject;      // Objeto objetivo al que la cámara se centrará
    private bool isCentered = false;      // Estado de si la cámara está centrada sobre el objetivo
    private Vector3 fixedOffset;          // Offset fijo para mantener una distancia constante al seguir

    private float targetZoom;             // Almacena el nivel de zoom objetivo
    private float zoomSmoothTime = 0.2f;  // Tiempo de suavizado del zoom
    private float zoomVelocity = 0f;      // Velocidad actual de zoom (para SmoothDamp)

    void Start()
    {
        // Buscar automáticamente el objeto con el tag "Player"
        targetObject = GameObject.FindWithTag("Player");
        if (targetObject == null)
        {
            Debug.LogWarning("No object with tag 'Player' found!");
        }
        else
        {
            // Calcular offset inicial
            fixedOffset = transform.position - targetObject.transform.position;
        }

        targetZoom = fixedOffset.magnitude; // Inicializar con la distancia actual al objetivo
    }

    void Update()
    {
        // Alternar centrado de la cámara con la tecla C
        if (Input.GetKeyDown(KeyCode.C) && targetObject != null)
        {
            isCentered = !isCentered;
            if (isCentered)
            {
                fixedOffset = transform.position - targetObject.transform.position; // Actualizar offset
                targetZoom = fixedOffset.magnitude; // Asegurarse de que el zoom respete el offset actual
            }
        }

        // Desactivar centrado si se usa WASD
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            isCentered = false;
        }

        // Lógica de centrado o movimiento libre
        if (isCentered && targetObject != null)
        {
            FollowTargetWithZoom();
            HandleOrbitalRotation(); // Permitir rotación orbital
        }
        else
        {
            MoveCamera();
        }

        HandleZoom();
    }

    void MoveCamera()
    {
        // Movimiento de la cámara con WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }

    void HandleZoom()
    {
        // Leer entrada de zoom (rueda del ratón)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            targetZoom = Mathf.Clamp(targetZoom - scroll * zoomSpeed, minZoom, maxZoom);
        }

        // Aplicar zoom gradual
        if (isCentered && targetObject != null)
        {
            // Ajustar el offset para reflejar el nuevo zoom
            fixedOffset = fixedOffset.normalized * Mathf.SmoothDamp(fixedOffset.magnitude, targetZoom, ref zoomVelocity, zoomSmoothTime);
        }
        else
        {
            // Ajustar la posición directamente cuando no está centrada
            float currentZoom = transform.position.y;
            float smoothedZoom = Mathf.SmoothDamp(currentZoom, targetZoom, ref zoomVelocity, zoomSmoothTime);
            transform.position = new Vector3(transform.position.x, smoothedZoom, transform.position.z);
        }
    }

    void HandleOrbitalRotation()
    {
        if (Input.GetMouseButton(2)) // Botón central del ratón
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Rotar alrededor del objetivo en función de la entrada del ratón
            transform.RotateAround(targetObject.transform.position, Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
            transform.RotateAround(targetObject.transform.position, transform.right, -mouseY * rotationSpeed * Time.deltaTime);

            // Actualizar el offset para mantener la posición relativa actual
            fixedOffset = transform.position - targetObject.transform.position;
        }
    }

    void FollowTargetWithZoom()
    {
        // Seguir al objetivo con el offset ajustado al zoom
        transform.position = targetObject.transform.position + fixedOffset;
        transform.LookAt(targetObject.transform);
    }
}