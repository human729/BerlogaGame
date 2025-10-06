using UnityEngine;

public class EventManagerFirstScene : MonoBehaviour, ICheckConditions
{
    public GameObject Door;
    public void CheckConditions()
    {
        Color color;
        Door.GetComponent<BoxCollider2D>().enabled = false;
        ColorUtility.TryParseHtmlString("#7B97A7", out color);
        Door.GetComponent<SpriteRenderer>().color = color;
    }
}
