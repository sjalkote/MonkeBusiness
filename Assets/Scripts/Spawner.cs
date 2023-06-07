using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemies;

    private void Spawn()
    {
        var id = Random.Range(0, enemies.Length);
        Instantiate(enemies[id], transform.position, Quaternion.identity);
    }
}