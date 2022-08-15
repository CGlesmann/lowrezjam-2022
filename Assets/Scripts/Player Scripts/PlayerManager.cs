using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private GameManager gameManager;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DeathTrigger") && (transform.position.y < collision.transform.position.y))
        {
            HandlePlayerDeath();
        }
    }

    private void HandlePlayerDeath()
    {
        gameManager.HandleGameFinish();
        GameObject.Destroy(gameObject);

        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
