using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))] // Asegura que el GameObject tenga un CharacterController

public class PlayerAnimations : MonoBehaviour // Clase para manejar las animaciones del jugador
{
    [SerializeField] private Animator animator; // Referencia al componente Animator
    [SerializeField] private CharacterController characterController; // Referencia al CharacterController

    [Tooltip("Velocidad máxima usada para normalizar la velocidad de movimiento")]
    [SerializeField] private float maxSpeed = 1f; // Velocidad máxima del jugador para normalizar

    private Vector3 movimientoLocal; // Vector de movimiento local del jugador


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(characterController == null)
        {
            characterController = GetComponent<CharacterController>(); // Obtener el CharacterController si no está asignado
        }

        if(animator == null)
        {
            animator = GetComponentInChildren<Animator>(); // Obtener el Animator si no está asignado
        }
    }

    // Update is called once per frame
    void Update()
    {
        ActualizarMovimiento(); // Actualizar el movimiento del jugador
    }

    private void ActualizarMovimiento() // Actualiza la velocidad de movimiento del jugador
    {
        Vector3 velocidad = characterController.velocity; // Obtener la velocidad del CharacterController
        movimientoLocal = transform.InverseTransformDirection(velocidad); // Convertir la velocidad a espacio local
        float x = movimientoLocal.x; // Componente X del movimiento local
        float y = movimientoLocal.y; // Componente Z del movimiento local

        if(maxSpeed > 0f) // Evitar división por cero
        {
            x /= maxSpeed; // Normalizar la componente X
            y /= maxSpeed; // Normalizar la componente Y
        }

        animator.SetFloat("X", x); // Actualizar el parámetro "X" del Animator
        animator.SetFloat("Y", y); // Actualizar el parámetro "Y" del Animator
        animator.SetBool("EnSuelo", characterController.isGrounded); // Actualizar el parámetro "EnSuelo" del Animator

    }
}
