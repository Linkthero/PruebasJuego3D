using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossFary : Boss
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

    //ataque magico
    public GameObject rayosPrefab;



    //Comportamiento del boss
    public void ComportamientoBoss()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 15)       //si el jugador esta a menos de 15 metros, el boss lo persigue y ataca
        {
            var lookPos = target.transform.position - transform.position;   //calcula la direccion hacia el jugador
            lookPos.y = 0; //para que no mire hacia arriba o abajo, solo en el plano horizontal
            var rotation = Quaternion.LookRotation(lookPos); //gira hacia el jugador
                                                             //musica.enabled = true;

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
                        Debug.Log("CORRE");
                        break;

                    case 1:
                        //ATAQUE MELEE
                        Debug.Log(Vector3.Distance(transform.position, target.transform.position));
                        if (Vector3.Distance(transform.position, target.transform.position) < 2)
                        {

                            Debug.Log("ATACA");
                            atacando = true;
                            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2); //gira hacia el jugador
                            animator.SetBool("walk", false);
                            animator.SetBool("run", false);
                            animator.SetBool("attack", true);
                            animator.SetFloat("skills", 0);
                            
                        } 
                        
                        break;

                    case 2:
                        //Ataque magico 
                        animator.SetBool("walk", false);
                        animator.SetBool("run", false);
                        animator.SetBool("attack", true);
                        animator.SetFloat("skills", 0.5f);
                        break;

                    case 3:
                        //BLOCK
                        animator.SetBool("walk", false);
                        animator.SetBool("run", false);
                        animator.SetBool("attack", true);
                        animator.SetFloat("skills", 1f);
                         
                            break;


                }
            }
        } else
        {
            //si no detecta jugador está tieso
            animator.SetBool("walk", false);
            animator.SetBool("run", false);
            animator.SetBool("attack", false);
        }
    }

    //NO TOCAR se llama en animaciones
    public void Final_AniBoss()
    {
        rutina = 0;
        animator.SetBool("attack", false);
        atacando = false;
        rango.GetComponent<CapsuleCollider>().enabled = true;
    }

    //ataque magico
    public void CreaRayos()
    {
        GameObject rayos = Instantiate(rayosPrefab, target.transform.position, Quaternion.identity);
        Destroy(rayos, 3f);
    }

    //activa arma cuerpo a cuerpo
    public void ColliderWeaponTrue()
    {
        hit[hit_select].GetComponent<SphereCollider>().enabled = true;
    }
    //desactiva arma cuerpo a cuerpo
    public void ColliderWeaponFalse()
    {
        hit[hit_select].GetComponent<SphereCollider>().enabled = false;
    }



    //comportamiento mientras esté vivo
    public void Vivo() { 
        ComportamientoBoss();
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
            }
        }
    }
}

