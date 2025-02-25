using UnityEngine;

/// <summary>
/// activates the rage damage modifier
/// </summary>
[CreateAssetMenu(fileName = "RageUpgrade", menuName = "Upgrades/Rage Upgrade")]
public class RageUpgrade : UpgradeSO
{
    public override void Activate(GameObject player)
    {
        if (player.GetComponent<RageController>() == null) // Prevent duplicates
        {
            player.AddComponent<RageController>();
        }
    }

    public override void Revert(GameObject player)
    {
        player.GetComponent<PlayerShootingController>().UpdateDamageModifiersRage(1f);
        RageController controller = player.GetComponent<RageController>();
        
        if (controller != null)
        {
            Destroy(controller);
        }
    }
}