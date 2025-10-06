using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    public Animator ElevatorAnimator;  
    public GameObject Lamp;
    private Color targetColor;
    private bool isElevatorOpen = false;

    void Update()
    {
        Color currentColor = Lamp.GetComponent<SpriteRenderer>().color;

        if (currentColor == targetColor && !isElevatorOpen)
        {
            SetElevatorOpen(true);
        }
        else if (currentColor != targetColor && isElevatorOpen)
        {
            SetElevatorOpen(false);
        }
    }

    void SetElevatorOpen(bool isOpen)
    {
        if (ElevatorAnimator != null)
        {
            ElevatorAnimator.SetBool("IsOpen", isOpen);
            isElevatorOpen = isOpen;
        }
    }

    public void SetTargetColor(string hexColor)
    {
        if (!ColorUtility.TryParseHtmlString(hexColor, out targetColor))
        {
            Debug.LogError("ElevatorManager: Неверный HEX цвет: " + hexColor);
            enabled = false;
        }
    }
}
