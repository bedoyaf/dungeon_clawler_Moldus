using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defined SO for the upgrades
/// </summary>
public abstract class UpgradeSO : ScriptableObject
{
    [Header("UI Information")]
    public string abilityName; //for UI purpose
    public Sprite abilityIcon;
    [TextArea] public string description;
    [Header("Gameplay Effects")]
    public int level = 1;
    public string ability; //just for code purpose
    //public List<string> mutualyExclusiveWith; //unused
    //public List<string> prerequisits;
    public abstract void Activate(GameObject player);
    public abstract void Revert(GameObject player);
}
