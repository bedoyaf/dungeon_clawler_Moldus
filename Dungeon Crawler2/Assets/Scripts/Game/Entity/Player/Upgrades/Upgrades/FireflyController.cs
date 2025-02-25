using Pathfinding;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine.AI;
using DG.Tweening;
using System;
using UnityEngine.UIElements;

/// <summary>
/// Manages a ball of light that follows the player and guides him to the exit
/// </summary>
public class FireflyController : MonoBehaviour
{
    //path finding
    [SerializeField] protected Transform target;
    [SerializeField] private int movementSpeed = 5;
    [SerializeField] private float distance_from_player = 10;

    [SerializeField] private string TagOfEndOfLevel = "LevelExit";

    protected AIDestinationSetter destinationSetter;
    protected AIPath aiPath;
    //helpers
    private GameObject player;

    void Start()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        destinationSetter.target = target;
        aiPath.maxSpeed = movementSpeed;

        var dungeonGenerator = FindFirstObjectByType<ProceduralGenerationRoomGenerator>();
        if (dungeonGenerator != null)
        {
            dungeonGenerator.OnLevelRegeration.AddListener(OnLevelRegenerated);
        }

        StartCoroutine(FireflyBehaviour());
    }

    /// <summary>
    /// sets up the firefly by giving it the exit and player
    /// </summary>
    /// <param name="_player"></param>
    public void Initialize( GameObject _player)
    {
        GameObject exit = GameObject.FindGameObjectWithTag(TagOfEndOfLevel);
        target = exit.transform;
        player = _player;
    }

    /// <summary>
    /// Makes it follow the player, or go to the exit
    /// </summary>
    private IEnumerator FireflyBehaviour()
    {
        aiPath.canMove = true;
        while (true)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if(distanceToPlayer > distance_from_player) 
            {
                destinationSetter.target = player.transform;
                aiPath.canMove = true;
            }
            else if(distanceToPlayer <=distanceToTarget)
            {
                destinationSetter.target = target;
                aiPath.canMove = true;
            }

            yield return null;
        }
    }

    /// <summary>
    /// Makes sure the firefly is setup properly after a new level has been regenerated 
    /// </summary>
    private void OnLevelRegenerated()
    {
        GameObject exit = GameObject.FindGameObjectWithTag(TagOfEndOfLevel);
        transform.position = player.transform.position;

        target = exit.transform;
        destinationSetter.target = target;
    }
}
