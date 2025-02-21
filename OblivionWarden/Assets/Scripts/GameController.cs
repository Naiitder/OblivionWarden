using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public static GameController instance;
    public PlayerManager PlayerManager;

    float time = 0;
    [SerializeField] TMP_Text timeText;

    int score = 0;
    [SerializeField] TMP_Text scoreText;

    [SerializeField] GameObject deadPanel;

    [SerializeField] AudioSource audioSource;
    [SerializeField] public AudioClip levelUpSound;
    [SerializeField] public AudioClip dieSound;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 0;

        deadPanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
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

    public void ActiveDeadPanel()
    {
        deadPanel.SetActive(true);
    }

    public void RestartButton(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void PlayAudioClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
