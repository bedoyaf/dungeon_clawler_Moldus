using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// object under player that is assignet to healthController, when takedamage is called, this object subtracts from it if possible
/// the shield itself recharges when depleted and attacked 
/// </summary>
public class ShieldController : MonoBehaviour
{
    [SerializeField] private int maxShieldHealth = 50;
    [SerializeField] private float rechargeDelay = 5f;  

    private int shieldHealth;
    private Coroutine rechargeCoroutine;

    private SpriteRenderer shieldRenderer;

    private bool flickering = false;

    [Header("Audio Settings")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip shieldHitSound;
    [SerializeField] private AudioClip shieldPowerUpSound;
    [SerializeField] private AudioClip shieldBreakSound;

    void Start()
    {
        shieldHealth = maxShieldHealth;
        shieldRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

    }

    public void Initiate()
    {
        shieldHealth = maxShieldHealth;
        shieldRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(shieldPowerUpSound);
    }

    /// <summary>
    /// Coroutine that waits a while and then recharges, it can be interupted if AbsorbDamage is called
    /// </summary>
    private IEnumerator RechargeShield()
    {
        yield return new WaitForSeconds(rechargeDelay);
        shieldHealth = maxShieldHealth;
        TurnOnShield();
    }

    /// <summary>
    /// called withing health controller, subtracts from damage and takes care of all the visuals and recharging of the shield
    /// practicly the main function of shield
    /// </summary>
    /// <param name="amount">amound of absorbed damage</param>
    /// <returns>damage left</returns>
    public int AbsorbDamage(int amount)
    {
        if(shieldHealth>0)
        {
            int absorbed = Math.Min(shieldHealth, amount);
            shieldHealth -= absorbed;
            amount -= absorbed;
            if (rechargeCoroutine != null)
            {
                StopCoroutine(rechargeCoroutine);
            }
            audioSource.PlayOneShot(shieldHitSound);
            rechargeCoroutine = StartCoroutine(RechargeShield());
            if(!flickering) FlickerOnHit();
            if(shieldHealth ==0) TurnOFFShield();

        }
        return amount;
    }

    /// <summary>
    /// turns on the shield with sounds and visuals
    /// </summary>
    private void TurnOnShield()
    {
        audioSource.PlayOneShot(shieldPowerUpSound);
        shieldRenderer.enabled = true;
        shieldRenderer.DOFade(1f, 0.5f); 
    }

    /// <summary>
    /// turns off shield with sounds and visuals
    /// </summary>
    private void TurnOFFShield()
    {
        audioSource.PlayOneShot(shieldBreakSound);
        shieldRenderer.DOKill();
        shieldRenderer.DOFade(0f, 0.5f).SetLoops(6, LoopType.Yoyo).OnComplete(() =>
        {
            shieldRenderer.enabled =false;
        });
    }

    /// <summary>
    /// visuals when shield takes damage
    /// </summary>
    private void FlickerOnHit()
    {
        flickering = true;
        Color originalColor = shieldRenderer.color;
        Color changeColor = new Color(1f, 1f, 1f, 0.5f) * 1.5f;
        shieldRenderer.DOColor(changeColor, 0.1f).OnComplete(() =>
        {
            shieldRenderer.DOColor(originalColor, 0.1f);
            flickering = false;
        });
    }
}
