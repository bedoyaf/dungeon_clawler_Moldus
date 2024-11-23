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

    /// <summary>
    /// just subtracts from currentHealth with rescpets to damageModifier, and if needed makes the object die dead
    /// </summary>
    public void TakeDamage(int damage)
    {
        damage =(int)(damage * damageModifier);
        currentHealth -= damage;
        //Debug.Log($"{gameObject.name} took {damage} damage.");
        FlashRed();
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
        onHeal.Invoke(amount);
    }

    /// <summary>
    /// invokes the Die event
    /// </summary>
    public void Die()
    {
        //Debug.Log($"{gameObject.name} died!");
        onDeathEvent?.Invoke(gameObject);
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
        Color changeColor = new Color(1f, 0f, 0f, 0.5f);
        _spriteRenderer.DOColor(changeColor, 0.1f).OnComplete(() =>
        {
            _spriteRenderer.DOColor(originalColor, 0.1f);
        });

    }
    void OnDestroy()
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.DOKill();
        }
    }
}
