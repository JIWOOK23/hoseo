using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public int score = 0;              // 실제 점수
    private int displayedScore = 0;    // 화면에 표시되는 점수

    public Text scoreText;

    private Coroutine scoreAnimCoroutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        // 폭탄 먹었을 때 0 처리
        if (amount < 0 && score < 10)
            score = 0;
        else
            score += amount;

        // 이미 코루틴 돌고 있으면 중단하고 새로 시작
        if (scoreAnimCoroutine != null)
            StopCoroutine(scoreAnimCoroutine);

        scoreAnimCoroutine = StartCoroutine(AnimateScoreChange());
    }

    IEnumerator AnimateScoreChange()
    {
        float duration = 1f;
        int startScore = displayedScore;
        int targetScore = score;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            displayedScore = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, t));
            UpdateScoreUI();
            yield return null;
        }

        displayedScore = targetScore;
        UpdateScoreUI();
        scoreAnimCoroutine = null;
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: <color=red>" + displayedScore + "</color>";
    }
}