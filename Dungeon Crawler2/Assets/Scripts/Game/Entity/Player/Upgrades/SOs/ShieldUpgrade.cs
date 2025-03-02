using UnityEngine;

/// <summary>
/// instantiates a shield for the player that soaks up damage
/// </summary>
[CreateAssetMenu(fileName = "ShieldUpgrade", menuName = "Upgrades/Shield Upgrade")]
public class ShieldUpgrade : UpgradeSO
{
    [SerializeField] private GameObject shieldPrefab;
    private GameObject shieldInstance;
    public override void Activate(GameObject player)
    {
        if(player.GetComponent<HealthController>().shield != null) {
            Debug.LogWarning("Trying to setup a shield while shield exists");
            return;
        }
        GameObject shield = Instantiate(shieldPrefab, player.transform);
        shield.name = "Shield"; 

        shield.transform.localPosition = Vector3.zero;

        ShieldController shieldController = shield.GetComponent<ShieldController>();

        player.GetComponent<HealthController>().shield = shield.GetComponent<ShieldController>();
        player.GetComponent<HealthController>().shield.Initiate();

        shield.SetActive(true);
    }

    public override void Revert(GameObject player)
    {
        if(player.GetComponent<HealthController>().shield != null)
        {
            Destroy(player.GetComponent<HealthController>().shield.gameObject);
            player.GetComponent<HealthController>().shield = null;
        }
    }
}