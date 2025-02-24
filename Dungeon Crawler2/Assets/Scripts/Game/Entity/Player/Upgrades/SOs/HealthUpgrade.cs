using UnityEngine;

[CreateAssetMenu(fileName = "HealthUpgrade", menuName = "Upgrades/Health Upgrade")]
public class HealthUpgrade : UpgradeSO
{
    public int healthIncrease = 10;

    public override void Activate(GameObject player)
    {
        player.GetComponent<HealthController>().IncreaseMaxHealth(healthIncrease);
        player.GetComponent<HealthController>().Heal();
    }

    public override void Revert(GameObject player)
    {
        player.GetComponent <HealthController>().Heal(-healthIncrease);
        player.GetComponent<HealthController>().IncreaseMaxHealth(-healthIncrease);
    }
}