using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTrigger : MonoBehaviour
{
    private bool inDeadZone;
    private bool isOpeningScene = false;
    private bool isOnTriggerWorkplace;
    private bool isOnTriggerTableZone;
    private MinigameSQL minigame;
    public GameObject spawnZone;
    [SerializeField] private SceneLoader loader;
    [SerializeField] private GameObject computerCanvas;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private string NextScene;
    public GameObject StoryObject;
    void Update()
    {
        if (!computerCanvas.activeInHierarchy && !StoryObject.activeInHierarchy)
        {
            characterController.enabled = true;
        } else
        {
            characterController.enabled = false;
        }
     
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isOnTriggerWorkplace && !computerCanvas.activeInHierarchy)
            {
                computerCanvas.SetActive(true);
                minigame.CreateTask();
            }
            if (isOnTriggerTableZone && !computerCanvas.activeInHierarchy)
            {
                computerCanvas.SetActive(true);
            }
        }

        if (inDeadZone)
        {
            StartCoroutine(Spawn());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Workplace"))
        {
            isOnTriggerWorkplace = true;
            minigame = collision.GetComponent<MinigameSQL>();
        }
        if (collision.CompareTag("BioEngineerTable"))
        {
            isOnTriggerTableZone = true;
        }
        if (collision.CompareTag("DeadZone"))
        {
           inDeadZone = true;
        }
        if (collision.CompareTag("EndTriggerZone") && !isOpeningScene)
        {
            loader.LoadNextScene();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isOnTriggerWorkplace = false;
        isOnTriggerTableZone = false;
        if (inDeadZone)
        {
            StartCoroutine(Spawn());
        }
        inDeadZone = false;
    }

    IEnumerator Spawn()
    {
        gameObject.GetComponent<CharacterController>().enabled = false;
        yield return new WaitForSeconds(.3f);
        transform.position = spawnZone.transform.position;
        gameObject.GetComponent<CharacterController>().enabled = true;
    }
}
