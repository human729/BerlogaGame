using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimateController : MonoBehaviour
{
    public TextMeshProUGUI textComponent;  // ���� TMP �����
    public float speed = 30f;              // �������� ��������
    public float stopY = 6.144f;             // ���������� Y, ��� ������������
    public Button nextButton;              // ������, ������� ������� ����� ���������

    private RectTransform rect;
    private bool isStopped = false;

    void Start()
    {
        rect = textComponent.GetComponent<RectTransform>();

        // ������ ������ �� ������� ���������
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
            // ������������
            rect.localPosition = new Vector3(rect.localPosition.x, stopY, rect.localPosition.z);
            isStopped = true;

            // �������� ������
            if (nextButton != null)
                nextButton.gameObject.SetActive(true);
        }
    }
}