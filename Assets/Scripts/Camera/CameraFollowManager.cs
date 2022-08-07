using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    private Transform playerTransform;
    private Vector3 lastPlayerPosition;

    private bool isFollowing = false;

    private void LateUpdate()
    {
        if (isFollowing)
        {
            if (playerTransform.position.y > lastPlayerPosition.y)
            {
                cameraTransform.position = new Vector3(
                    cameraTransform.position.x,
                    cameraTransform.position.y + (playerTransform.position.y - lastPlayerPosition.y),
                    cameraTransform.position.z
               );

                lastPlayerPosition = playerTransform.position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isFollowing = true;

            playerTransform = collision.transform;
            lastPlayerPosition = playerTransform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isFollowing = false;

            playerTransform = null;
            lastPlayerPosition = Vector3.zero;
        }
    }
}
