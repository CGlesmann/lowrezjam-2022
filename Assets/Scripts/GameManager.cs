using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static float meltingFactor = 1f;

    [Header("Object References")]
    [SerializeField] private SpawningManager spawningManager;
    [SerializeField] private ScoreText scoreText;

    private void Awake()
    {
        Application.targetFrameRate = 24;
        Screen.SetResolution(640, 640, FullScreenMode.Windowed);
    }

    public void HandleGameFinish()
    {
        SpawnablePlatformGroup[] allGroups = GameObject.FindObjectsOfType<SpawnablePlatformGroup>();
        foreach(SpawnablePlatformGroup platformGroup in allGroups)
        {
            GameObject.Destroy(platformGroup.gameObject);
        }

        scoreText.StartEndGameTransition();
        spawningManager.FinishScrolling();

        RestartGame();
    }

    private void RestartGame()
    {
        StartCoroutine(DelayedReload());
    }

    private IEnumerator DelayedReload()
    {
        yield return new WaitForSecondsRealtime(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
