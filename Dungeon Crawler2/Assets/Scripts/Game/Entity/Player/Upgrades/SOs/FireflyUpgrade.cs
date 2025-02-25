using UnityEngine;

/// <summary>
/// Instantiates a firefly that guides the player to the exit
/// </summary>
[CreateAssetMenu(fileName = "FireflyUpgrade", menuName = "Upgrades/Firefly Upgrade")]
public class FireFlyUpgrade : UpgradeSO
{
    [SerializeField] private GameObject fireflyPrefab;

    private GameObject theFirefly; 
    public override void Activate(GameObject player)
    {

        GameObject firefly = Instantiate(fireflyPrefab, player.transform.position, Quaternion.identity);
        firefly.GetComponent<FireflyController>().Initialize( player);

        firefly.name = "FireFly";
        theFirefly = firefly;
    }

    public override void Revert(GameObject player)
    {
        Destroy(theFirefly);
    }
}