using UnityEngine;

public class LampInteract : MonoBehaviour
{
    [SerializeField] private CanvasScriptLvl3 canvasController;
    public GameObject Lamp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canvasController.SetCurrentLamp(Lamp);
        }
    }
}