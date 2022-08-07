using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentChunk : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DespawnTrigger"))
        {
            GameObject.Destroy(gameObject);
        }
    }
}
