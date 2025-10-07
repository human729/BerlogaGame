using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimateController : MonoBehaviour
{
    public TextMeshProUGUI textComponent;  // твой TMP текст
    public float speed = 30f;              // скорость движения
    public float stopY = 6.144f;             // координата Y, где остановиться
    public Button nextButton;              // кнопка, которую покажем после остановки

    private RectTransform rect;
    private bool isStopped = false;

    void Start()
    {
        rect = textComponent.GetComponent<RectTransform>();

        // Прячем кнопку до момента остановки
        if (nextButton != null)
            nextButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isStopped) return;

        if (rect.localPosition.y < stopY)
        {
            rect.localPosition += Vector3.up * speed * Time.deltaTime;
        }
        else
        {
            // Остановились
            rect.localPosition = new Vector3(rect.localPosition.x, stopY, rect.localPosition.z);
            isStopped = true;

            // Включаем кнопку
            if (nextButton != null)
                nextButton.gameObject.SetActive(true);
        }
    }
}