using UnityEngine;

/// <summary>
/// Adds damage to attacks, setup just calls function that increases modifiers
/// </summary>
[CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Upgrades/Damage Upgrade")]
public class DamageUpgrade : UpgradeSO
{
    public float newDamageModifier = 1.3f;
    public override void Activate(GameObject player)
    {
        player.GetComponent<PlayerShootingController>().IncreaseDamageModifier(newDamageModifier);
    }
    public override void Revert(GameObject player)
    {
        player.GetComponent<PlayerShootingController>().IncreaseDamageModifier(1f);
    }
}