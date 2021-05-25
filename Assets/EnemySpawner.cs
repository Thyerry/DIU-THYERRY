using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject spawnEnemy01, spawnEnemy02, spawnEnemy03;
    [SerializeField]
    Movement playerScript;
    [SerializeField]
    GameObject turkey;

    int numOfSpawns;
    int dificultLvl;
    int[] spots;

    bool LifeUp1, LifeUp2, LifeUp3, LifeUp4, LifeUp5;

    private List<Vector3> spawnSpots = new List<Vector3>()
    {
        new Vector3(-9.3f, 0, 0),
        new Vector3(-9.3f, -0.2f, 0),
        new Vector3(-9.3f, -0.4f, 0),
        new Vector3(-9.3f, -0.6f, 0),
        new Vector3(0.6f, 0, 0),
        new Vector3(0.6f, -0.2f, 0),
        new Vector3(0.6f, -0.4f, 0),
        new Vector3(0.6f, -0.6f, 0),
    };


    // Start is called before the first frame update
    void Start()
    {
        dificultLvl = 0;
        numOfSpawns = 3;
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        InvokeRepeating("Spawn", 0, 5);
        InvokeRepeating("DificultUp", 15, 30);
        Instantiate(turkey);
    }

    void DificultUp()
    {
        switch (dificultLvl)
        {
            case 0:
                CancelInvoke();
                numOfSpawns = 1;
                InvokeRepeating("Spawn", 0, 1);
                InvokeRepeating("DificultUp", 15, 10);
                break;
            case 1:
                CancelInvoke();
                numOfSpawns = 4;
                InvokeRepeating("Spawn", 5, 8);
                InvokeRepeating("DificultUp", 40, 40);
                Instantiate(turkey);
                break;
            case 2:
                CancelInvoke();
                numOfSpawns = 2;
                InvokeRepeating("Spawn", 0, 1);
                InvokeRepeating("DificultUp", 15, 5);
                break;
            case 3:
                CancelInvoke();
                numOfSpawns = 4;
                InvokeRepeating("Spawn", 5, 5);
                InvokeRepeating("DificultUp", 60, 40);
                Instantiate(turkey);
                break;
            case 4:
                CancelInvoke();
                numOfSpawns = 3;
                InvokeRepeating("Spawn", 0, 1);
                InvokeRepeating("DificultUp", 30, 40);
                Instantiate(turkey);
                break;
            default:
                CancelInvoke();
                if (dificultLvl % 2 != 0)
                {
                    numOfSpawns = 4;
                    InvokeRepeating("Spawn", 10, 1);
                    InvokeRepeating("DificultUp", 30, 40);
                }
                else
                {
                    numOfSpawns = 8;
                    InvokeRepeating("Spawn", 10, 1);
                    InvokeRepeating("DificultUp", 10, 40);
                }
                break;

        }

        dificultLvl += 1;

    }

    void Update()
    {
        try
        {
            if (playerScript.score >= 25 && playerScript.score < 75 && !LifeUp1)
            {
                Instantiate(turkey);
                LifeUp1 = true;
            }
            else if (playerScript.score >= 75 && playerScript.score < 125 && !LifeUp2)
            {
                Instantiate(turkey);
                LifeUp2 = true;
            }
            else if (playerScript.score >= 150 && playerScript.score < 200 && !LifeUp3)
            {
                Instantiate(turkey);
                LifeUp3 = true;
            }
            else if (playerScript.score >= 200 && !LifeUp4)
            {
                Instantiate(turkey);
                LifeUp4 = true;
            }
        }
        catch (Exception)
        {
            return;
        }
        
    }

    private void Spawn()
    {
        RandomSpots();
        for (int i = 0; i < numOfSpawns; i++)
        {
            int enemyId = UnityEngine.Random.Range(0, 3);
            switch (enemyId)
            {
                case 0:
                    InstantiateEnemy(spawnEnemy01, spots[i]);
                    break;
                case 1:
                    InstantiateEnemy(spawnEnemy02, spots[i]);
                    break;
                case 2:
                    InstantiateEnemy(spawnEnemy03, spots[i]);
                    break;
                default:
                    break;
            }
        }
    }

    void InstantiateEnemy(GameObject enemy, int spot)
    {
        enemy.transform.position = spawnSpots[spot];
        Instantiate(enemy);
    }

    void RandomSpots()
    {
        spots = Enumerable.Range(0, spawnSpots.Count - 1).ToArray();
        var rnd = new System.Random();

        for (int i = 0; i < spots.Length; ++i)
        {
            int randomIndex = rnd.Next(spots.Length);
            int temp = spots[randomIndex];
            spots[randomIndex] = spots[i];
            spots[i] = temp;
        }
    }
}
