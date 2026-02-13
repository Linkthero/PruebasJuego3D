using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float cronometro;
    public int velocidad;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);      // se mueve palante
        transform.localScale += new Vector3(3,3,3) * Time.deltaTime;    //se van haciendo mas grandes

        cronometro += 1 * Time.deltaTime;    //va contando el tiempo

        if(cronometro > 1f)
        {
            transform.localScale = new Vector3(1,1,1);   //vuelve a su tamaño original
            gameObject.SetActive(false);    //se desactiva
            cronometro = 0;    //reinicia el cronometro
        }
    }
}
