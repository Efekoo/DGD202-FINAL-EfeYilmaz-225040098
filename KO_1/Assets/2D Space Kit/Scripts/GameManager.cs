using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentScore = 0;
    public int highScore = 0;
    public int enemyKillCount = 0;
    public int enemyPassedCount = 0;

    public int level1HighScore = 0;
    public bool level2Unlocked = false;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject level2UnlockedText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // sahne geçiþinde kaybolma
        }
        else
        {
            Destroy(gameObject);
        }

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        level1HighScore = PlayerPrefs.GetInt("Level1HighScore", 0);
    }

    void Start()
    {
        UpdateScoreUI();
        UpdateHighScoreUI();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreUI();

        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
            UpdateHighScoreUI();
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore;
    }

    void UpdateHighScoreUI()
    {
        if (highScoreText != null)
            highScoreText.text = "High Score: " + highScore;
    }

    public void EnemyKilled()
    {
        enemyKillCount++;

        if (!level2Unlocked && enemyKillCount >= 30)
        {
            level2Unlocked = true;

            if (level2UnlockedText != null)
                level2UnlockedText.SetActive(true);

            Debug.Log("LEVEL 2 UNLOCKED!");
        }
    }

    public void EnemyPassed()
    {
        enemyPassedCount++;

        if (enemyPassedCount >= 100)
        {
            var player = FindFirstObjectByType<PlayerController>();
            if (player != null)
                player.ShowGameOver();
        }
    }
}