using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    //Codigo enemigo base //
    [Header("Codigo enemigo base")]
    public int rutina;
    public float cronometro;
    public float time_rutinas;
    public Animator animator;
    public Quaternion angulo;
    public float grado;
    public GameObject target;
    public float speed;
    public int velocidadPersecucion = 3;
    public bool atacando;
    public RangoBoss rango;
    public GameObject[] hit;

    [Header("Invocar Minioms")]
    public GameObject prefabMiniom;
    private List<GameObject> listaMinioms;


    public bool curando = false;
    public bool invocando = false;

    
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
    //public AudioSource musica; //musica batalla
    public bool muerto;     //boss muerto


    private void Start()
    {
        animator = GetComponent<Animator>();
        target = GameObject.Find("Player");
        listaMinioms = new List<GameObject>();
    }

    public void ComportamientoBoss()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 15)       //si el jugador esta a menos de 15 metros, el boss lo persigue y ataca
        {
            var lookPos = target.transform.position - transform.position;   //calcula la direccion hacia el jugador
            lookPos.y = 0; //para que no mire hacia arriba o abajo, solo en el plano horizontal
            var rotation = Quaternion.LookRotation(lookPos); //gira hacia el jugador
            point.transform.LookAt(target.transform.position); //el punto de donde salen las bolas de fuego mira al jugador para que las bolas vayan hacia el jugador
            //musica.enabled = true;

            cronometro += 1 * Time.deltaTime;   //aumenta el cronometro para cambiar de rutina cada cierto tiempo
            if (cronometro > time_rutinas)  //si el cronometro es mayor que el tiempo de rutina, cambia de rutina aleatoriamente entre 0, 1 y 2  y resetea el cronometro
            {
                rutina = Random.Range(0, 4);
                //Debug.Log("Rutina: " + rutina);
                cronometro = 0;
            }

            if (Vector3.Distance(transform.position, target.transform.position) > 1 && !atacando)   //
            {
                switch (rutina)
                {
                    case 0:
                    //    //WALK
                    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2); //gira hacia el jugador lentamente para que no sea tan brusco el movimiento, 2 es la velocidad de giro
                    //    animator.SetBool("walk", true); //activa la animacion de caminar
                    //    animator.SetBool("run", false);//desactiva la animacion de correr

                    //    if (transform.rotation == rotation)
                    //    {
                    //        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);  //mueve hacia el jugador a velocidad normal
                    //    }

                    //    animator.SetBool("attack", false);  //desactiva la animacion de ataque

                    //    cronometro += 1 * Time.deltaTime;   //aumenta el cronometro para cambiar de rutina cada cierto tiempo
                    //    if (cronometro > time_rutinas)  //si el cronometro es mayor que el tiempo de rutina, cambia de rutina aleatoriamente entre 0, 1 y 2 (walk, run y ataque) y resetea el cronometro
                    //    {
                    //        rutina = Random.Range(0, 2);
                    //        cronometro = 0;
                    //    }
                    //    break;

                    //case 1:
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
                        if (Vector3.Distance(transform.position, target.transform.position) < 1)
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
                        ////Lanzallamas
                        //animator.SetBool("walk", false);
                        //animator.SetBool("run", false);
                        //animator.SetBool("attack", true);
                        //animator.SetFloat("skill", 0);
                        //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);
                        //rango.GetComponent<CapsuleCollider>().enabled = false;

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
                        if((listaMinioms.Count < 4) && !invocando)
                        {
                            invocando = true;
                            animator.SetBool("walk", false);
                            animator.SetBool("run", false);
                            animator.SetBool("attack", true);
                            animator.SetFloat("skills", 0.2f);
                        } 
                            break;

                    //case 3:
                    //    //Fireball

                    //    //if (fase == 2)
                    //    //{
                    //    //    animator.SetBool("walk", false);
                    //    //    animator.SetBool("run", false);
                    //    //    animator.SetBool("attack", true);
                    //    //    animator.SetFloat("skill", 0);
                    //    //    rango.GetComponent<CapsuleCollider>().enabled = false;
                    //    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 0.5f);
                    //    //}
                    //    //else
                    //    //{
                    //    //    rutina = 0;
                    //    //    cronometro = 0;
                    //    //}

                    //    //Ataque melee
                    //    animator.SetBool("walk", false);
                    //    animator.SetBool("run", false);
                    //    animator.SetBool("attack", true);
                    //    animator.SetFloat("skill", 0.2f);
                    //    break;

                }
            }
        }
    }


    public void Final_AniBoss()
    {
        rutina = 0;
        animator.SetBool("attack", false);
        atacando = false;
        curando = false;
        invocando = false;
        rango.GetComponent<CapsuleCollider>().enabled = true;
        lanzallamas = false;
        jump_distance = 0;
        direction_Skill = false;
    }

    public void Invocar()
    {
        listaMinioms.Add(Instantiate(prefabMiniom, transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)), Quaternion.identity)); //spawnea un miniom en una posicion aleatoria alrededor del boss
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

