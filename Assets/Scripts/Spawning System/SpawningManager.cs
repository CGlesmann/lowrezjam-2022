using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningManager : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Scrolling Settings")]
    [SerializeField] private float scrollSpeed;

    [Header("Spawning Settings")]
    [SerializeField] private float spawningYPosDelay;
    [SerializeField] private Transform spawnPointReference;
    [SerializeField] private SpawnablePlatformGroup lastSpawnedGroup;

    [Header("UI References")]
    [SerializeField] private ScoreText scoreText;

    private GameObject newSpawnedGroupToDestroy;
    private bool isScrolling = false;
    private bool isSpawning = false;

    private float nextYPosSpawningPoint;

    private void Update()
    {
        if (isSpawning)
        {
            HandleSpawning();
        }

        if (isScrolling)
        {
            HandleScrolling();
        }
    }

    public void BeginScrolling()
    {
        isScrolling = true;
    }

    public void FinishScrolling()
    {
        isScrolling = false;
    }

    private void HandleScrolling()
    {
        if (cameraTransform != null)
        {
            cameraTransform.transform.position = new Vector3(
                cameraTransform.transform.position.x,
                cameraTransform.transform.position.y + (scrollSpeed * Time.deltaTime),
                cameraTransform.transform.position.z
            );
        }
    }

    public void BeginSpawning()
    {
        isSpawning = true;
        nextYPosSpawningPoint = spawnPointReference.position.y;
    }

    private void HandleSpawning()
    {
        if (spawnPointReference.position.y < nextYPosSpawningPoint)
        {
            return;
        }

        GameObject newGroupPrefabToSpawn = lastSpawnedGroup.GetAvailableSpawnableGroup();
        GameObject newGroupPlatformInstance = GameObject.Instantiate(
            newGroupPrefabToSpawn,
            new Vector3(
                0f, nextYPosSpawningPoint, 0f
            ), 
            Quaternion.identity
        );

        if (newSpawnedGroupToDestroy != null)
        {
            GameObject.Destroy(newSpawnedGroupToDestroy);
            scoreText.UpdateScore();
        }
        newSpawnedGroupToDestroy = lastSpawnedGroup.gameObject;

        lastSpawnedGroup = newGroupPlatformInstance.GetComponent<SpawnablePlatformGroup>();
        nextYPosSpawningPoint = nextYPosSpawningPoint + spawningYPosDelay;
    }
}
