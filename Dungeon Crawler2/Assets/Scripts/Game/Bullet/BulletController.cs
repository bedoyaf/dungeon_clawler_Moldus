using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Vector3 originalPosition;

    public GameObject owner;
    [SerializeField] float speed = 10f;

    [SerializeField] int damage = 10;
    [SerializeField] private float despawnDistance = 20f;
    [SerializeField] private BulletData bulletData;
    [SerializeField] private float playerBiasModifier = 1f; //to adjust if I want to make the player deal More damage

    void Start()
    {
        damage = (int)(damage * playerBiasModifier);
    }

    void Update()
    {
        float distanceTraveled = Vector2.Distance(originalPosition, transform.position);

        if (distanceTraveled > despawnDistance)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Initializes the bullet
    /// </summary>
    /// <param name="shooter">Shooters object to check if it is a player or enemy</param>
    /// <param name="direction">The point that the bullet should fly towards</param>
    /// <param name="_originalPosition">The position of the player when he shot it, for calculating despawnDistance</param>
    /// <param name="additionalBulletSpeed">Just makes the bullets faster corresponing to the speed of the shooter</param>
    public void Initialize(GameObject shooter, Vector2 direction, Vector3 _originalPosition, float additionalBulletSpeed)
    {
        SetTag(shooter);
        SetDirection(direction, _originalPosition, additionalBulletSpeed);
    }
    /// <summary>
    /// Sets the tag for checking collisions later
    /// </summary>
    private void SetTag(GameObject shooter)
    {
        if (shooter.CompareTag("Player"))
        {
            tag = "FriendlyBullet";
        }
        else if (shooter.CompareTag("Enemy"))
        {
            tag = "EnemyBullet";
        }
        else
        {
            Debug.LogWarning("Shooter does not have a valid tag. Cannot set bullet tag.");
        }
    }
    /// <summary>
    /// Sets the bullets the bullet should fly towards
    /// </summary>
    private void SetDirection(Vector2 direction, Vector3 _originalPosition, float additionalbulletSpeed)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.Rotate(0, 0, -90); ;
        direction = direction.normalized;
        speed += additionalbulletSpeed;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * speed;
        originalPosition = _originalPosition;
    }
    /// <summary>
    /// Just configurates the bullets by data, helps to create a bunch of bullet types
    /// </summary>
    public void setBulletData(BulletData newbulletData)
    {
        bulletData = newbulletData;
        var _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = newbulletData.sprite;
        speed = newbulletData.speed;
        damage = (int)(newbulletData.damage * playerBiasModifier);
        despawnDistance = newbulletData.despawnDistance;
        transform.Rotate(0, 0, bulletData.rotationAngle);
        AudioSource _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = bulletData.audioClip;
    }

    /// <summary>
    /// Just checks what has been hit by the bullet and acts acordingly
    /// </summary>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("FriendlyBullet") && collision.CompareTag("Enemy"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (gameObject.CompareTag("EnemyBullet") && collision.CompareTag("Player"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Spawner") && gameObject.CompareTag("FriendlyBullet"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Structure"))
        {
            Destroy(gameObject);
        }
    }

}
