using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickAndDrop : MonoBehaviour
{
    [Header("Mochila")]
    [SerializeField] private Transform mano;

    [Header("Input System")]
    [Tooltip("Opcional: arrastra aquí la acción 'Soltar' (InputActionReference)." +
        " Si lo dejas vacío, se busca por nombre en PlayerInput.")]
    [SerializeField] private InputActionReference soltarActionRef;

    [SerializeField] private string soltarActionName = "Soltar";

    [Header("Drop")]
    [SerializeField] private Vector3 dropOffset = new Vector3(2f, 0f, 0);

    private GameObject objetoEnMochila;

    private InputAction soltarAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(soltarActionRef != null)
        {
            soltarAction = soltarActionRef.action; // Obtener la acción desde la referencia
        }
        else
        {
            var playerInput = GetComponent<PlayerInput>(); // Obtener el componente PlayerInput
            if(playerInput != null)
            {
                // Buscar la acción por nombre en PlayerInput
                soltarAction = playerInput.actions.FindAction(soltarActionName, throwIfNotFound:false);
            }
        }
    }

    private void OnEnable()
    {
        if(soltarAction != null)
        {
            soltarAction.performed += OnSoltarPerformed;
            soltarAction.Enable();
        }
    }

    private void OnSoltarPerformed(InputAction.CallbackContext context) => Soltar(); // Llamado cuando se realiza la acción de soltar

    private void Soltar()
    {
        if(objetoEnMochila == null)
        {
            return; // No hay ningún objeto en la mochila para soltar
        }

        objetoEnMochila.transform.SetParent(null); // Quitar el padre del objeto
        objetoEnMochila.transform.position = transform.TransformPoint(dropOffset); // Colocar el objeto en la posición de soltado relativa al jugador
        //if(objetoEnMochila.TryGetComponent<Rigidbody>(out var rb))
        //{
        //    rb.isKinematic = false; // Hacer el Rigidbody no cinemático para permitir interacciones físicas
        //}
        objetoEnMochila = null; // Vaciar la referencia al objeto en la mochila
    }

    private void OnTriggerEnter(Collider other) => TryPick(other.gameObject); // Intentar recoger un objeto al entrar en el trigger

    private void TryPick(GameObject go)
    {
        if(objetoEnMochila != null)
        {
            return; // Ya hay un objeto en la mochila
        }

        if (!go.CompareTag("Pick"))
        {
            return; // El objeto no tiene la etiqueta "Pick"
        }

        objetoEnMochila = go;
        if(objetoEnMochila.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero; // Detener cualquier movimiento del objeto
            rb.angularVelocity = Vector3.zero; // Detener cualquier rotación del objeto
            rb.isKinematic = true; // Hacer el Rigidbody cinemático para evitar interacciones físicas
        }

        objetoEnMochila.transform.SetParent(mano, worldPositionStays:false); // Establecer la mochila como padre del objeto
        objetoEnMochila.transform.localPosition = Vector3.zero; // Colocar el objeto en la posición local (0,0,0) de la mochila
        objetoEnMochila.transform.localRotation = Quaternion.identity; // Restablecer la rotación local del objeto
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
