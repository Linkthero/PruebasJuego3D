using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))] // Asegura que el GameObject tenga un CharacterController
public class SlopeSlideOnly : MonoBehaviour
{
    [Header("Deslizamiento pendiente")]
    [SerializeField] private float slideSpeed = 4f; // Velocidad de deslizamiento
    [SerializeField] private float minSlopeAngleToSlide = 3f; // Ángulo mínimo de pendiente para iniciar el deslizamiento

    private CharacterController controller; // Referencia al CharacterController del jugador

    private Vector2 moveInput; // Almacena la entrada de movimiento del jugador


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>(); // Obtener el CharacterController del jugador
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>(); // Obtener la entrada de movimiento del jugador
    }

    // Update is called once per frame
    void Update()
    {
        if (!controller.isGrounded)
        {
            return; // Solo aplicar deslizamiento si el jugador está en el suelo
        }
        
        bool hasInput = moveInput.sqrMagnitude > 0.01f; // Comprobar si hay entrada de movimiento significativa
        if (hasInput)
        {
            return; // No aplicar deslizamiento si el jugador está moviéndose
        }

        if (!IsOnSlope(out RaycastHit hitInfo))
        {
            return; // No aplicar deslizamiento si no está en una pendiente
        }

        float angle = Vector3.Angle(hitInfo.normal, Vector3.up); // Calcular el ángulo de la pendiente

        if(angle < minSlopeAngleToSlide)
        {
            return; // No aplicar deslizamiento si la pendiente es menor que el umbral
        }

        Vector3 slideDir = Vector3.ProjectOnPlane(Vector3.down, hitInfo.normal).normalized; // Calcular la dirección de deslizamiento
        Vector3 displacement = slideDir * slideSpeed * Time.deltaTime; // Calcular el desplazamiento de deslizamiento
        controller.Move(displacement); // Mover el CharacterController en la dirección de deslizamiento
    }

    bool IsOnSlope(out RaycastHit hit)
    {
        Vector3 origin = controller.bounds.center; // Centro del CharacterController
        float rayLength = (controller.height / 2f + 0.5f); // Longitud del rayo para detectar el suelo

        // Lanzar un rayo hacia abajo para detectar el suelo
        if (Physics.Raycast(origin, Vector3.down, out hit, rayLength))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up); // Calcular el ángulo entre la normal del suelo y la vertical 
            return angle > 0.01f; // Devolver true si el ángulo es mayor que un pequeño umbral
        }
        return false; // Devolver false si no se detecta suelo
    }

}
