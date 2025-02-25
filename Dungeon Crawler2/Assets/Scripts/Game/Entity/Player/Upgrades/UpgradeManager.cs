using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages the player upgrades
/// </summary>
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

    private bool isShowingUpgrades = false;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (UpgradesOnCards==null) chooseAndDisplayNewUpgrades();
        }
        if(Input.GetKeyDown(KeyCode.KeypadEnter)&& isShowingUpgrades)
        {
            CloseUpgradeScreen();
        }
    }

    private void Awake()
    {
        LoadUpgradesFromResources();
    }

    private void LoadUpgradesFromResources()
    {
        allUpgrades = Resources.LoadAll<UpgradeSO>("UpgradeSOs").ToList();
        availableUpgrades = allUpgrades;
        Debug.Log($"Loaded {allUpgrades.Count} upgrades.");
    }

    /// <summary>
    /// Main function for activating upgrades, checks and chooses them and then activates UI
    /// </summary>
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

    /// <summary>
    /// picks cards to be displayed
    /// </summary>
    /// <param name="upgrades">list of upgrades that havent yet been used</param>
    /// <returns>up to three upgrades that can be displayed on cards</returns>
    private List<UpgradeSO> ChooseUpgradesForCardsFromList(List<UpgradeSO> upgrades)
    {
        List<UpgradeSO> chosenUpgrades = new List<UpgradeSO>();
        int numOfUpgradesToChoose = (upgrades.Count < displayCardsNum) ? upgrades.Count : displayCardsNum; 
        while (chosenUpgrades.Count < numOfUpgradesToChoose)
        {
            int randomIndex = Random.Range(0, upgrades.Count);
            UpgradeSO randomUpgrade = upgrades[randomIndex];

            chosenUpgrades.Add(randomUpgrade);

            availableUpgrades.Remove(randomUpgrade);
            upgrades.Remove(randomUpgrade);
        }
        return chosenUpgrades;
    }

    /// <summary>
    /// Gets all the upgrades that have prerequisited upgrades
    /// </summary>
    /// <returns>List of upgrades that can be used</returns>
    private List<UpgradeSO> getAllAviableUpgrades()
    {
        List<UpgradeSO> available = new List<UpgradeSO> ();
        foreach (var u in availableUpgrades)
        {
            if(CheckIfItHasLowerLevelVersionAlready(u)) available.Add(u);
        }
        return available;
    }

    /// <summary>
    /// checks if the upgrade can be used, by checking if prerequisited upgrades are activated
    /// </summary>
    /// <param name="newupgrade"></param>
    /// <returns>if it has prerequisits, true</returns>
    private bool CheckIfItHasLowerLevelVersionAlready(UpgradeSO newupgrade)
    {
        if(newupgrade.level == 1) return true; 

        foreach(var u in activatedUpgrades)
        {
            if (u.level + 1 == newupgrade.level && u.ability == newupgrade.ability) return true;
        }
        return false;
    }

    /// <summary>
    /// Handles the players choice of upgrade
    /// returns the not chosen and activates the chosen one
    /// </summary>
    /// <param name="pickedUpgrade">the upgrade that has been clicked</param>
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

    /// <summary>
    /// Reverts all upgrades
    /// </summary>
    public void RevertAllUpgrades()
    {
        foreach(var  upgrade in activatedUpgrades)
        {
            upgrade.Revert(gameObject);
        }
    }

    /// <summary>
    /// Reverts all the upgrades and then Activates all the upgrades loaded from the save 
    /// </summary>
    /// <param name="loadedUpgrades">upgrades loaded from the save</param>
    public void LoadUpgrades(List<UpgradeSO> loadedUpgrades)
    {
        RevertAllUpgrades();
        foreach(var upgrade in loadedUpgrades)
        {
            activatedUpgrades.Add(upgrade);
            upgrade.Activate(gameObject);
        }
    }

    /// <summary>
    /// Both reverts and activates all upgrades, useful for safe restart 
    /// </summary>
    public void RevertAndReActivateAllUpgrades()
    {
        foreach(var upgrade in activatedUpgrades)
        {
            upgrade.Revert(gameObject);
        }
        foreach (var upgrade in activatedUpgrades)
        {
            upgrade.Activate(gameObject);
        }
    }

    /// <summary>
    /// Closes upgrade screen, when no upgrade has been picked
    /// </summary>
    public void CloseUpgradeScreen()
    {
        isShowingUpgrades = false;
        UI.gameObject.SetActive(false);
        onClosingShopUpgradeUI.Invoke();
        foreach (var u in UpgradesOnCards)
        {
            availableUpgrades.Add(u);
        }
        UpgradesOnCards = null;
    }
}
