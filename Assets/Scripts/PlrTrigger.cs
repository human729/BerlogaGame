using UnityEngine;

public class PlrTrigger : MonoBehaviour
{
    private bool isOnTriggerLampZone = false;
    private bool isOnTriggerTempControl = false;
    private bool isOnTriggerWorkplace = false;

    [SerializeField] private GameObject TempControlGameObject;
    [SerializeField] private GameObject ColorControlGameObject;
    [SerializeField] private GameObject computerCanvas;

    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!TempControlGameObject.activeInHierarchy && !ColorControlGameObject.activeInHierarchy)
        {
            characterController.enabled = true;
        }
        else if (TempControlGameObject.activeInHierarchy && ColorControlGameObject.activeInHierarchy)
        {
            characterController.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isOnTriggerWorkplace)
            {
                TurnOffMovement();
            }
            else if (isOnTriggerLampZone)
            {
                ColorControlGameObject.SetActive(true);
                TurnOffMovement();
            }
            else if (isOnTriggerTempControl)
            {
                TempControlGameObject.SetActive(true);
                TurnOffMovement();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Workplace"))
            isOnTriggerWorkplace = true;

        if (collision.CompareTag("LampZone"))
            isOnTriggerLampZone = true;

        if (collision.CompareTag("TempControl"))
            isOnTriggerTempControl = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Workplace"))
            isOnTriggerWorkplace = false;

        if (collision.CompareTag("LampZone"))
            isOnTriggerLampZone = false;

        if (collision.CompareTag("TempControl"))
            isOnTriggerTempControl = false;
    }

    private void TurnOffMovement()
    {
        characterController.enabled = false;
    }
}
