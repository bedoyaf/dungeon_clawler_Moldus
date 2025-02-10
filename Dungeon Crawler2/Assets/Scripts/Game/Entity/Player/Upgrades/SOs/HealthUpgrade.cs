using UnityEngine;

[CreateAssetMenu(fileName = "HealthUpgrade", menuName = "Upgrades/Health Upgrade")]
public class HealthUpgrade : UpgradeSO
{
    public int healthIncrease = 10;

    public override void Activate(GameObject player)
    {
        player.GetComponent<HealthController>().Heal();
        player.GetComponent<HealthController>().IncreaseMaxHealth(healthIncrease);
        Debug.Log($"{abilityName} applied! Health increased by {healthIncrease}");
    }
}