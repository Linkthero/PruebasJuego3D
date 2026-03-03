using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossPaquirrin : Boss
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

    [Header("Invocar Minioms")]
    public GameObject prefabMiniom;
    private List<GameObject> listaMinioms;


    public bool curando = false;
    public bool invocando = false;

    
    public int hit_select;



    private void Start()
    {
        listaMinioms = new List<GameObject>();
    }


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
                rutina = Random.Range(0, 4);
                //Debug.Log("Rutina: " + rutina);
                cronometro = 0;
            }

            if (Vector3.Distance(transform.position, target.transform.position) > 1 && !atacando)   
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
                        //Debug.Log("CORRE");
                        break;

                    case 1:
                        //ATAQUE MELEE
                        if (Vector3.Distance(transform.position, target.transform.position) < 1)
                        {
                            //Debug.Log("ATACA");
                            atacando = true;
                            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2); //gira hacia el jugador
                            animator.SetBool("walk", false);
                            animator.SetBool("run", false);
                            animator.SetBool("attack", true);
                            animator.SetFloat("skills", 0);
                            
                        } 
                        
                        break;

                    case 2:
                        //CURA 
                        if ((HP_Min < HP_Max * 0.5f) && !curando) //si la vida es menor al 50%, se cura un 20% de su vida maxima, pero no puede superar su vida maxima
                        {
                            curando = true;
                            animator.SetBool("walk", false);
                            animator.SetBool("run", false);
                            animator.SetBool("attack", true);
                            animator.SetFloat("skills", 0.1f);

                            HP_Min += HP_Max * 0.2f;
                            barra.fillAmount = HP_Min / HP_Max;
                            Debug.Log("CURA");
                        } 
                        break;

                    case 3:
                        //SPAWNEA ENEMIGOS
                        if((listaMinioms.Count < 4) && !invocando)
                        {
                            invocando = true;
                            animator.SetBool("walk", false);
                            animator.SetBool("run", false);
                            animator.SetBool("attack", true);
                            animator.SetFloat("skills", 0.2f);
                        } 
                            break;

                }
            }
        } else
        {
            //si el jugador no está cerca se queda tieso
            animator.SetBool("walk", false);
            animator.SetBool("run", false);
            animator.SetBool("attack", false);
        }
    }


    //NO TOCAR, se llama al final de las animaciones de ataque
    public void Final_AniBoss()
    {
        rutina = 0;
        animator.SetBool("attack", false);
        atacando = false;
        curando = false;
        invocando = false;
        rango.GetComponent<CapsuleCollider>().enabled = true;
    }

    //Invoca bichos
    public void Invocar()
    {
        listaMinioms.Add(Instantiate(prefabMiniom, transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)), Quaternion.identity)); //spawnea un miniom en una posicion aleatoria alrededor del boss
    }

    //Activa collider de arma cuerpo a cuerpo
    public void ColliderWeaponTrue()
    {
        hit[hit_select].GetComponent<SphereCollider>().enabled = true;
    }

    //Desactiva collider de arma cuerpo a cuerpo
    public void ColliderWeaponFalse()
    {
        hit[hit_select].GetComponent<SphereCollider>().enabled = false;
    }


    public void Vivo() { 
    //{
    //    if(HP_Min < 500)
    //    {
    //        fase = 2;
    //        time_rutinas = 1;
    //    }

        ComportamientoBoss();

        //if(lanzallamas)
        //{
        //    Lanzallamas_Skill();
        //}
    }

    private void Update()
    {
        barra.fillAmount = HP_Min / HP_Max;
        if(HP_Min > 0)
        {
            Vivo();
        } else
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

