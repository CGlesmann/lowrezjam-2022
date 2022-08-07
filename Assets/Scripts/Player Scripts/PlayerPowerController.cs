using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPowerController : MonoBehaviour
{
    [Header("Prefab References")]
    [SerializeField] private GameObject icePlatformPrefab;

    [Header("Object References")]
    [SerializeField] private Transform icePlatformSpawnPoint;

    [Header("Ice Platform Settings")]
    [SerializeField] private float icePlatformDelay;

    private PlayerAnimatorController playerAnimationController; 
    private PlayerControls controlHandler;
    private PlayerMovement playerMovement;

    private float remainingIcePlatformDelay;

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
        playerMovement = GetComponent<PlayerMovement>();

        // Input Setup
        controlHandler = new PlayerControls();
        controlHandler.Movement.HorizontalMovement.performed += HandleHorizontalPerformed;
        controlHandler.Abilities.IcePlatform.performed += HandleIcePlatformPerformed;

        controlHandler.Enable();

        // Ability Setup
        remainingIcePlatformDelay = 0f;
    }

    private void Update()
    {
        if (remainingIcePlatformDelay > 0f)
        {
            remainingIcePlatformDelay -= Time.deltaTime;
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

        // Setting Cooldown
        remainingIcePlatformDelay = icePlatformDelay;
    }
}
