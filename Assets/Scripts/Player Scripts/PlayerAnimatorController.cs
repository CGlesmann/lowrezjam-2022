using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator playerAnim;
    private SpriteRenderer rend;

    private void Awake()
    {
        playerAnim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    public void ToggleWalkAnimation(bool isMoving)
    {
        if (!playerAnim)
        {
            Debug.LogError("Couldn't find Player Animator Reference, won't toggle Moving anim");
            return;
        }

        playerAnim.SetBool("IsMoving", isMoving);
    }

    // Set to true at the end of the jump animation
    public void StartFallingAnimation() { ToggleFallingAnimation(true); }

    public void ToggleFallingAnimation(bool isFalling)
    {
        if (!playerAnim)
        {
            Debug.LogError("Couldn't find Player Animator Reference, won't toggle Falling anim");
            return;
        }

        playerAnim.SetBool("IsFalling", isFalling);
    }

    public void StartJumpAnimation()
    {
        if (!playerAnim)
        {
            Debug.LogError("Couldn't find Player Animator Reference, won't Start Jump anim");
            return;
        }

        playerAnim.SetTrigger("Jump");
    }

    public void StartIceShotAnimation()
    {
        if (!playerAnim)
        {
            Debug.LogError("Couldn't find Player Animator Reference, won't Start IceShot anim");
            return;
        }

        playerAnim.SetTrigger("IceShot");
    }

    public void StartCloudShotAnimation()
    {
        if (!playerAnim)
        {
            Debug.LogError("Couldn't find Player Animator Reference, won't Start CloudShot anim");
            return;
        }

        playerAnim.SetTrigger("CloudShot");
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
