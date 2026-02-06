using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Enemigo
{

    //Codigo enemigo base //
    public float time_rutinas;

    public RangoBoss rango;
    public GameObject[] hit;
    public int hit_select;

    ///////////Lanzallamas//////////////
    public bool lanzallamas;
    public List<GameObject> pool = new List<GameObject>();  //almacena las esferas
    public GameObject fire;     //prefab de esferas
    public GameObject cabeza;   //punto donde salen las esferas
    private float cronometro2;      //tiempo entre esferas

    ///////////Jump Attack/////////////////////////
    public float jump_distance;     //distancia para saltar
    public bool direction_Skill;    //direccion del salto

    ///////////FireBall//////////////////
    public GameObject fire_ball;   //prefab bola de fuego
    public GameObject point;    //de donde salen
    public List<GameObject> pool2 = new List<GameObject>();  //almacena las bolas de fuego

    public int fase = 1;    //fase boss
    public float HP_Min;    //vida minima
    public float HP_Max;    //vida maxima
    public Image barra;   //barra de vida
    public AudioSource musica; //musica batalla
    public bool muerto;     //boss muerto


    public void ComportamientoBoss()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 15)
        {
            var lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            point.transform.LookAt(target.transform.position);
            musica.enabled = true;

            if (Vector3.Distance(transform.position, target.transform.position) > 1 && !atacando)
            {
                switch (fase)
                {
                    case 0:
                        //WALK
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                        animator.SetBool("walk", true);
                        animator.SetBool("run", false);

                        if (transform.rotation == rotation)
                        {
                            transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
                        }

                        animator.SetBool("attack", false);

                        cronometro += 1 * Time.deltaTime;
                        if (cronometro > time_rutinas)
                        {
                            rutina = Random.Range(0, 3);
                            cronometro = 0;
                        }
                        break;

                    case 1:
                        //RUN
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                        animator.SetBool("walk", false);
                        animator.SetBool("run", true);

                        if (transform.rotation == rotation)
                        {
                            transform.Translate(Vector3.forward * velocidadPersecucion * Time.deltaTime);
                        }
                        animator.SetBool("attack", false);
                        break;

                    case 2:
                        ////Lanzallamas
                        //animator.SetBool("walk", false);
                        //animator.SetBool("run", false);
                        //animator.SetBool("attack", true);
                        //animator.SetFloat("skill", 0);
                        //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                        //rango.GetComponent<CapsuleCollider>().enabled = false;

                        //CURA 
                        animator.SetBool("walk", false);
                        animator.SetBool("run", false);
                        animator.SetBool("attack", true);
                        animator.SetFloat("skill", 0);

                        //aumenta vida
                        break;

                    case 3:
                        //Jumpattack
                        //if (fase == 2)
                        //{
                        //    jump_distance += 1 * Time.deltaTime;
                        //    animator.SetBool("walk", false);
                        //    animator.SetBool("run", false);
                        //    animator.SetBool("attack", true);
                        //    animator.SetFloat("skill", 0);
                        //    hit_select = 3;
                        //    rango.GetComponent<CapsuleCollider>().enabled = false;

                        //    if (direction_Skill)
                        //    {
                        //        if (jump_distance < 1f)
                        //        {
                        //            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                        //        }

                        //        transform.Translate(Vector3.forward * 8 * Time.deltaTime);
                        //    }
                        //}
                        //else
                        //{
                        //    rutina = 0;
                        //    cronometro = 0;
                        //}

                        //SPAWNEA ENEMIGOS
                        animator.SetBool("walk", false);
                        animator.SetBool("run", false);
                        animator.SetBool("attack", true);
                        animator.SetFloat("skill", 0.4f);


                        break;

                    case 4:
                        //Fireball

                        //if (fase == 2)
                        //{
                        //    animator.SetBool("walk", false);
                        //    animator.SetBool("run", false);
                        //    animator.SetBool("attack", true);
                        //    animator.SetFloat("skill", 0);
                        //    rango.GetComponent<CapsuleCollider>().enabled = false;
                        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 0.5f);
                        //}
                        //else
                        //{
                        //    rutina = 0;
                        //    cronometro = 0;
                        //}

                        //Ataque melee
                        animator.SetBool("walk", false);
                        animator.SetBool("run", false);
                        animator.SetBool("attack", true);
                        animator.SetFloat("skill", 0.2f);
                        break;

                }
            }
        }
    }

    new public void ComportamientoEnemigo()
    {
        //no hago nada
    }

    new public void Final_Ani()
    {
        rutina = 0;
        animator.SetBool("attack", false);
        atacando = false;
        rango.GetComponent<CapsuleCollider>().enabled = true;
        lanzallamas = false;
        jump_distance = 0;
        direction_Skill = false;
    }

    public void Direction_Attack_Start()
    {
        direction_Skill = true;
    }

    public void Direction_Attack_End()
    {
        direction_Skill = false;
    }

    public void ColliderWeaponTrue()
    {
        hit[hit_select].GetComponent<SphereCollider>().enabled = true;
    }

    public void ColliderWeaponFalse()
    {
               hit[hit_select].GetComponent<SphereCollider>().enabled = false;
    }

    //Lanzallamas
    public GameObject GetBala()
    {
        for(int i = 0; i < pool.Count; i++)
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

    public void Lanzallamas_Skill()
    {
        cronometro2 += 1* Time.deltaTime;
        if (cronometro2 > 0.1f)
        {
            GameObject bala = GetBala();
            bala.transform.position = cabeza.transform.position;
            bala.transform.rotation = cabeza.transform.rotation;
            cronometro2 = 0;
        }
    }

    public void Start_Fire()
    {
        lanzallamas = true;
    }

    public void Stop_Fire()
    {
        lanzallamas = false;
    }

    public GameObject Get_Fire_Ball()   //
    {
        for(int i = 0; i< pool2.Count; i++)
        {
            if (!pool2[i].activeInHierarchy)
            {
                pool2[i].SetActive(true);
                return pool2[i];
            }
        }
        GameObject obj = Instantiate(fire_ball, point.transform.position, point.transform.rotation) as GameObject;
        pool2.Add(obj);
        return obj;
    }

    public void Fire_Ball_Skill()
    {
        GameObject bola = Get_Fire_Ball();
        bola.transform.position = point.transform.position;
        bola.transform.rotation = point.transform.rotation;
    }

    public void Vivo()
    {
        if(HP_Min < 500)
        {
            fase = 2;
            time_rutinas = 1;
        }

        ComportamientoBoss();

        if(lanzallamas)
        {
            Lanzallamas_Skill();
        }
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
                musica.enabled = false;
            }
        }
    }
}

