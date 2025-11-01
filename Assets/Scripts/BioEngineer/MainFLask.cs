using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class MainFlask : MonoBehaviour
{
    [Header("Flask Layers")]
    public List<Image> flaskLayers;

    [Header("Chemical Reactions")]
    public List<ChemicalReaction> chemicalReactions;

    private List<FlaskElement> currentElements = new List<FlaskElement>();

    [System.Serializable]
    public class ChemicalReaction
    {
        public string reactionName;
        public List<Reactant> reactants;
        public List<Product> products;
        public Color reactionColor;
    }

    [System.Serializable]
    public class Reactant
    {
        public string elementName;
        public int count = 1;
    }

    [System.Serializable]
    public class Product
    {
        public string elementName;
        public Color elementColor;
        public int count = 1;
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
            print("Flask is full!");
            return;
        }

        currentElements.Add(new FlaskElement(elementName, elementColor));
        UpdateFlaskDisplay();
    }

    public void TryMixAllElements()
    {
        if (currentElements.Count < 2)
        {
            print("Need at least 2 elements to mix!");
            return;
        }

        var possibleReactions = FindPossibleReactions();

        if (possibleReactions.Count > 0)
        {
            ExecuteReaction(possibleReactions[0]);
        }
        else
        {
            print("No chemical reactions possible");
        }
    }

    private List<ChemicalReaction> FindPossibleReactions()
    {
        List<ChemicalReaction> possibleReactions = new List<ChemicalReaction>();

        foreach (var reaction in chemicalReactions)
        {
            if (CanExecuteReaction(reaction))
            {
                possibleReactions.Add(reaction);
            }
        }

        return possibleReactions;
    }

    private bool CanExecuteReaction(ChemicalReaction reaction)
    {
        Dictionary<string, int> availableElements = new Dictionary<string, int>();

        foreach (var element in currentElements)
        {
            if (availableElements.ContainsKey(element.name))
                availableElements[element.name]++;
            else
                availableElements[element.name] = 1;
        }

        foreach (var reactant in reaction.reactants)
        {
            if (!availableElements.ContainsKey(reactant.elementName))
                return false;

            if (availableElements[reactant.elementName] < reactant.count)
                return false;
        }

        return true;
    }

    private void ExecuteReaction(ChemicalReaction reaction)
    {
        print($"Executing reaction: {reaction.reactionName}");

        List<FlaskElement> newElements = new List<FlaskElement>();
        List<string> elementsToRemove = new List<string>();

        foreach (var reactant in reaction.reactants)
        {
            for (int i = 0; i < reactant.count; i++)
            {
                elementsToRemove.Add(reactant.elementName);
            }
        }

        foreach (var element in currentElements)
        {
            if (elementsToRemove.Contains(element.name))
            {
                elementsToRemove.Remove(element.name);
                continue;
            }
            newElements.Add(element);
        }

        foreach (var product in reaction.products)
        {
            for (int i = 0; i < product.count; i++)
            {
                newElements.Add(new FlaskElement(product.elementName, product.elementColor));
            }
        }

        currentElements = newElements;
        UpdateFlaskDisplay();

        print($"Reaction completed: {reaction.reactionName}");
        PrintCurrentElements();
    }

    public void ClearFlask()
    {
        currentElements.Clear();
        UpdateFlaskDisplay();
    }

    private void UpdateFlaskDisplay()
    {
        foreach (var layer in flaskLayers)
        {
            layer.color = Color.white;
        }

        for (int i = 0; i < currentElements.Count && i < flaskLayers.Count; i++)
        {
            flaskLayers[i].color = currentElements[i].color;
        }
    }

    public void PrintCurrentElements()
    {
        string elements = "Current elements: ";
        foreach (var element in currentElements)
        {
            elements += $"{element.name}, ";
        }
        print(elements);
    }
}