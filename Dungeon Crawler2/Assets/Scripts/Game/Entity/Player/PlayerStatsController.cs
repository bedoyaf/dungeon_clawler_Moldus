using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    //animation
    [SerializeField] private List<RuntimeAnimatorController> animationControllers; //default,green,red, violet
    private Animator animator;
    public ColorEnemy currentInfection = ColorEnemy.None;
    [SerializeField] private EnemyKillCountController pointsCounter;
    [SerializeField] private int pointsRequiredToTransform = 5;
    [SerializeField] private TextMeshProUGUI transformationTimeText;
    [SerializeField] private float transformationTimeSeconds = 10f;
    private HealthController healthController;
    //helpers
    private PlayerShootingController shootingController;

    private SpriteRenderer _spriteRenderer;


    void Start()
    {
        var playerHealthController = GetComponent<HealthController>();
        playerHealthController.onDeathEvent.AddListener(PlayerDeath);
        shootingController = GetComponent<PlayerShootingController>();
        animator=GetComponent<Animator>();
        healthController = GetComponent<HealthController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        UpdatePlayerInfection();
    }

    void Update()
    {
        CheckIfCanSwitchForm();
    }

    /// <summary>
    /// Checks if the player has enaugh points then switches the infection and updates the shooter, also heals player
    /// </summary>
    private void CheckIfCanSwitchForm()
    {
        ColorEnemy originalInfection = currentInfection;
        bool changed = false;
        if(currentInfection == ColorEnemy.None)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (pointsCounter.RedPoints >= pointsRequiredToTransform)
                {
                    pointsCounter.RedPoints = pointsCounter.RedPoints - pointsRequiredToTransform;
                    currentInfection = ColorEnemy.Red;
                    changed = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (pointsCounter.GreenPoints >= pointsRequiredToTransform)
                {
                    pointsCounter.GreenPoints = pointsCounter.GreenPoints - pointsRequiredToTransform;
                    currentInfection = ColorEnemy.Green;
                    changed = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (pointsCounter.PurplePoints >= pointsRequiredToTransform)
                {
                    pointsCounter.PurplePoints = pointsCounter.PurplePoints - pointsRequiredToTransform;
                    currentInfection = ColorEnemy.Purple;
                    changed = true;
                }
            }
        }
        if(changed)
        {
            StartCoroutine(TransformationTime(transformationTimeSeconds));
            healthController.Heal(100);
            UpdatePlayerInfection();
            pointsCounter.UpdateTextPoints();
        }
    }

    /// <summary>
    /// This coroutine ensures that after a certain amount of time the player referts back 
    /// </summary>
    private IEnumerator TransformationTime(float duration)
    {
        float timeRemaining = duration;

        while (timeRemaining > 0)
        {
            transformationTimeText.text =  timeRemaining.ToString("F1") ;
            yield return new WaitForSeconds(0.1f);
            timeRemaining -= 0.1f;
        }
        transformationTimeText.text = "";
        currentInfection = ColorEnemy.None;
        UpdatePlayerInfection();
    }

    /// <summary>
    /// just updates the shootingCOntroller and changes animation to match the infection
    /// </summary>
    private void UpdatePlayerInfection()
    {
        shootingController.SetInfection(currentInfection);
        int index = 0;
        switch (currentInfection)
        {
            case ColorEnemy.Purple:
                index = 3;
                break;
            case ColorEnemy.Green:
                index = 1;
                break;
            case ColorEnemy.Red:
                index = 2;
                break;
            case ColorEnemy.None:
                index = 0;
                break;
        }
        animator.runtimeAnimatorController = animationControllers[index];
    }

    public void ResetInfection()
    {
        currentInfection = ColorEnemy.None;
        UpdatePlayerInfection();
    }

    /// <summary>
    /// On death deactivates the object, its easier than destroying it
    /// </summary>
    public virtual void PlayerDeath(GameObject dead)
    {
        gameObject.SetActive(false);
    }
}
