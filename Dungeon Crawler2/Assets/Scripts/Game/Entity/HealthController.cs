using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
public class HealthController : MonoBehaviour, IDamageable
{
    //configurable
    [SerializeField] public int maxHealth = 100;
    [SerializeField] private float damageModifier = 1f; //so I can adjust how much damage that player/enemy takes

    [SerializeField] public ShieldController shield;
    public bool isOnFire { get;  set; } = false;
    public int currentHealth { get; private set; }
    //events
    [SerializeField] public UnityEvent<int> onTakeDamage;
    [SerializeField] public UnityEvent<GameObject> onDeathEvent;
    [SerializeField] public UnityEvent<int> onHeal;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            GetComponent<HealthController>()?.onDeathEvent.Invoke(gameObject);
        }
    }


    /// <summary>
    /// just subtracts from currentHealth with rescpets to damageModifier, and if needed makes the object die dead
    /// </summary>
    public void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            damage = (int)(damage * damageModifier);

            if(shield !=null)
            {
                int newdamage = shield.AbsorbDamage(damage); //temporary, might return damage
                if (damage != newdamage)
                {
                    return;
                }
            }

            currentHealth -= damage;
            //Debug.Log($"{gameObject.name} took {damage} damage.");
            if (gameObject.CompareTag("Player") || gameObject.CompareTag("Enemy") || gameObject.CompareTag("Spawner") || gameObject.CompareTag("Boss"))
            {
                FlashRed();
            }
            if (onTakeDamage != null)
            {
                onTakeDamage.Invoke(damage);
            }
            if (currentHealth <= 0)
            {
                currentHealth = 0;

                Die();
            }
        }
    }

    /// <summary>
    /// Increments the currentHealth, not exceeding maxHealth
    /// </summary>
    public void Heal(int amount)
    {
        currentHealth += amount;
        if(currentHealth >maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if(currentHealth<=0)
        {
            currentHealth = 1;
        }
        onHeal.Invoke(amount);
    }
    public void Heal()
    {
        currentHealth = maxHealth;
        onHeal.Invoke(-1); //not sure if breaks anything
    }

    public void IncreaseMaxHealth(int amount)
    {
        int oldMaxhealth = maxHealth;
        maxHealth = maxHealth+ amount;
        onHeal.Invoke(maxHealth-oldMaxhealth);
    }

    /// <summary>
    /// invokes the Die event
    /// </summary>
    public void Die()
    {
        // Debug.Log($"{gameObject.name} died!");
        if (onDeathEvent != null)
        {
            onDeathEvent?.Invoke(gameObject);
        }
    }
    public void LoadHealth(SaveData saveData)
    {
        currentHealth = saveData.Health;
    }

    public void FlashRed()
    {
        if (DOTween.IsTweening(_spriteRenderer))
        {
            return;
        }

        Color originalColor = _spriteRenderer.color;
        Color changeColor = new Color(1f, 0f, 0f, 0.5f) * 1.5f;
        _spriteRenderer.DOColor(changeColor, 0.1f).OnComplete(() =>
        {
            _spriteRenderer.DOColor(originalColor, 0.1f);
        });

    }
    public void OnDestroy()
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.DOKill();
        }
    }
}
