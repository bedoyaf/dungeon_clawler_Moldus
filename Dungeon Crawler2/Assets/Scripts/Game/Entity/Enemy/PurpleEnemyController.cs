using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleEnemyController : BasicEnemy
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private BulletData bulletData;  //so its projectile is unique to the others
    void Start()
    {
        ConfigurateBasicFields();
        StartCoroutine(EnemyBehavior());
        spriteFlipCustomizer = false;
        colorOfEnemy = ColorEnemy.Purple;
    }
    /// <summary>
    /// could be the same as Shoot, but decided to make it more exact
    /// </summary>
    public override void Attack()
    {
        Shoot();
    }
    /// <summary>
    /// spawns the bulletPrefab and configurates it
    /// </summary>
    private void Shoot()
    {
        Vector2 shootDirection = (target.position - transform.position).normalized;
        float bulletSpeed = _rigidBody.linearVelocity.magnitude;
        Vector3 currentPosition = transform.position;
        currentPosition.z = 0;
        GameObject bullet = Instantiate(bulletPrefab, currentPosition, Quaternion.identity);
        bullet.GetComponent<BulletController>().setBulletData(bulletData);
        bullet.GetComponent<BulletController>().Initialize(gameObject,shootDirection, currentPosition, bulletSpeed);
    }
}
