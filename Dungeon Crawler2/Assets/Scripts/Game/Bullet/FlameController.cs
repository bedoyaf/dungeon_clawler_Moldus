using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the damage taken by the flame from the flamethrower the player spawns
/// </summary>
public class FlameController : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float damageInterval = 0.1f;
    [SerializeField] public float damageModifier = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Spawner") || collision.gameObject.CompareTag("Pillar") || collision.gameObject.CompareTag("Boss"))
        {
            //IDamageable enemy = collision.GetComponent<>();  

            HealthController enemy = collision.GetComponent<HealthController>();
            if (enemy != null && !enemy.isOnFire && gameObject.activeInHierarchy)
            {
                enemy.isOnFire = true;
                StartCoroutine(DamageOverTime(enemy));
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        HealthController health = other.GetComponent<HealthController>();
        if(health != null)
        {
            health.isOnFire = false;
        }
    }
    private IEnumerator DamageOverTime(HealthController enemy)
    {
        //enemy.IsTakingDamage = true; // Prevent multiple coroutines
        while (enemy != null && enemy.isOnFire) // Custom flag for tracking flame state
        {
            enemy.TakeDamage((int)(damage*damageModifier));
            yield return new WaitForSeconds(damageInterval);
        }
       // enemy.IsTakingDamage = false;
    }
}
