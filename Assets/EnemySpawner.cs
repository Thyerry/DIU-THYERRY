using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject spawnEnemy;
    int numOfSpawns = 1;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 2, 5);
        InvokeRepeating("DificultUp", 30, 30);
    }

    void DificultUp()
    {
        numOfSpawns += 1;
    }

    void Spawn()
    {

        for (int i = 0; i < numOfSpawns; i++)
        {
            spawnEnemy.transform.position = new Vector3(transform.position.x, transform.position.y + i * 0.1f, transform.position.z);
            Instantiate(spawnEnemy);
        }
    }
}
