using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndOfLevelController : MonoBehaviour
{
    [SerializeField] public UnityEvent LevelCompleted;
    /// <summary>
    /// Just checks if someOne entered the end of level and if its the Player it invokes the end of the level event
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LevelCompleted.Invoke();
        }
    }
}
