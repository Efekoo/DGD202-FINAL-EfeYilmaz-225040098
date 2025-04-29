using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration = 1f;
    public float extraWaitBeforeSceneLoad = 0.7f;

    void Awake()
    {
        if (fadeImage != null)
            fadeImage.raycastTarget = false; // UI etkileþimi engellenmesin
    }

    void Start()
    {
        if (fadeImage != null)
            StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        if (fadeImage != null)
            StartCoroutine(FadeOut(sceneName));
        else
            SceneManager.LoadScene(sceneName); // failsafe
    }

    IEnumerator FadeIn()
    {
        float t = fadeDuration;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            SetFadeAlpha(t / fadeDuration);
            yield return null;
        }

        SetFadeAlpha(0f);
    }

    IEnumerator FadeOut(string sceneName)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            SetFadeAlpha(t / fadeDuration);
            yield return null;
        }

        SetFadeAlpha(1f);
        yield return new WaitForSeconds(extraWaitBeforeSceneLoad);

        SceneManager.LoadScene(sceneName);
    }

    void SetFadeAlpha(float alpha)
    {
        if (fadeImage != null)
            fadeImage.color = new Color(0f, 0f, 0f, Mathf.Clamp01(alpha));
    }
}