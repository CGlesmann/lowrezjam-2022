using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(CollisionHandler))]
public class PlayerMovement : MonoBehaviour
{
	public float jumpHeight = 4;
	public float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	[SerializeField] float moveSpeed = 6;

	float gravity;
	float jumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	PlayerAnimatorController animController;
	PlayerControls playerControls;
	CollisionHandler controller;

	[HideInInspector] public Vector3 lastVelocity;

	void Start()
	{
		playerControls = new PlayerControls();
		playerControls.Enable();

		animController = GetComponent<PlayerAnimatorController>();
		controller = GetComponent<CollisionHandler>();

		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
	}

    void Update()
	{
		if (controller.collisions.above || controller.collisions.below)
		{
			velocity.y = 0;
		}

		float targetVelocityX = playerControls.Movement.HorizontalMovement.ReadValue<float>() * moveSpeed;
		if (targetVelocityX != 0f)
        {
			animController.SetDirection(Mathf.Sign(targetVelocityX));
		}

		if ((playerControls.Movement.Jump.ReadValue<float>() == 1f) && controller.collisions.below)
        {
			velocity.y = jumpVelocity;
		}

		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;

		controller.Move(velocity * Time.deltaTime);

		if (velocity != Vector3.zero)
        {
			lastVelocity = velocity;

		}
	}
}