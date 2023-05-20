using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public float health = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(this);
        }
    }
    
    public void Damage(float damageAmount)
    {
        Debug.Log("hit");
        health -= damageAmount;
        if (health <= 0)
        {
            Destroy(this);
        }
    }
}
