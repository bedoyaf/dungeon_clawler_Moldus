using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class GreenBossControler : SpawnerController
{

    private void OnDestroy()
    {
        Debug.Log("Znicen"+spawned.Count);
        foreach(var e in spawned)
        {
            if(e!=null)
            {
                e.GetComponent<HealthController>().TakeDamage(1000);
            }
        }
        Destroy(gameObject);
    }
}