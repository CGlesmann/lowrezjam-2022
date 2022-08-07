using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlatform : MonoBehaviour
{
    public enum MoveDirection { Horizontal, Vertical }

    [Header("Ice Platform Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lifeSpan;

    private SpriteRenderer sprRenderer;
    private CollisionHandler collisionHandler;

    private Vector3 velocity = Vector3.zero;
    private float remainingLifespan;

    private void Awake()
    {
        collisionHandler = GetComponent<CollisionHandler>();
        sprRenderer = GetComponent<SpriteRenderer>();

        sprRenderer.color = new Color(sprRenderer.color.r, sprRenderer.color.g, sprRenderer.color.b, 1);
    }

    private void Update()
    {
        if (!collisionHandler.collisions.HasCollision())
        {
            // Handle Moving
            collisionHandler.Move(velocity * Time.deltaTime);
        }
        else
        {
            if (velocity != Vector3.zero)
            {
                velocity = Vector3.zero;
                remainingLifespan = lifeSpan;

                return;
            }

            // Handle Melting
            remainingLifespan -= (Time.deltaTime * GameManager.meltingFactor);
            if (remainingLifespan <= 0f)
            {
                GameObject.Destroy(gameObject);
                return;
            }

            sprRenderer.color = new Color(sprRenderer.color.r, sprRenderer.color.g, sprRenderer.color.b, remainingLifespan / lifeSpan);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DespawnTrigger"))
        {
            GameObject.Destroy(gameObject);
        }
    }

    public void InitializePlatform(float directionMod, MoveDirection dir)
    {
        switch(dir)
        {
            case MoveDirection.Horizontal:
                velocity = new Vector3(moveSpeed * directionMod, 0f, 0f);
                break;

            case MoveDirection.Vertical:
                velocity = new Vector3(0f, moveSpeed * directionMod, 0f);
                break;
        }
    }
}
