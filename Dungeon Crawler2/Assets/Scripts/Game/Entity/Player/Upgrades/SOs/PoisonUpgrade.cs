using UnityEngine;

/// <summary>
/// Poison added to players attacks
/// </summary>
[CreateAssetMenu(fileName = "PoisonUpgrade", menuName = "Upgrades/Poison Upgrade")]
public class PoisonUpgrade : UpgradeSO
{
    [SerializeField] private PoisonController poison;
    public override void Activate(GameObject player)
    {
        player.GetComponent<PlayerShootingController>().AssignStatusEffect(poison);
    }

    public override void Revert(GameObject player)
    {
        player.GetComponent<PlayerShootingController>().AssignStatusEffect(null);
    }
}