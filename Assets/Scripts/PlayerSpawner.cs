using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    IEnumerator Spawn()
    {
        player.transform.position = transform.position;
        yield return new WaitForSeconds(0.5f);
        Instantiate(player);
    }
}
