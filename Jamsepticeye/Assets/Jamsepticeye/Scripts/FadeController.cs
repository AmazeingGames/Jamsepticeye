using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float waitDuration = 2f;

    public static FadeController instance { get; private set; }

    private void Awake()
    {
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            // If another instance exists, destroy this one
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Make sure the image starts transparent
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    // Call this method to trigger the fade effect
    public void TriggerFade()
    {
        StartCoroutine(FadeOutAndIn());
    }
    public void TriggerFadeForever()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOutAndIn()
    {
        // Fade out to black
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        // Wait for specified duration
        yield return new WaitForSeconds(waitDuration);

        // Fade back in
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));
    }

    private IEnumerator FadeIn()
    {
        yield return StartCoroutine(Fade(0f, 1f, 3f));
        yield return new WaitForSeconds(10);
        yield return StartCoroutine(Fade(1f, 0f, 1f));

        SceneManager.LoadScene("Menu");
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            c.a = alpha;
            fadeImage.color = c;
            yield return null;
        }

        // Ensure final value is set
        c.a = endAlpha;
        fadeImage.color = c;
    }
}