using UnityEngine;
using System.Collections.Generic;

public class MixingController : MonoBehaviour
{
    public static MixingController Instance;
    public GameObject Door;

    [Header("References")]
    public MainFlask mainFlask;
    public List<ReagentFlask> reagentFlasks;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            mainFlask.TryMixAllElements();
            for (int i = 0; i < mainFlask.currentElements.Count; i++)
            {
                for (int j = 0; j < mainFlask.TargetReaction.products.Count; j++) {
                    if (mainFlask.TargetReaction.products[j].elementName == mainFlask.currentElements[i].name)
                    {
                        Door.GetComponent<BoxCollider2D>().enabled = false;
                        Door.GetComponent<SpriteRenderer>().color = Color.white;
                        break;
                    }
                }
            }
        }
    }

    public void AddReagentToMainFlask(string elementName, Color elementColor)
    {
        mainFlask.AddElement(elementName, elementColor);
    }

    public void ClearMainFlask()
    {
        mainFlask.ClearFlask();
    }
}