using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] private int maxShieldHealth = 50;
    [SerializeField] private float rechargeDelay = 5f;  
    [SerializeField] private int rechargeRate = 50;

    private int shieldHealth;
    private Coroutine rechargeCoroutine;

    private SpriteRenderer shieldRenderer;

    private bool flickering = false;

    void Start()
    {
        shieldHealth = maxShieldHealth;
        shieldRenderer = GetComponent<SpriteRenderer>();

    }

    void Update()
    {

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
            rechargeCoroutine = StartCoroutine(RechargeShield());
            if(!flickering) FlickerOnHit();
            if(shieldHealth ==0) TurnOFFShield();

        }
        return amount;
    }

    private void TurnOnShield()
    {
        shieldRenderer.enabled = true;
        shieldRenderer.DOFade(1f, 0.5f); // Smoothly fade in
    }

    private void TurnOFFShield()
    {
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
