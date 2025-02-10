using UnityEngine;

[CreateAssetMenu(fileName = "SpeedUpgrade", menuName = "Upgrades/Speed Upgrade")]
public class SpeedUpgrade : UpgradeSO
{
    public int newSpeed = 10;

    public override void Activate(GameObject player)
    {
        player.GetComponent<PlayerMovementController>().changeSpeed(newSpeed);
        Debug.Log($"{abilityName} applied! Speed increased to {newSpeed}");
    }
}