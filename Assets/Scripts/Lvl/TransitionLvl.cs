using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionLvl : MonoBehaviour
{
    public static TransitionLvl Instance;

    private GameObject fadeCanvasObject;
    private Image fadeImage;
    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            FadeObject();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneWith(string sceneName)
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionToLvl(sceneName));
        }
    }

    public void LoadSceneWith(int sceneNumber)
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionToLvl(sceneNumber));
        }
    }

    private IEnumerator TransitionToLvl(string sceneName)
    {
        isTransitioning = true;

        yield return StartCoroutine(Fade(0f, 1f));
        SceneManager.LoadScene(sceneName);
        yield return new WaitForEndOfFrame();
        yield return StartCoroutine(Fade(1f, 0f));

        isTransitioning = false;
    }

    private IEnumerator TransitionToLvl(int sceneNumber)
    {
        isTransitioning = true;

        yield return StartCoroutine(Fade(0f, 1f));
        SceneManager.LoadScene(sceneNumber);
        yield return new WaitForEndOfFrame();
        yield return StartCoroutine(Fade(1f, 0f));

        isTransitioning = false;
    }

    private IEnumerator Fade(float startAlpha, float targetAlpha)
    {
        float duration = 1f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, targetAlpha);
    }

    public void FadeObject()
    {
        fadeCanvasObject = new GameObject("Fade");
        fadeCanvasObject.transform.SetParent(transform);

        Canvas canvas = fadeCanvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        GameObject imageObject = new GameObject("FadeImage");
        imageObject.transform.SetParent(fadeCanvasObject.transform);
        fadeImage = imageObject.AddComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 0);
        RectTransform rect = fadeImage.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }
}