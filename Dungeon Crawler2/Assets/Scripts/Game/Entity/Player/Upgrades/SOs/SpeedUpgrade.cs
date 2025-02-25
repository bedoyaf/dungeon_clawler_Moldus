using UnityEngine;

/// <summary>
/// Increases the players speed value
/// </summary>
[CreateAssetMenu(fileName = "SpeedUpgrade", menuName = "Upgrades/Speed Upgrade")]
public class SpeedUpgrade : UpgradeSO
{
    public float newSpeed = 10;
    private float originalSpeed;
    public override void Activate(GameObject player)
    {
        originalSpeed = player.GetComponent<PlayerMovementController>().speed;
        player.GetComponent<PlayerMovementController>().changeSpeed(newSpeed);
    }
    public override void Revert(GameObject player)
    {
        player.GetComponent<PlayerMovementController>().changeSpeed(originalSpeed);
    }
}