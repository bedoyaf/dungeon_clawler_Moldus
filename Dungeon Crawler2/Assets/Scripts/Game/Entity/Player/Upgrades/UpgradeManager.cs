using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private UpgradesUIManager UI;
    [SerializeField] private List<UpgradeSO> allUpgrades;

    [SerializeField] private UnityEvent onOpeningShopUpgradeUI = new UnityEvent();
    [SerializeField] private UnityEvent onClosingShopUpgradeUI = new UnityEvent();

    private List<UpgradeSO> availableUpgrades;
    private List <UpgradeSO> activatedUpgrades = new List<UpgradeSO>();

    private List<UpgradeSO> UpgradesOnCards;

    private int displayCardsNum = 3;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
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
        Debug.Log(actualyAviableUpgrades.Count);
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
}
