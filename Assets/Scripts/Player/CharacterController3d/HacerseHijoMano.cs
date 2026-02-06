using UnityEngine;

public class HacerseHijoMano : MonoBehaviour
{
    [SerializeField] private GameObject mano; // Asigna el objeto "Mano" desde el Inspector


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mano")) // Verifica si el objeto que colisiona tiene la etiqueta "Mano"
        {
            transform.SetParent(mano.transform); // Establece el objeto actual como hijo del objeto "Mano"
            transform.localPosition = new Vector3(-0.546999991f, -0.351999998f, -0.275000006f); // Ajusta la posición local del objeto hijo
            transform.localRotation = Quaternion.Euler(7.22930861f, 329.437012f, 126.729393f); // Ajusta la rotación local del objeto hijo
        }
    }
}
