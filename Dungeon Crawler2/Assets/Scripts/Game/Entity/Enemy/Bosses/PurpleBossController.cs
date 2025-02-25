using DG.Tweening;
using System.Collections;
using UnityEngine;

/// <summary>
/// Extends the normal basic enemy, but makes it stand in place and shoot around
/// </summary>
public class PurpleBossController : BasicEnemy
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private BulletData bulletData;

    [SerializeField] float shootInterval = 1.0f;
    [SerializeField] float rotationAngle = 20f;
    [SerializeField] int numberOfDirections = 4;
    private float currentAngle = 0f;

    void Start()
    {
        ConfigurateBasicFields();
        GetComponent<HealthController>().onDeathEvent.AddListener(On_Death);
        StartCoroutine(PurpleEnemyBehavior());
        aiPath.canMove = false;
    }

    /// <summary>
    /// Changes the normal behaviour to be just shooting in circles
    /// </summary>
    protected IEnumerator PurpleEnemyBehavior()
    {
        while (true)
        {
            Attack();
            currentAngle += rotationAngle; 
            yield return new WaitForSeconds(shootInterval);
        }
    }

    /// <summary>
    /// Shoots in x directions
    /// </summary>
    public override void Attack()
    {
        float angleStep = 360f / numberOfDirections; 
        float startAngle = currentAngle;
        for (int i = 0; i < numberOfDirections; i++)
        {
            // Calculate the angle for the current direction
            float currentShootAngle = startAngle + i * angleStep;
            Vector2 shootDirection = new Vector2(Mathf.Cos(currentShootAngle * Mathf.Deg2Rad), Mathf.Sin(currentShootAngle * Mathf.Deg2Rad));
            Shoot(shootDirection);
        }
    }

    /// <summary>
    /// Just the instantiation of one bullet 
    /// </summary>
    /// <param name="shootDirection">the direction the bullet should go</param>
    private void Shoot(Vector2 shootDirection)
    {

        float bulletSpeed = _rigidBody.linearVelocity.magnitude;
        Vector3 currentPosition = transform.position;
        currentPosition.z = 0;
        GameObject bullet = Instantiate(bulletPrefab, currentPosition, Quaternion.identity);
        bullet.GetComponent<BulletController>().setBulletData(bulletData);
        bullet.GetComponent<BulletController>().Initialize(gameObject, shootDirection, currentPosition, bulletSpeed);
    }
   /* public new void On_Death()
    {
        Sequence deathSequence = DOTween.Sequence();
        deathSequence.Append(transform.DOScale(0f, 0.5f).SetEase(Ease.InBack));
        deathSequence.Join(transform.DORotate(new Vector3(0f, 0f, 360f), 0.5f, RotateMode.FastBeyond360));
        deathSequence.OnComplete(() => Destroy(gameObject));
    }*/
}
