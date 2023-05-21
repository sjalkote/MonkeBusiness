using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    
    public GameObject[] enemies;

    void Spawn()
    {
        int id = Random.Range(0, enemies.Length);
        Instantiate(enemies[id], transform.position, Quaternion.identity);
    }
}
