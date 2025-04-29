using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float fireCooldown = 0.25f;
    private float fireTimer;

    [Header("Overheat System")]
    public float heatPerShot = 0.1f;
    public float heatCooldownSpeed = 0.05f;
    public float maxHeat = 1f;
    private float currentHeat = 0f;
    private bool isOverheated = false;
    private float overheatTimer;
    public float overheatCooldown = 2.5f;

    [Header("Overheat UI")]
    public GameObject overheatText;
    public Slider overheatBar;
    public Image overheatFillImage;
    private Vector3 originalFillScale;
    private float pulseSpeed = 20f;
    private float pulseAmount = 0.05f;

    [Header("Audio")]
    public AudioClip shootSound;
    public AudioClip hitSound;
    public AudioClip overheatClickSound;
    private AudioSource audioSource;

    [Header("Health")]
    public int health = 3;
    public TextMeshProUGUI hpText;
    public GameObject hitEffect;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverHighScoreText;

    void Start()
    {
        UpdateHPText();
        audioSource = GetComponent<AudioSource>();

        if (overheatFillImage != null)
            originalFillScale = overheatFillImage.rectTransform.localScale;
    }

    void Update()
    {
        HandleMovement();
        HandleOverheat();
        HandleShooting();
        UpdateOverheatBar();
    }

    void HandleMovement()
    {
        float h = 0f;
        float v = 0f;

        if (Input.GetKey(KeyCode.D)) v = 1f;
        if (Input.GetKey(KeyCode.A)) v = -1f;
        if (Input.GetKey(KeyCode.S)) h = 1f;
        if (Input.GetKey(KeyCode.W)) h = -1f;

        Vector2 move = new Vector2(h, v).normalized;
        transform.Translate(move * speed * Time.deltaTime);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -9f, 9f);
        pos.y = Mathf.Clamp(pos.y, -4f, 6f);
        transform.position = pos;
    }

    void HandleOverheat()
    {
        fireTimer += Time.deltaTime;

        if (currentHeat > 0f)
        {
            currentHeat -= heatCooldownSpeed * Time.deltaTime;
            currentHeat = Mathf.Clamp(currentHeat, 0f, maxHeat);
        }

        if (isOverheated && currentHeat <= 0f)
        {
            isOverheated = false;
            overheatText?.SetActive(false);
        }

        if (isOverheated)
        {
            overheatTimer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) && overheatClickSound != null)
                audioSource.PlayOneShot(overheatClickSound);

            if (overheatTimer >= overheatCooldown)
            {
                isOverheated = false;
                overheatTimer = 0f;
                overheatText?.SetActive(false);
            }

            return;
        }
    }

    void HandleShooting()
    {
        if (!isOverheated && Input.GetKeyDown(KeyCode.Space) && fireTimer >= fireCooldown)
        {
            Fire();
            fireTimer = 0f;

            currentHeat += heatPerShot;
            if (currentHeat >= maxHeat)
            {
                isOverheated = true;
                overheatText?.SetActive(true);
                if (overheatClickSound != null)
                    audioSource.PlayOneShot(overheatClickSound);
            }
        }
        else if (isOverheated && Input.GetKeyDown(KeyCode.Space))
        {
            if (overheatClickSound != null)
                audioSource.PlayOneShot(overheatClickSound);
        }
    }

    void Fire()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        if (shootSound != null)
            audioSource.PlayOneShot(shootSound);
    }

    void UpdateOverheatBar()
    {
        if (overheatBar != null)
        {
            float t = currentHeat / maxHeat;
            overheatBar.value = t;

            if (overheatFillImage != null)
            {
                if (t < 0.4f)
                    overheatFillImage.color = Color.green;
                else if (t < 0.7f)
                    overheatFillImage.color = Color.yellow;
                else
                    overheatFillImage.color = Color.red;

                if (t >= 0.7f)
                {
                    float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
                    overheatFillImage.rectTransform.localScale = new Vector3(scale, scale, 1f);
                }
                else
                {
                    overheatFillImage.rectTransform.localScale = originalFillScale;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateHPText();

        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);

        if (hitEffect != null)
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.2f);
        }

        if (health <= 0)
            ShowGameOver();
    }

    void UpdateHPText()
    {
        if (hpText != null)
            hpText.text = "HP: " + health;
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);

        int level1Score = GameManager.Instance.currentScore;

        if (level1Score > GameManager.Instance.level1HighScore)
        {
            GameManager.Instance.level1HighScore = level1Score;
            PlayerPrefs.SetInt("Level1HighScore", level1Score);
        }

        if (gameOverHighScoreText != null)
            gameOverHighScoreText.text = "Level 1 High Score: " + GameManager.Instance.level1HighScore;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Oyun kapandı.");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}