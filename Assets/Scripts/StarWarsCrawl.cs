using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StarWarsCrawl : MonoBehaviour
{
    [Header("Refs")]
    public RectTransform root;           // напр. CrawlRoot (наклоняем по X)
    public RectTransform content;        // напр. CrawlContent (едет вверх, скейлится)
    public TextMeshProUGUI text;         // сам TMP-текст
    public Button nextButton;            // показать после завершения (опц.)

    [Header("Crawl Motion")]
    public float tiltAngle = 25f;        // наклон плоскости (deg)
    public float speed = 120f;           // подъём (лок.ед./сек)
    public float startY = -300f;         // стартовая Y (в системе root)
    public float endY = 900f;            // конечная Y (в системе root)
    public float endScale = 0.7f;        // масштаб в конце
    public bool playOnEnable = true;

    [Header("Per-line fade @ stop line")]
    [Tooltip("Если задан, это визуальная линия стопа (любая UI-точка). Скрипт сам переведёт её в локальные координаты TMP.")]
    public RectTransform stopLine;
    [Tooltip("Если stopLine не задан — абсолют Y линии стопа в ЛОКАЛЬНЫХ координатах TMP-текста.")]
    public float fadeStopYText = 300f;
    [Tooltip("Высота зоны плавного затухания: строки начинают гаснуть за fadeBandHeight ДО линии стопа и полностью исчезают на ней.")]
    public float fadeBandHeight = 250f;
    [Tooltip("Коэффициент скорости затухания (1 = нормально, >1 = быстрее).")]
    public float fadeSpeedScale = 2.5f;
    public bool enablePerLineFade = true;

    private bool playing;

    void Reset()
    {
        if (!root) root = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        if (playOnEnable) Play();
    }

    public void SetText(string value)
    {
        if (text) text.text = value;
    }

    public void Play()
    {
        if (!root || !content || !text) return;

        // наклон плоскости
        var e = root.localEulerAngles; e.x = tiltAngle; root.localEulerAngles = e;

        // старт позы/масштаба
        var p = content.anchoredPosition; p.y = startY; content.anchoredPosition = p;
        content.localScale = Vector3.one;

        if (nextButton) nextButton.gameObject.SetActive(false);

        // подготовка TMP
        text.enableWordWrapping = true;
        text.enableAutoSizing = false;
        text.overflowMode = TextOverflowModes.Overflow;
        text.ForceMeshUpdate();

        playing = true;
    }

    void Update()
    {
        if (!playing) return;

        // движение вверх
        var p = content.anchoredPosition;
        p.y += speed * Time.deltaTime;
        content.anchoredPosition = p;

        // прогресс
        float t = Mathf.InverseLerp(startY, endY, p.y);

        // перспектива
        float s = Mathf.Lerp(1f, endScale, t);
        content.localScale = new Vector3(s, s, 1f);

        // построчное затухание у линии стопа
        if (enablePerLineFade) ApplyLineFade();

        // финиш
        if (p.y >= endY)
        {
            p.y = endY; content.anchoredPosition = p;
            playing = false;
            if (nextButton) nextButton.gameObject.SetActive(true);
        }
    }

    // ====== ПОСТРОЧНОЕ ЗАТУХАНИЕ, ПРИВЯЗАНОЕ К ЛИНИИ STOP ======
    void ApplyLineFade()
    {
        text.ForceMeshUpdate();

        var ti = text.textInfo;
        int charCount = ti.characterCount;
        if (charCount == 0) return;

        // 1) вычисляем Y линии стопа в ЛОКАЛЬНЫХ координатах TMP
        float stopY = GetStopYInTextLocal();            // полностью прозрачно на этой линии
        float startFadeY = stopY - fadeBandHeight;      // начало затухания

        // 2) проходим символы и задаём альфу по строке
        for (int i = 0; i < charCount; i++)
        {
            var ch = ti.characterInfo[i];
            if (!ch.isVisible) continue;

            int line = ch.lineNumber;
            float baselineY = ti.lineInfo[line].baseline; // локальная Y базовой линии этой строки (в системе TMP)

            // k=0 ниже зоны → полностью видно; k=1 на stopY → полностью прозрачно
            float k = Mathf.InverseLerp(startFadeY, stopY, baselineY);

            // ускоряем/замедляем затухание
            k = Mathf.Clamp01(k * fadeSpeedScale);

            // сглаживание (smoothstep)
            k = k * k * (3f - 2f * k);

            byte a = (byte)Mathf.RoundToInt(255f * (1f - k));

            int matIndex = ch.materialReferenceIndex;
            int vIndex = ch.vertexIndex;
            var colors = ti.meshInfo[matIndex].colors32;

            var c0 = colors[vIndex + 0]; c0.a = a;
            var c1 = colors[vIndex + 1]; c1.a = a;
            var c2 = colors[vIndex + 2]; c2.a = a;
            var c3 = colors[vIndex + 3]; c3.a = a;

            colors[vIndex + 0] = c0;
            colors[vIndex + 1] = c1;
            colors[vIndex + 2] = c2;
            colors[vIndex + 3] = c3;
        }

        // применяем на сабмешах
        for (int m = 0; m < ti.meshInfo.Length; m++)
        {
            var mi = ti.meshInfo[m];
            mi.mesh.colors32 = mi.colors32;
            text.UpdateGeometry(mi.mesh, m);
        }
    }

    float GetStopYInTextLocal()
    {
        if (stopLine == null) return fadeStopYText;

        // мировая позиция stopLine → в локальные координаты ТЕКСТА
        Vector3 world = stopLine.position;
        Vector3 localToText = text.rectTransform.InverseTransformPoint(world);
        return localToText.y;
    }
}
