using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudShot : MonoBehaviour
{
    public static bool isCloudShotActive = false;

    [Header("Cloud Shot Settings")]
    [SerializeField] private Vector2 cloudShotSpeed = new Vector2(2f, 5f);
    [SerializeField] private float cloudShotLifetime = 3f;

    private Animator cloudShotAnim;
    private float acceptanceDistance = 0.1f;
    private float remainingCloudshotLife;

    private bool isMoving = true;
    private bool hasGrown = false;

    private void OnDestroy()
    {
        GameManager.meltingFactor = 1f;
        isCloudShotActive = false;
    }

    private void Awake()
    {
        cloudShotAnim = GetComponent<Animator>();
        cloudShotAnim.SetBool("IsMoving", true);

        GameManager.meltingFactor = 0f;
        isCloudShotActive = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.Translate(new Vector3(0f, cloudShotSpeed.y * Time.deltaTime, 0f), Space.World);
            if (Mathf.Abs(transform.parent.position.y - transform.position.y) <= acceptanceDistance)
            {
                HandleStartGrowth();
            }
        }
        else if (hasGrown)
        {
            transform.Translate(new Vector3(cloudShotSpeed.x * Time.deltaTime, 0f, 0f), Space.World);

            remainingCloudshotLife -= Time.deltaTime;
            if (remainingCloudshotLife <= 0f)
            {
                HandleEndCloudShot();
            }
        }
    }

    public void HandleStartGrowth()
    {
        isMoving = false;

        cloudShotAnim.SetBool("IsMoving", false);
        cloudShotAnim.SetTrigger("Grow");
    }

    // Called by grow animation
    public void HandleFinishGrowth()
    {
        remainingCloudshotLife = cloudShotLifetime;
        hasGrown = true;
    }

    public void HandleEndCloudShot()
    {
        GameObject.Destroy(gameObject);
    }
}
