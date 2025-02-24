using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] private int maxShieldHealth = 50;
    [SerializeField] private float rechargeDelay = 5f;  
  //  [SerializeField] private int rechargeRate = 50;

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

    void Update()
    {

    }

    public void Initiate()
    {
        shieldHealth = maxShieldHealth;
        shieldRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(shieldPowerUpSound);
    }

    private IEnumerator RechargeShield()
    {
        //recharging = false;
        yield return new WaitForSeconds(rechargeDelay);

        shieldHealth = maxShieldHealth;
        TurnOnShield();
        /*recharging = true;
        while (shieldHealth < maxShieldHealth)
        {
            shieldHealth = Math.Min(shieldHealth + rechargeRate, maxShieldHealth);
            yield return new WaitForSeconds(1f);
        }

        recharging = false;*/
    }

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

    private void TurnOnShield()
    {
        audioSource.PlayOneShot(shieldPowerUpSound);
        shieldRenderer.enabled = true;
        shieldRenderer.DOFade(1f, 0.5f); // Smoothly fade in
    }

    private void TurnOFFShield()
    {
        audioSource.PlayOneShot(shieldBreakSound);
        shieldRenderer.DOKill();
        shieldRenderer.DOFade(0f, 0.5f).SetLoops(6, LoopType.Yoyo).OnComplete(() =>
        {
            shieldRenderer.enabled =false;
        });
    }

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
