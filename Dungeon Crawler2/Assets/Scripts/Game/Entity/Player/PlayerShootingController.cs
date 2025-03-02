using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerShootingController : MonoBehaviour
{
    //attack
    private ColorEnemy currentInfection = ColorEnemy.None; //determins weapons
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] private GameObject bombPrefab;       
    [SerializeField] public BulletData longRangeBullet;
    [SerializeField] public BulletData shortRangeBullet;
    private BulletData currentBulletData;
    [SerializeField] public float delayBetweenShots { get; private set; } = 1f; 
    private float lastShotTime;
    //flame
    [SerializeField] private GameObject flamePrefab;
    [SerializeField] private Transform flamePoint;

    private bool isFiring = false;
    private GameObject currentFlameInstance;
    private float flameDistanceFromPlayer = 2f;
    [SerializeField] private float flamerCooldown = 1f;
    private float lastFlame = 0f;
    [SerializeField] float flameDuration = 2;
    //helpers
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;

    private float currentDamageModifier = 1f;
    private float currentDamageModifierRage = 1f;

    [SerializeField] private StatusEffectAbstract statusEffect;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Time.timeScale == 0f) {
            StopFlameThrower();
        return; 
        }

        FlipSpriteIfNecessary();

        if (Input.GetMouseButtonDown(0) && currentInfection != ColorEnemy.None) // 0 = left mouse button
        {
            Attack();
        }
        if (Input.GetButtonDown("Fire1") && currentInfection == ColorEnemy.None)
        {
            StartFlameThrower();
        }

        if (Input.GetButtonUp("Fire1") && currentInfection == ColorEnemy.None)
        {
            StopFlameThrower();
        }
            UpdateFlamePointDirection();
    }

    /// <summary>
    /// flips the sprite in relation to the mouse position
    /// </summary>
    private void FlipSpriteIfNecessary()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    /// <summary>
    /// Stops the attack of the flamethrower by destroying the object
    /// </summary>
    public void StopFlameThrower()
    {
        if (isFiring)
        {
            isFiring = false;
            Destroy(currentFlameInstance);  // Destroy the current flame instance when the player releases the button
        }
    }
    /// <summary>
    /// spawns a flame object that damages the enemy with respect to time
    /// </summary>
    private void StartFlameThrower()
    {
        if (Time.time >= lastFlame + flamerCooldown)
        {
            if (!isFiring)
            {
                lastFlame = Time.time;
                isFiring = true;
                currentFlameInstance = Instantiate(flamePrefab, flamePoint.position, flamePoint.rotation, flamePoint);
                StartCoroutine(StopFlameAfterDuration(flameDuration));
            }
        }
    }
    /// <summary>
    /// Just snuffs out the flame after a while
    /// </summary>
    private IEnumerator StopFlameAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        StopFlameThrower();
    }
    /// <summary>
    /// switches current infection and changes the type of attack if its one of the bullet type attacks
    /// </summary>
    public void SetInfection(ColorEnemy newInfection)
    {
        currentInfection = newInfection;
        switch (currentInfection)
        {
            case ColorEnemy.Purple:
                currentBulletData = longRangeBullet;
                break;
            case ColorEnemy.Green:
                currentBulletData = shortRangeBullet;
                break;
        }
    }

    /// <summary>
    /// spawns an attack corresponding to the current infection
    /// </summary>
    public void Attack()
    {
        if (Time.time - lastShotTime > delayBetweenShots)
        {
            if(currentInfection == ColorEnemy.Purple)
            {
                Shoot();
            }
            else if(currentInfection == ColorEnemy.Red)
            {
                PlantBomb();
            }
            else if(currentInfection == ColorEnemy.Green)
            {
                Shoot();
            }
            else
            {
                Debug.Log("no infection");
            }
            lastShotTime = Time.time;
        }
    }

    /// <summary>
    /// spawns and cinfigurates the bullet object in a way so it flys in the direction of the mouse
    /// </summary>
    void Shoot()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 shootDirection = (mousePosition - transform.position).normalized;

        float bulletSpeed = rigidBody.linearVelocity.magnitude;//*dotProduct;
                                                  // Debug.Log(bulletSpeed);
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector3 currentPosition = transform.position;

        bullet.GetComponent<BulletController>().Initialize(gameObject,shootDirection, currentPosition, bulletSpeed);
        bullet.GetComponent<BulletController>().setBulletData(currentBulletData);
        bullet.GetComponent<BulletController>().SetStatusEffect(statusEffect);

    }


    /// <summary>
    /// Creates the bomb
    /// </summary>
    void PlantBomb()
    {
        var bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        if(statusEffect!=null)
        {
            bomb.GetComponent<BombController>().AddStatusEffect(statusEffect);
        }
    }

    /// <summary>
    /// Just changes the flames position acording to the mouse position so it rotates around the player
    /// </summary>
    private void UpdateFlamePointDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector3 directionToMouse = (mousePosition - transform.position).normalized;

        flamePoint.position = transform.position + directionToMouse * flameDistanceFromPlayer;

        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        flamePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void changeFireRate(float amount)
    {
        delayBetweenShots = amount;
    }

    public void IncreaseDamageModifier(float value)
    {
        currentDamageModifier = value;
        longRangeBullet.damageModifier = currentDamageModifier * currentDamageModifierRage;
        shortRangeBullet.damageModifier = currentDamageModifier * currentDamageModifierRage;
        flamePrefab.GetComponent<FlameController>().damageModifier = currentDamageModifier * currentDamageModifierRage;
    }

    public void UpdateDamageModifiersRage(float value)
    {
        currentDamageModifierRage = value;
        longRangeBullet.damageModifier = currentDamageModifier*currentDamageModifierRage;
        shortRangeBullet.damageModifier = currentDamageModifier * currentDamageModifierRage;
        flamePrefab.GetComponent<FlameController>().damageModifier = currentDamageModifier * currentDamageModifierRage;
    }

    public void AssignStatusEffect(StatusEffectAbstract effect)
    {
        statusEffect = effect;
    }
}
