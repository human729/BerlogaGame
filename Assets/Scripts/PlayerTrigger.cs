using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTrigger : MonoBehaviour
{
    private bool inDeadZone;
    private bool isOpeningScene = false;
    private bool isOnTriggerLeverZone;
    private bool isOnTriggerLampZone;
    private bool isOnTriggerWorkplace;
    private MinigameSQL minigame;
    public GameObject spawnZone;
    [SerializeField] private GameObject computerCanvas;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private string NextScene;
    public GameObject panelOpenScene;
    void Update()
    {
        if (!computerCanvas.activeInHierarchy)
        {
            characterController.enabled = true;
        }
     
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isOnTriggerWorkplace && !computerCanvas.activeInHierarchy)
            {
                computerCanvas.SetActive(true);
                minigame.CreateTask();
                characterController.enabled = false;
            }
            //if (isOnTriggerLeverZone)
            //{
            //    minigame.SucceedQueries++;
            //    print(minigame.SucceedQueries);
            //}
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
        if (collision.CompareTag("LampZone"))
        {
            isOnTriggerLampZone = true;
        }
        if (collision.CompareTag("LeverZone"))
        {
            isOnTriggerLeverZone= true;
        }
        if (collision.CompareTag("DeadZone"))
        {
           inDeadZone = true;
        }
        if (collision.CompareTag("EndTriggerZone") && !isOpeningScene)
        {
            isOpeningScene = true;
            StartCoroutine(OpenScene());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isOnTriggerLampZone = false;
        isOnTriggerWorkplace = false;
        isOnTriggerLeverZone = false;
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

    IEnumerator OpenScene()
    {
        yield return new WaitForSeconds(.7f);
        SceneManager.LoadScene(NextScene);
    }
}
