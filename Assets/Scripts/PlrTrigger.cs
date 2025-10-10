using System.Collections;
using UnityEngine;

public class PlrTrigger : MonoBehaviour
{
    private bool inDeadZone = false;
    private bool isOnTriggerLampZone = false;
    private bool isOnTriggerTempControl = false;
    private bool isOnTriggerWorkplace = false;
    public GameObject StoryObject;
    [SerializeField] private GameObject spawnZone;
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
        if (inDeadZone)
        {
            StartCoroutine(Spawn());
        }
        if (!StoryObject.activeInHierarchy)
        {
            characterController.canMove = false;
        }
        else if (!TempControlGameObject.activeInHierarchy && !ColorControlGameObject.activeInHierarchy)
        {
            characterController.canMove = true;
        }
        else if (TempControlGameObject.activeInHierarchy || ColorControlGameObject.activeInHierarchy)
        {
            characterController.canMove = false;
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

        if (collision.CompareTag("DeadZone"))
            inDeadZone = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Workplace"))
            isOnTriggerWorkplace = false;

        if (collision.CompareTag("LampZone"))
            isOnTriggerLampZone = false;

        if (collision.CompareTag("TempControl"))
            isOnTriggerTempControl = false;

        if (collision.CompareTag("DeadZone"))
            inDeadZone = true;
    }

    private void TurnOffMovement()
    {
        characterController.canMove = false;
    }

    IEnumerator Spawn()
    {
        gameObject.GetComponent<CharacterController>().enabled = false;
        yield return new WaitForSeconds(.3f);
        transform.position = spawnZone.transform.position;
        gameObject.GetComponent<CharacterController>().enabled = true;
    }
}
