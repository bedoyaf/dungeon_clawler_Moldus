using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] private float explosionTime = 2f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private int damage=30;
    public GameObject explosionEffect; //just smoke visual object thats left after the explosion
    [SerializeField] public float damageModifier = 1f;

    void Start()
    {
        // starts the countdown to explosion
        Invoke("Explode", explosionTime);
    }

    /// <summary>
    /// Spawns the smoke visuals and checks all the colliders in the viscinity then deals damage to them and destroys itself
    /// </summary>
    void Explode()
    {

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D nearbyObject in colliders)
        {
            IDamageable damageable = nearbyObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage((int)(damage*damageModifier));
            }
        }
        Destroy(gameObject);
    }

    void Update()
    {
        Invoke("Explode", explosionTime);
    }
}
