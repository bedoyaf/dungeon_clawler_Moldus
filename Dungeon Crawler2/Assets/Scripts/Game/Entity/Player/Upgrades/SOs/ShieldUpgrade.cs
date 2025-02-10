using UnityEngine;

[CreateAssetMenu(fileName = "ShieldUpgrade", menuName = "Upgrades/Shield Upgrade")]
public class ShieldUpgrade : UpgradeSO
{
    [SerializeField] private GameObject shieldPrefab;
    public override void Activate(GameObject player)
    {
        GameObject shield = Instantiate(shieldPrefab, player.transform);
        shield.name = "Shield"; 

        shield.transform.localPosition = Vector3.zero;

        ShieldController shieldController = shield.GetComponent<ShieldController>();

        player.GetComponent<HealthController>().shield = shield.GetComponent<ShieldController>();

        shield.SetActive(true);
    }
}