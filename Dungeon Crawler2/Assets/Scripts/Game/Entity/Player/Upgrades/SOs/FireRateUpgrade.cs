using UnityEngine;

/// <summary>
/// Increases the shooting frequency, by changing values
/// </summary>
[CreateAssetMenu(fileName = "FireRateUpgrade", menuName = "Upgrades/FireRate Upgrade")]
public class FireRateUpgrade : UpgradeSO
{
    public float delayBetweenShots = 0.5f;
    private float original_delayBetweenShotsRed;
    private float original_delayBetweenShotsGreen;
    private float original_delayBetweenShotsPurple;

    public override void Activate(GameObject player)
    {
        original_delayBetweenShotsRed = player.GetComponent<PlayerShootingController>().delayBetweenShotsRed;
        original_delayBetweenShotsGreen = player.GetComponent<PlayerShootingController>().delayBetweenShotsGreen;
        original_delayBetweenShotsPurple = player.GetComponent<PlayerShootingController>().delayBetweenShotsPurple;
        player.GetComponent<PlayerShootingController>().changeFireRateMultiply(delayBetweenShots);
    }

    public override void Revert(GameObject player)
    {
        player.GetComponent<PlayerShootingController>().changeFireRate(original_delayBetweenShotsRed, original_delayBetweenShotsGreen,original_delayBetweenShotsPurple);
    }
}