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

    private PlayerAnimatorController playerAnimationController; 
    private PlayerControls controlHandler;

    private float remainingIcePlatformDelay;
    private float remainingCloudShotDelay;

    private void OnDestroy()
    {
        if (controlHandler != null)
        {
            controlHandler.Movement.HorizontalMovement.performed -= HandleHorizontalPerformed;
            controlHandler.Abilities.IcePlatform.performed -= HandleIcePlatformPerformed;
        }
    }

    private void Awake()
    {
        // Component Refs
        playerAnimationController = GetComponent<PlayerAnimatorController>();

        // Input Setup
        controlHandler = new PlayerControls();
        controlHandler.Movement.HorizontalMovement.performed += HandleHorizontalPerformed;
        controlHandler.Abilities.IcePlatform.performed += HandleIcePlatformPerformed;
        controlHandler.Abilities.CloudShot.performed += HandleCloudShotPerformed;

        controlHandler.Enable();

        // Ability Setup
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

    private void HandleIcePlatformPerformed(InputAction.CallbackContext ctx)
    {
        if (remainingIcePlatformDelay > 0f)
        {
            return;
        }

        if (ctx.phase == InputActionPhase.Performed && ctx.ReadValue<float>() == 1f)
        {
            // Starting Animation
            playerAnimationController.StartIceShotAnimation();

            // Setting Cooldown
            remainingIcePlatformDelay = icePlatformDelay;
        }
    }

    private void HandleCloudShotPerformed(InputAction.CallbackContext ctx)
    {
        if (!CloudShot.isCloudShotActive && remainingIcePlatformDelay > 0f)
        {
            return;
        }

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

        newIcePlatformHandler.InitializePlatform(movementDirMod, IcePlatform.MoveDirection.Horizontal);
    }

    // Called by CloudShot Animation
    public void CreateCloudShot()
    {
        // Create the Ice Platform
        GameObject.Instantiate(cloudShotPrefab, transform.position, Quaternion.identity, cloudShotParent);
    }
}
