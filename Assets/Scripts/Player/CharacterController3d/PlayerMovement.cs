using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")] public float moveSpeed = 5f;

    [Header("Salto / Gravedad")] public float jumpHeight = 3f;
    public float gravity = -9.81f;

    private CharacterController characterController;

    [SerializeField] private Vector2 moveInput;
    private float verticalVelocity;
    private bool jumpRequested = false;

    [SerializeField] private AudioSource salto;
    [SerializeField] private AudioSource pasos;
    [SerializeField] private int minSpeedSound = 1;

    [SerializeField] private Animator animator;

    private bool isGrounded; // Variable para verificar si el jugador está en el suelo

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController == null)
            return;
        ControlMovimiento();
        ControlAnimacion();
        SonidoPasos();
    }
    
    private void ControlAnimacion()
    {
        Vector3 velocidad = characterController.velocity; // Obtener la velocidad del CharacterController
        Vector3 movimientoLocal = characterController.transform.InverseTransformDirection(velocidad); // Convertir la velocidad a espacio local

        //animator.SetFloat("X", movimientoLocal.x); // Actualizar el parámetro "X" del Animator
        //animator.SetFloat("Y", movimientoLocal.z); // Actualizar el parámetro "Y" del Animator
        //animator.SetBool("EnSuelo", characterController.isGrounded); // Actualizar el parámetro "EnSuelo" del Animator
        //animator.SetFloat("Z", verticalVelocity); // Actualizar el parámetro "Z" del Animator
    
    }

    private void SonidoPasos() // Reproducir sonido de pasos al moverse
    {
        if (pasos == null)
        {
            return;
        }
        Vector3 v = characterController.velocity; // Obtener la velocidad del CharacterController
        v.y = 0; // Ignorar la componente vertical de la velocidad
        bool andando = characterController.isGrounded && v.magnitude > minSpeedSound; // Comprobar si el jugador está en el suelo y se está moviendo

        if (andando)
        {
            if (!pasos.isPlaying) // Reproducir el sonido de pasos si no se está reproduciendo
            {
                pasos.Play();
            }
            else if (pasos.isPlaying) // Detener el sonido de pasos si no se está moviendo
            {
                pasos.Stop();
            }
        }
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed) // Solicitar salto al presionar el botón de salto
            jumpRequested = true; // Señalar que se ha solicitado un salto
    }
    private void ControlMovimiento()
    {
        isGrounded = characterController.isGrounded;
        //Reset vertical al tocar suelo
        if (isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        //Movimiento local XZ
        Vector3 localMove = new Vector3(moveInput.x, 0, moveInput.y);

        //convertir de local a mundo
        Vector3 worldMove = transform.TransformDirection(localMove);

        if (worldMove.sqrMagnitude > 1f)
            worldMove.Normalize();

        Vector3 horizontalVelocity = worldMove * moveSpeed;
        //Salto
        if (isGrounded && jumpRequested)
        {
            if(salto != null)
            {
                salto.Play();
            }
            animator.SetTrigger("Saltar");
            verticalVelocity =Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpRequested = false;
        }


        /////////Salto
        verticalVelocity += gravity * Time.deltaTime;
      //  Vector3 velocity = horizontalVelocity;
       // velocity.y = verticalVelocity;
       horizontalVelocity.y = verticalVelocity;
        characterController.Move(horizontalVelocity * Time.deltaTime);
    }
}