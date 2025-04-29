using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Fade & UI")]
    public ScreenFader screenFader;
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        ShowHighScore();
    }

    void ShowHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (highScoreText != null)
            highScoreText.text = $"High Score: {highScore}";
    }

    public void PlayGame()
    {
        if (screenFader != null)
            screenFader.FadeToScene("GameScene");
        else
            SceneManager.LoadScene("GameScene"); // failsafe
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Oyun kapandý.");
    }
}