using UnityEngine;

public class CubePingPong : MonoBehaviour
{

    [Header("Puntos entre los que se mueve el cubo")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Velocidad de movimiento del cubo")]
    [SerializeField] private float velocidad = 2f;

    private Transform currentTarget;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(pointA == null || pointB == null)
        {
            Debug.LogError("PointA o PointB no están asignados en el inspector.");
            Destroy(this.gameObject);
            return;
        }

        // Inicializa la posición del cubo en el punto A
        transform.position = pointA.position;

        // Establece el objetivo inicial como el punto B
        currentTarget = pointB;

    }


    // Update is called once per frame
    void Update()
    {
        // Mueve el cubo hacia el objetivo actual
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, velocidad * Time.deltaTime);

        if(Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            // Cambia el objetivo al otro punto cuando se alcanza el objetivo actual
            currentTarget =(currentTarget == pointA) ? pointB : pointA;
        }
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Mueve el jugador junto a la plataforma
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(UnityEngine.Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
