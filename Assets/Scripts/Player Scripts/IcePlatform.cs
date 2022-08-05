using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePlatform : MonoBehaviour
{
    public enum MoveDirection { Horizontal, Vertical }

    [Header("Ice Platform Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lifeSpan;

    private CollisionHandler collisionHandler;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        collisionHandler = GetComponent<CollisionHandler>();
    }

    private void Update()
    {
        if (velocity != Vector3.zero)
        {
            collisionHandler.Move(velocity * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        velocity = Vector3.zero;
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
