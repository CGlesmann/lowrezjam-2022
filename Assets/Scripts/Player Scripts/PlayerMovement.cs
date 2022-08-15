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

	public bool isGravityEnabled = true;

	PlayerAnimatorController animController;
	PlayerControls playerControls;
	CollisionHandler controller;

	[HideInInspector] public Vector3 lastVelocity;

	public CollisionHandler.CollisionInfo collisions
    {
		get
        {
			return controller.collisions;
        } 
    }

	void Start()
	{
		playerControls = new PlayerControls();
		playerControls.Enable();

		animController = GetComponent<PlayerAnimatorController>();
		controller = GetComponent<CollisionHandler>();

		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ScreenWrapTrigger"))
        {
			float wrapDir = Mathf.Sign(transform.position.x);
			float positionThreshold = ((collision.gameObject.GetComponent<BoxCollider2D>().size.x / 2) + 0.25f) * wrapDir;

			if (
				(wrapDir == -1 && transform.position.x < positionThreshold) ||
				(wrapDir == 1 && transform.position.x > positionThreshold)
			) {
				transform.position = new Vector3(
					transform.position.x * -1,
					transform.position.y,
					transform.position.z
				);
			}
        }
    }

    void Update()
	{
		if (controller.collisions.above || controller.collisions.below)
		{
			velocity.y = 0;
			animController.ToggleFallingAnimation(false);
		}

		float targetVelocityX = playerControls.Movement.HorizontalMovement.ReadValue<float>() * moveSpeed;
		animController.ToggleWalkAnimation(targetVelocityX != 0f);

		if (targetVelocityX != 0f)
        {
			animController.SetDirection(Mathf.Sign(targetVelocityX));
		}

		if ((playerControls.Movement.Jump.ReadValue<float>() == 1f) && controller.collisions.below)
        {
			velocity.y = jumpVelocity;
			animController.StartJumpAnimation();
		}

		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

		if (isGravityEnabled)
        {
			velocity.y += gravity * Time.deltaTime;
		}

		controller.Move(velocity * Time.deltaTime);

		if (controller.collisions.above)
        {
			TryBreakIcePlatform();
        }

		if (velocity != Vector3.zero)
        {
			lastVelocity = velocity;
		}
	}

	private void TryBreakIcePlatform()
    {
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up);
		if (hit && hit.collider.GetComponent<IcePlatform>() != null)
        {
			GameObject.Destroy(hit.collider.gameObject);
        }
    }
}