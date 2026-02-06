using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cameras")]
    [Tooltip("Si se deja vacío, se buscarán automáticamente las cámaras hijas")]
    [SerializeField] private List<Camera> cameras = new List<Camera>();

    private int currentCamera = 0;

    private void Awake()
    {
        if(cameras == null || cameras.Count == 0)
        {
            cameras = new List<Camera>(GetComponentsInChildren<Camera>()); // Buscar cámaras hijas automáticamente
        }
        SetActiveCamera(currentCamera); // Activar la cámara inicial
    }

    public void OnChangeCamera(InputValue value)
    {
        if(cameras == null || cameras.Count == 0) // Verifica que haya cámaras disponibles
            return;
        currentCamera++;

        // Si el índice de la cámara actual excede el número de cámaras, vuelve a la primera
        if (currentCamera >= cameras.Count)
        {
            currentCamera = 0; // Si se excede el número de cámaras, vuelve a la primera
        }
        SetActiveCamera(currentCamera); // Cambia a la cámara actual
    }

    private void SetActiveCamera(int index) // Activa solo la cámara en el índice dado
    {
        for (int i = 0; i < cameras.Count; i++) // Recorre todas las cámaras
        {
            bool isActive = (i == index);
            if (cameras[i] != null) // Verifica que la cámara no sea nula
            {
                cameras[i].enabled = isActive; // Activa o desactiva la cámara según corresponda
            }
        }
    }

}
