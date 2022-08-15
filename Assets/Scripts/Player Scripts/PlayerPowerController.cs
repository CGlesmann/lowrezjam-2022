using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPowerController : MonoBehaviour
{
    [Header("Prefab References")]
    [SerializeField] private GameObject icePlatformPrefab;
    [SerializeField] private GameObject cloudShotPrefab;

    [Header("Object References")]
    [SerializeField] private Transform icePlatformSpawnPoint;
    [SerializeField] private Transform cloudShotParent;

    [Header("Ice Platform Settings")]
    [SerializeField] private float icePlatformDelay;
    [SerializeField] private float cloudShotDelay;
    [SerializeField] private float downwardIcePlatformDelay;

    private PlayerAnimatorController playerAnimationController;
    private PlayerMovement playerMovement;
    private PlayerControls controlHandler;

    private bool isPerformingDownwardIcePlatform;
    private float remainingIcePlatformDelay;
    private float remainingCloudShotDelay;
    private float remainingDownwardIcePlatformDelay;

    private void OnDestroy()
    {
        if (controlHandler != null)
        {
            controlHandler.Movement.HorizontalMovement.performed -= HandleHorizontalPerformed;
            controlHandler.Abilities.IcePlatform.performed -= HandleIcePlatformPerformed;
            controlHandler.Abilities.CloudShot.performed -= HandleCloudShotPerformed;
            controlHandler.Abilities.DownShotModifer.performed -= HandleIceShotDownModifier;

            controlHandler = null;
        }
    }

    private void Awake()
    {
        // Component Refs
        playerAnimationController = GetComponent<PlayerAnimatorController>();
        playerMovement = GetComponent<PlayerMovement>();

        // Input Setup
        controlHandler = new PlayerControls();
        controlHandler.Movement.HorizontalMovement.performed += HandleHorizontalPerformed;
        controlHandler.Abilities.IcePlatform.performed += HandleIcePlatformPerformed;
        controlHandler.Abilities.CloudShot.performed += HandleCloudShotPerformed;
        controlHandler.Abilities.DownShotModifer.performed += HandleIceShotDownModifier;

        controlHandler.Enable();

        // Ability Setup
        isPerformingDownwardIcePlatform = false;
        remainingIcePlatformDelay = 0f;
        remainingCloudShotDelay = 0f;
    }

    private void Update()
    {
        if (remainingIcePlatformDelay > 0f)
        {
            remainingIcePlatformDelay -= Time.deltaTime;
        }

        if (remainingCloudShotDelay > 0f)
        {
            remainingCloudShotDelay -= Time.deltaTime;
        }

        if (remainingDownwardIcePlatformDelay > 0f)
        {
            remainingDownwardIcePlatformDelay -= Time.deltaTime;
        }
    }

    private void HandleHorizontalPerformed(InputAction.CallbackContext ctx)
    {
        float currentValue = ctx.ReadValue<float>();
        if (ctx.phase == InputActionPhase.Performed && currentValue != 0f)
        {
            Vector3 spawnPointPosition = icePlatformSpawnPoint.localPosition;
            spawnPointPosition.x = Mathf.Abs(spawnPointPosition.x) * Mathf.Sign(currentValue);

            icePlatformSpawnPoint.localPosition = spawnPointPosition;
        }
    }

    private void HandleIceShotDownModifier(InputAction.CallbackContext ctx)
    {
        if (remainingDownwardIcePlatformDelay > 0f || ctx.ReadValue<float>() == 0f) return;

        if (!playerMovement.collisions.below || isPerformingDownwardIcePlatform)
        {
            float currentValue = ctx.ReadValue<float>();

            // Isn't pressed down
            if (currentValue == 0)
            {
                isPerformingDownwardIcePlatform = false;
            }
            else if (currentValue == 1)
            {
                // Is being pressed down
                isPerformingDownwardIcePlatform = true;
            }
            
        }
    }

    private void HandleIcePlatformPerformed(InputAction.CallbackContext ctx)
    {
        if (remainingIcePlatformDelay > 0f) return;

        if (ctx.phase == InputActionPhase.Performed && ctx.ReadValue<float>() == 1f)
        {
            // Starting Animation
            playerAnimationController.StartIceShotAnimation();

            // Pausing Falling
            if (isPerformingDownwardIcePlatform)
            {
                playerMovement.isGravityEnabled = false;
                remainingDownwardIcePlatformDelay = downwardIcePlatformDelay;
            }

            // Setting Cooldown
            remainingIcePlatformDelay = icePlatformDelay;
        }
    }

    private void HandleCloudShotPerformed(InputAction.CallbackContext ctx)
    {
        if (CloudShot.activeCloudShotInstance != null || remainingCloudShotDelay > 0f) return;

        if (ctx.phase == InputActionPhase.Performed && ctx.ReadValue<float>() == 1f)
        {
            // Starting Animation
            playerAnimationController.StartCloudShotAnimation();

            // Setting Cooldown
            remainingCloudShotDelay = cloudShotDelay;
        }
    }

    // Called by IceShot Animation
    public void CreateIcePlatform()
    {
        // Create the Ice Platform
        GameObject newIcePlatformObj = GameObject.Instantiate(icePlatformPrefab, icePlatformSpawnPoint.position, Quaternion.identity);

        IcePlatform newIcePlatformHandler = newIcePlatformObj.GetComponent<IcePlatform>();
        float movementDirMod = Mathf.Sign(icePlatformSpawnPoint.localPosition.x);

        // Resume Falling
        playerMovement.isGravityEnabled = true;

        if (!isPerformingDownwardIcePlatform)
        {
            newIcePlatformHandler.InitializePlatform(false, movementDirMod, IcePlatform.MoveDirection.Horizontal);
            return;
        }
        else
        {
            newIcePlatformHandler.transform.position = new Vector3(
                newIcePlatformHandler.transform.position.x,
                transform.position.y - 1.25f,
                newIcePlatformHandler.transform.position.z
            );

            /*
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + 0.25f,
                transform.position.z
            );
            */

            isPerformingDownwardIcePlatform = false;
            newIcePlatformHandler.InitializePlatform(true, movementDirMod, IcePlatform.MoveDirection.Horizontal);
        }
    }

    // Called by CloudShot Animation
    public void CreateCloudShot()
    {
        // Create the Ice Platform
        GameObject.Instantiate(cloudShotPrefab, transform.position, Quaternion.identity, cloudShotParent);
    }
}
