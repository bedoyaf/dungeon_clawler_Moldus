using UnityEngine;

/// <summary>
/// Increases the shooting frequency, by changing values
/// </summary>
[CreateAssetMenu(fileName = "FireRateUpgrade", menuName = "Upgrades/FireRate Upgrade")]
public class FireRateUpgrade : UpgradeSO
{
    public float delayBetweenShots = 0.5f;
    private float original_delayBetweenShots;

    public override void Activate(GameObject player)
    {
        original_delayBetweenShots = player.GetComponent<PlayerShootingController>().delayBetweenShots;
        player.GetComponent<PlayerShootingController>().changeFireRate(delayBetweenShots);
    }

    public override void Revert(GameObject player)
    {
        player.GetComponent<PlayerShootingController>().changeFireRate(original_delayBetweenShots);
    }
}