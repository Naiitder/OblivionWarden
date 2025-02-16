using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    float time = 0;
    [SerializeField] TMP_Text timeText;

    int score = 0;
    [SerializeField] TMP_Text scoreText;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
       if(timeText != null)
        {
            time += Time.deltaTime;
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timeText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        if (scoreText != null)
        {
            score += scoreToAdd;
            scoreText.text = score.ToString();
        }
    }
}
