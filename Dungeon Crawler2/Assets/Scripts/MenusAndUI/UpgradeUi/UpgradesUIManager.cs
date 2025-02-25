using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Handles the UI of upgrades
/// these are up to 3 cards the player can choose from, displaying upgrades
/// </summary>
public class UpgradesUIManager : MonoBehaviour
{
    [SerializeField] private UnityEvent<UpgradeSO> onUpgradePicked = new UnityEvent<UpgradeSO>();

    [SerializeField] private List<GameObject> cards;


    /// <summary>
    /// Sets up the upgrades to be displayed
    /// </summary>
    /// <param name="upgrades">upgrades to be shown</param>
    public void ShowUpgradeCards(List<UpgradeSO> upgrades)
    {
        if (cards.Count < upgrades.Count || upgrades.Count ==0) { Debug.LogWarning("Incorrect number of cards and upgrades in UI"); }
        setActiveCards(true);
        int i = 0;

        while(i<upgrades.Count)
        {
            cards[i].GetComponent<UpgradeUICardController>().updateCard(upgrades[i]);
            i++;
        }
        while(i<cards.Count)
        {
            cards[i].SetActive(false);
            i++;
        }
    }


    /// <summary>
    /// turns on/off all cards
    /// </summary>
    /// <param name="t">just a bool so it can be used both ways</param>
    private void setActiveCards(bool t)
    {
        foreach(var card in cards)
        {
            card.SetActive(t);
        }
    }

    private void Start()
    {
        UpgradeUICardController.OnUpgradePicked += UpgradePicked;
    }


    /// <summary>
    /// pressed the certain upgrade card
    /// </summary>
    /// <param name="pickedUpgrade">the upgrade picked</param>
    private void UpgradePicked(UpgradeSO pickedUpgrade)
    {
        onUpgradePicked.Invoke(pickedUpgrade);
    }

    public void AddUpgradeListener(UnityAction<UpgradeSO> listener)
    {
        onUpgradePicked.AddListener(listener);
    }
    public void RemoveUpgradeListener(UnityAction<UpgradeSO> listener)
    {
        onUpgradePicked.RemoveListener(listener);
    }

}
