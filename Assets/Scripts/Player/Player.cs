using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float HP_Min;
    public float HP_Max;
    public Image barra;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        barra.fillAmount = HP_Min / HP_Max;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ataque"))
        {
            Debug.Log("Hace pupa");
            other.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
