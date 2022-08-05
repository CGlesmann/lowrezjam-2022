using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private SpriteRenderer rend;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    public void SetDirection(float directionMod)
    {
        switch(directionMod)
        {
            // Left Direction, by default sprite faces right so flip for left facing
            case -1:
                rend.flipX = true;
                break;

            // Right Direction
            case 1:
            case 0:
                rend.flipX = false;
                break;

            default:
                Debug.LogWarning($"Unknown Direction Mod {directionMod}, defaulting to right facing player");
                break;
        }
    }
}
