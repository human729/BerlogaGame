using UnityEngine;

public class InterfaceController : MonoBehaviour
{
    public bool isGameStart = false;
    [SerializeField] GameObject buttonsInteface;
    private void Update()
    {
        if (isGameStart)
        {
            buttonsInteface.SetActive(false);
        }
    }
}
