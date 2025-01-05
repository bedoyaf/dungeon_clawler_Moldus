using DG.Tweening;
using UnityEngine;

public class RedBoss : BasicEnemy
{
    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public override void Attack()
    {
        
    }
    public void On_Death()
    {
        Sequence deathSequence = DOTween.Sequence();
        deathSequence.Append(transform.DOScale(0f, 0.5f).SetEase(Ease.InBack));
        deathSequence.Join(transform.DORotate(new Vector3(0f, 0f, 360f), 0.5f, RotateMode.FastBeyond360));
        deathSequence.OnComplete(() => Destroy(gameObject));
    }
}
