using TMPro;
using UnityEngine;

/// <summary>
/// Manages the bomb thrown by the red boss
/// </summary>
public class BossBombController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private int speed = 10;

    private Vector2 targetPosition;
    private bool hasExploded = false;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private int damage = 30;

    /// <summary>
    /// sets up the direction the bomb should fly towards
    /// </summary>
    /// <param name="target">for direction</param>
    public void flyTowards(Vector2 target)
    {
        targetPosition = target;
        rb = GetComponent<Rigidbody2D>();

        Vector2 direction = target - new Vector2(transform.position.x, transform.position.y);

        Vector2 normalizedDirection = direction.normalized;

        rb.linearVelocity = normalizedDirection * speed;
    }

    void Update()
    {
        if (!hasExploded && Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {

            Explode();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasExploded)
        {
            Explode();
        }
    }

    /// <summary>
    /// Manages the explosion itself
    /// </summary>
    void Explode()
    {
        hasExploded = true;
        GetComponent<SpriteRenderer>().enabled = false;
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
                damageable.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}
