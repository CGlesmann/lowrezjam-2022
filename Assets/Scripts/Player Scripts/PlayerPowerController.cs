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

    private PlayerControls controlHandler;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        controlHandler = new PlayerControls();
        controlHandler.Movement.HorizontalMovement.performed += HandleHorizontalPerformed;
        controlHandler.Abilities.IcePlatform.performed += HandleIcePlatformPerformed;

        controlHandler.Enable();
    }

    private void HandleHorizontalPerformed(InputAction.CallbackContext ctx)
    {
        Vector3 spawnPointPosition = icePlatformSpawnPoint.localPosition;
        spawnPointPosition.x = Mathf.Abs(spawnPointPosition.x) * Mathf.Sign(ctx.ReadValue<float>());

        icePlatformSpawnPoint.localPosition = spawnPointPosition;
    }

    private void HandleIcePlatformPerformed(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed && ctx.ReadValue<float>() == 1f)
        {
            GameObject newIcePlatformObj = GameObject.Instantiate(icePlatformPrefab, icePlatformSpawnPoint.position, Quaternion.identity);

            IcePlatform newIcePlatformHandler = newIcePlatformObj.GetComponent<IcePlatform>();
            float movementDirMod = Mathf.Sign(playerMovement.lastVelocity.x);

            newIcePlatformHandler.InitializePlatform(movementDirMod, IcePlatform.MoveDirection.Horizontal);
        }
    }
}
