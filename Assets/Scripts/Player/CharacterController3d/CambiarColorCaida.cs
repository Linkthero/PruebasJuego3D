using UnityEngine;

public class CambiarColorCaida : MonoBehaviour
{
    private Color groundedColor =Color.white;
    private Color goingUpColor = Color.red;
    private Color fallingColor = Color.blue;

    [Header("Ajustes")]
    [SerializeField] private float verticalDeadZone = 0.01f;
    [Header("Rendere que queremos colorear")]
    [SerializeField]private Renderer capsuleRenderer;

    private CharacterController characterController;

    private enum VerticalState { Grounded, GoingUp, FallingDown }
    private VerticalState currentState = VerticalState.Grounded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (capsuleRenderer == null)
        {
            foreach (var r in GetComponentsInChildren<Renderer>())
            {
                if (r.gameObject.name.Contains("Player"))
                {
                    capsuleRenderer = r;
                    break;
                }
            }
        }

        if (capsuleRenderer == null)
        {
            Debug.LogWarning("No se encontró renderer");
            return;
        }
        SetColor(groundedColor);
    }

    private void SetColor(Color color)
    {
       if (capsuleRenderer == null || capsuleRenderer.material==null)
           return;
       if (capsuleRenderer.material.HasProperty("_Color"))
           capsuleRenderer.material.color=color;
       else
       if (capsuleRenderer.material.HasProperty("_BaseColor"))
           capsuleRenderer.material.SetColor("_BaseColor", color);

    }

    // Update is called once per frame
    void Update()
    {
        if (characterController == null || capsuleRenderer == null)
            return;
        float vy=characterController.velocity.y;
        bool grounded =characterController.isGrounded;
        VerticalState newState;
        if (grounded)
            newState = VerticalState.Grounded;
        else if (vy>verticalDeadZone)
            newState = VerticalState.GoingUp;
        else if (vy<-verticalDeadZone)
            newState = VerticalState.FallingDown;
        else
            newState = currentState;
        if (newState != currentState)
        {
            currentState = newState;
            switch (currentState)
            {
                case VerticalState.Grounded: SetColor(groundedColor); break;
                case VerticalState.GoingUp: SetColor(goingUpColor); break;
                case VerticalState.FallingDown: SetColor(fallingColor); break;
            }
        }

    }
}
