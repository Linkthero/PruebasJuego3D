using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCigala : Boss
{
    //Codigo enemigo base //
    [Header("Codigo enemigo base")]
    public int rutina;
    public float cronometro;
    public float time_rutinas;
    public Quaternion angulo;
    public float grado;
    public bool atacando;
    public RangoBoss rango;
    public GameObject[] hit;


    public int hit_select;

    [Header("Lanzallamas")]
    ///////////Lanzallamas//////////////
    public bool lanzallamas;
    public List<GameObject> pool = new List<GameObject>();  //almacena las esferas
    public GameObject fire;     //prefab de esferas
    public GameObject cabeza;   //punto donde salen las esferas
    private float cronometro2;      //tiempo entre esferas



    ///COMPORTAMIENTO DEL BOSS, SE LLAMA EN EL UPDATE//
    public void ComportamientoBoss()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 15)       //si el jugador esta a menos de 15 metros, el boss lo persigue y ataca
        {
            var lookPos = target.transform.position - transform.position;   //calcula la direccion hacia el jugador
            lookPos.y = 0; //para que no mire hacia arriba o abajo, solo en el plano horizontal
            var rotation = Quaternion.LookRotation(lookPos); //gira hacia el jugador

            cronometro += 1 * Time.deltaTime;   //aumenta el cronometro para cambiar de rutina cada cierto tiempo
            if (cronometro > time_rutinas)  //si el cronometro es mayor que el tiempo de rutina, cambia de rutina aleatoriamente entre 0, 1 y 2  y resetea el cronometro
            {
                rutina = Random.Range(0, 3);
                cronometro = 0;
            }

            if (Vector3.Distance(transform.position, target.transform.position) > 1 && !atacando)   //
            {
                switch (rutina)
                {
                    case 0:
                        //RUN
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2); //gira hacia el jugador
                        animator.SetBool("walk", false);
                        animator.SetBool("run", true);
                        animator.SetBool("attack", false);
                        if (transform.rotation == rotation)
                        {
                            transform.Translate(Vector3.forward * velocidadPersecucion * Time.deltaTime);   //se mueve hacia el jugador
                        }
                        break;

                    case 1:
                        //ATAQUE AREA
                        if (Vector3.Distance(transform.position, target.transform.position) < 2)
                        {
                            hit_select = 0;
                            atacando = true;
                            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2); //gira hacia el jugador
                            animator.SetBool("walk", false);
                            animator.SetBool("run", false);
                            animator.SetBool("attack", true);
                            animator.SetFloat("skills", 1);

                        }

                            break;

                    case 2:
                        //Lanzallamas
                        hit_select = 1;
                        atacando = true;
                        animator.SetBool("walk", false);
                        animator.SetBool("run", false);
                        animator.SetBool("attack", true);
                        animator.SetFloat("skills", 0);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                        //rango.GetComponent<CapsuleCollider>().enabled = false;

                        break;
                }
            }
        } else
         {
            //si jugador no esta cerca se queda tieso
            animator.SetBool("walk", false);
            animator.SetBool("run", false);
            animator.SetBool("attack", false);
         }
    }

    //NO TOCAR s llama en animaciones
    public void Final_AniBoss()
    {
        rutina = 0;
        animator.SetBool("attack", false);
        atacando = false;
        //rango.GetComponent<CapsuleCollider>().enabled = true;
        lanzallamas = false;
    }
    //activa collider arma
    public void ColliderWeaponTrue()
    {
        hit[hit_select].GetComponent<SphereCollider>().enabled = true;
    }
    //desactiva collider arma
    public void ColliderWeaponFalse()
    {
        hit[hit_select].GetComponent<SphereCollider>().enabled = false;
    }

    //obtiene bala
    public GameObject GetBala()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                return pool[i];
            }
        }
        GameObject obj = Instantiate(fire, cabeza.transform.position, cabeza.transform.rotation) as GameObject;
        pool.Add(obj);
        return obj;
    }

    //Ataque Lanzallamas(ataque 1)
    public void Lanzallamas_Skill()
    {
        cronometro2 += 1 * Time.deltaTime;
        if (cronometro2 > 0.1f)
        {
            GameObject bala = GetBala();
            bala.transform.position = cabeza.transform.position;
            bala.transform.rotation = cabeza.transform.rotation;
            cronometro2 = 0;
        }
    }
    //metodo auxiliar de lanzallamas
    public void Start_Fire()
    {
        lanzallamas = true;
    }

    //metodo auxiliar de lanzallanmas
    public void Stop_Fire()
    {
        lanzallamas = false;
    }

    //qué hace si está vivo
    public void Vivo()
    {
        ComportamientoBoss();

    }

    private void Update()
    {
        barra.fillAmount = HP_Min / HP_Max;
        if (HP_Min > 0)
        {
            Vivo();
        }
        else
        {
            if (!muerto)
            {
                animator.SetBool("dead", true);
                muerto = true;
                //musica.enabled = false;
            }
        }
    }
}
