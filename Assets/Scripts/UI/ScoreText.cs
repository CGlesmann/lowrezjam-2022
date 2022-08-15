using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Animation References")]
    [SerializeField] private Vector3 endGamePosition;
    [SerializeField] private float transitionSpeed;

    private int score = 0;

    private void Awake()
    {
        score = 0;
        scoreText.text = score.ToString();
    }

    public void UpdateScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void StartEndGameTransition()
    {
        StartCoroutine(LerpToEndPosition());
    }

    private IEnumerator LerpToEndPosition()
    {
        Vector3 startPos = transform.position;
        float completion = 0;

        int iterations = 60;
        while(completion < 1f)
        {
            completion += transitionSpeed / iterations;
            transform.localPosition = Vector3.Lerp(startPos, endGamePosition, completion);

            yield return new WaitForSeconds(transitionSpeed / iterations);
        }
    }
}
