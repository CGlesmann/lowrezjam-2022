using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayStartTrigger : MonoBehaviour
{
    [Header("Component Refs")]
    [SerializeField] private SpawningManager spawnManager;

    private bool hasInitialized = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasInitialized)
        {
            hasInitialized = true;

            spawnManager.BeginSpawning();
            spawnManager.BeginScrolling();
        }
    }
}
