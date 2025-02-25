using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Just extends the spawner, since it is just a normal enemy being a spawner
/// </summary>
public class GreenBossControler : SpawnerController
{
    /// <summary>
    /// Kills of its spawned minions
    /// </summary>
    private void OnDestroy()
    {
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