using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnablePlatformGroup : MonoBehaviour
{
    [Header("Group Settings")]

    [Tooltip("List of other SpawnablePlatformGroup Prefabs that can be spawned after this instance")]
    public List<GameObject> connectableSpawnableGroups;

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
