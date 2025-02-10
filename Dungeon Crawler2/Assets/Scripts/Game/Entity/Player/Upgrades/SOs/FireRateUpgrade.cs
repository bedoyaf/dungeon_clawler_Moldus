using UnityEngine;

[CreateAssetMenu(fileName = "FireRateUpgrade", menuName = "Upgrades/FireRate Upgrade")]
public class FireRateUpgrade : UpgradeSO
{
    public float delayBetweenShots = 0.5f;

    public override void Activate(GameObject player)
    {
        player.GetComponent<PlayerShootingController>().changeFireRate(delayBetweenShots);
        Debug.Log($"{abilityName} applied! Firerate increased to {delayBetweenShots}");
    }
}