using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnablePlatformGroup : MonoBehaviour
{
    #if UNITY_EDITOR
    [Header("DEBUG Settings")]
    [SerializeField] private Vector3 platformGroupSize = new Vector3(8f, 8f, 1f);
    #endif

    [Header("Group Settings")]
    [Tooltip("List of other SpawnablePlatformGroup Prefabs that can be spawned after this instance")]
    public List<GameObject> connectableSpawnableGroups;

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, platformGroupSize);
    }
    #endif

    public void MoveGroup(Vector3 moveSpeed)
    {
        transform.position += moveSpeed;
    }

    public GameObject GetAvailableSpawnableGroup()
    {
        if (connectableSpawnableGroups == null || connectableSpawnableGroups.Count == 0)
        {
            return null;
        }

        return connectableSpawnableGroups[Random.Range(0, connectableSpawnableGroups.Count)];
    }
}
