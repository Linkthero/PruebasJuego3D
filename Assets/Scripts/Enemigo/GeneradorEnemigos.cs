using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorEnemigos : MonoBehaviour
{
    //array de enemigos
    public GameObject[] enemies;

    private List<GameObject> enemigosActuales;

    //tiempo q pasa para spawnear
    public float timeSpawn = 1;

    [SerializeField] private int MaxEnemigos = 6;
    //se crea un enemigo cada x seg
    private float repeatSpawnRate;

    [SerializeField] public Transform xRangeLeft;
    [SerializeField] public Transform xRangeRight;

    [SerializeField] public Transform yRangeUp;
    [SerializeField] public Transform yRangeDown;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemigosActuales = new List<GameObject>();
       
        //StartCoroutine(Example());
        //InvokeRepeating("SpawnEnemies", timeSpawn, repeatSpawnRate);
    }

    public void SpawnEnemies()
    {
        if (enemigosActuales.Count < MaxEnemigos)
        {
            //posicion donde se crea, aleatoria
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(xRangeLeft.position.x, xRangeRight.position.x), UnityEngine.Random.Range(yRangeDown.position.y, yRangeUp.position.y), UnityEngine.Random.Range(yRangeDown.position.z, yRangeUp.position.z));

            int numEnemigo = UnityEngine.Random.Range(0, enemies.Length);
            Quaternion rotacion = gameObject.transform.rotation;
            rotacion.y = rotacion.y + 180;
            GameObject enemie = Instantiate(enemies[numEnemigo], spawnPosition, rotacion);
            enemigosActuales.Add(enemie);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            repeatSpawnRate = (float)UnityEngine.Random.Range(5, 10);
            InvokeRepeating("SpawnEnemies", timeSpawn, repeatSpawnRate);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CancelInvoke("SpawnEnemies");
            enemigosActuales.ForEach(enemy => Destroy(enemy));
            enemigosActuales.Clear();
        }
    }
}
