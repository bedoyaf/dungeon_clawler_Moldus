using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private UpgradesUIManager UI;
    [SerializeField] private List<UpgradeSO> allUpgrades;  //all the upgrades, 

    [SerializeField] private UnityEvent onOpeningShopUpgradeUI = new UnityEvent();
    [SerializeField] private UnityEvent onClosingShopUpgradeUI = new UnityEvent();

    [Header("Firefly setup")]
    [SerializeField] private GameObject levelExit;

    private List<UpgradeSO> availableUpgrades;  //available to be shown
    public List<UpgradeSO> activatedUpgrades { get; private set; } = new List<UpgradeSO>();  //being used by the player

    private List<UpgradeSO> UpgradesOnCards;

    private int displayCardsNum = 3;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (UpgradesOnCards==null) chooseAndDisplayNewUpgrades();
        }
    }

    private void Awake()
    {
        LoadUpgradesFromResources();
       // UI.AddUpgradeListener(HandlePickedUpgrade);
    }

    private void LoadUpgradesFromResources()
    {
        allUpgrades = Resources.LoadAll<UpgradeSO>("UpgradeSOs").ToList();
        availableUpgrades = allUpgrades;
        Debug.Log($"Loaded {allUpgrades.Count} upgrades.");
    }

    public void chooseAndDisplayNewUpgrades()
    {
        var actualyAviableUpgrades = getAllAviableUpgrades();
        if (actualyAviableUpgrades.Count==0)
        {
            Debug.LogWarning("Not enough upgrades available!");
            return;
        }
        onOpeningShopUpgradeUI.Invoke();
        UI.gameObject.SetActive(true);

        UpgradesOnCards = ChooseUpgradesForCardsFromList(actualyAviableUpgrades);
        UI.ShowUpgradeCards(UpgradesOnCards);
    }

    private List<UpgradeSO> ChooseUpgradesForCardsFromList(List<UpgradeSO> upgrades)
    {
        List<UpgradeSO> chosenUpgrades = new List<UpgradeSO>();
        int numOfUpgradesToChoose = (upgrades.Count < displayCardsNum) ? upgrades.Count : displayCardsNum; //g get 3 cards, or lower if needed
        while (chosenUpgrades.Count < numOfUpgradesToChoose)
        {
            int randomIndex = Random.Range(0, upgrades.Count);
            UpgradeSO randomUpgrade = upgrades[randomIndex];
            //store
            chosenUpgrades.Add(randomUpgrade);
            //removeval
            availableUpgrades.Remove(randomUpgrade);
            upgrades.Remove(randomUpgrade);
        }
        return chosenUpgrades;
    }

    private List<UpgradeSO> getAllAviableUpgrades()
    {
        List<UpgradeSO> available = new List<UpgradeSO> ();
        foreach (var u in availableUpgrades)
        {
            if(CheckIfItHasLowerLevelVersionAlready(u)) available.Add(u);
        }
        return available;
    }

    private bool CheckIfItHasLowerLevelVersionAlready(UpgradeSO newupgrade)
    {
        if(newupgrade.level == 1) return true; 

        foreach(var u in activatedUpgrades)
        {
            if (u.level + 1 == newupgrade.level && u.ability == newupgrade.ability) return true;
        }
        return false;
    }


    private void HandlePickedUpgrade(UpgradeSO pickedUpgrade)
    {
        UI.gameObject.SetActive(false);
        onClosingShopUpgradeUI.Invoke();
        foreach (var u in UpgradesOnCards)
        {
            if(u != pickedUpgrade) availableUpgrades.Add(u);
        }
        UpgradesOnCards = null;
        activatedUpgrades.Add(pickedUpgrade);
        pickedUpgrade.Activate(gameObject);
    }

    private void OnEnable()
    {
        UI.AddUpgradeListener(HandlePickedUpgrade);
    }

    private void OnDisable()
    {
        UI.RemoveUpgradeListener(HandlePickedUpgrade);
    }


    //Todo should update any upgrades that need it
    public void RevertAllUpgrades()
    {
        foreach(var  upgrade in activatedUpgrades)
        {
            upgrade.Revert(gameObject);
        }
    }

    public void LoadUpgrades(List<UpgradeSO> loadedUpgrades)
    {
        RevertAllUpgrades();
        foreach(var upgrade in loadedUpgrades)
        {
            activatedUpgrades.Add(upgrade);
            upgrade.Activate(gameObject);
        }
    }
}
