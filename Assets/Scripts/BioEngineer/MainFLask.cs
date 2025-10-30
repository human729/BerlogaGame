using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class MainFlask : MonoBehaviour
{
    [Header("Flask Layers")]
    public List<Image> flaskLayers;

    [Header("Mixture Rules")]
    public List<MixtureRule> mixtureRules;

    private Stack<FlaskElement> currentElements = new Stack<FlaskElement>();

    [System.Serializable]
    public class MixtureRule
    {
        public string element1;
        public string element2;
        public string resultElement;
        public Color resultColor;
    }

    [System.Serializable]
    public class FlaskElement
    {
        public string name;
        public Color color;

        public FlaskElement(string elementName, Color elementColor)
        {
            name = elementName;
            color = elementColor;
        }
    }

    public void AddElement(string elementName, Color elementColor)
    {
        if (currentElements.Count >= flaskLayers.Count)
        {
            Debug.Log("Flask is full!");
            return;
        }

        currentElements.Push(new FlaskElement(elementName, elementColor));
        UpdateFlaskDisplay();
    }

    public void MixElements()
    {
        if (currentElements.Count < 2)
        {
            Debug.Log("Need at least 2 elements to mix!");
            return;
        }

        // ����� ��� ������� ��������
        var element1 = currentElements.Pop();
        var element2 = currentElements.Pop();

        // ���� ������� ����������
        var mixture = FindMixture(element1.name, element2.name);

        if (mixture != null)
        {
            // ������� ����� �����
            currentElements.Push(new FlaskElement(mixture.resultElement, mixture.resultColor));
            Debug.Log($"Created mixture: {mixture.resultElement}");
        }
        else
        {
            // ���� ���������� ����������, ���������� �������� �������
            currentElements.Push(element2);
            currentElements.Push(element1);
            Debug.Log("No mixture possible with these elements");
        }

        UpdateFlaskDisplay();
    }

    private MixtureRule FindMixture(string elem1, string elem2)
    {
        foreach (var rule in mixtureRules)
        {
            if ((rule.element1 == elem1 && rule.element2 == elem2) ||
                (rule.element1 == elem2 && rule.element2 == elem1))
            {
                return rule;
            }
        }
        return null;
    }

    public void ClearFlask()
    {
        currentElements.Clear();
        UpdateFlaskDisplay();
    }

    private void UpdateFlaskDisplay()
    {
        // ������� ��� ����
        foreach (var layer in flaskLayers)
        {
            layer.color = Color.white;
        }

        // ��������� ���� �������� ���������� (����� �����)
        var elementsArray = currentElements.ToArray();

        for (int i = 0; i < elementsArray.Length && i < flaskLayers.Count; i++)
        {
            // elementsArray ���� ������ ����, ��� ����� ����� �����
            int layerIndex = flaskLayers.Count - 1 - i;
            flaskLayers[layerIndex].color = elementsArray[i].color;
        }
    }
}