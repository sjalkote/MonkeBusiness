using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public float health = 100;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (health <= 0) Destroy(gameObject);
    }

    public void Damage(float damageAmount)
    {
        Debug.Log("hit");
        health -= damageAmount;
        if (health <= 0) Destroy(gameObject);
    }
}