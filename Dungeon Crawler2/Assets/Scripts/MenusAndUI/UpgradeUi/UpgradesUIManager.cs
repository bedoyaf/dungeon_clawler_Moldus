using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpgradesUIManager : MonoBehaviour
{
    [SerializeField] private UnityEvent<UpgradeSO> onUpgradePicked = new UnityEvent<UpgradeSO>();

    [SerializeField] private List<GameObject> cards;
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

    private void UpgradePicked(UpgradeSO pickedUpgrade)
    {
        Debug.Log("Picked upgrade: " + pickedUpgrade.abilityName);
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
