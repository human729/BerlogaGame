using System.Collections;
using UnityEngine;

public class PlrTrigger : MonoBehaviour
{
    [System.Serializable]
    public class MiniGame
    {
        public string tag;
        public GameObject gameObject;
        public bool isActive = false;
    }

    [SerializeField] private MiniGame[] miniGames;
    [SerializeField] private GameObject spawnZone;

    private CharacterController characterController;
    private string currentTriggerTag = "";
    private bool inDeadZone = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (inDeadZone)
        {
            StartCoroutine(Spawn());
            return;
        }

        bool anyGameActive = false;
        foreach (var game in miniGames)
        {
            if (game.gameObject != null && game.gameObject.activeInHierarchy)
            {
                anyGameActive = true;
                break;
            }
        }

        characterController.canMove = !anyGameActive;
        if (Input.GetKeyDown(KeyCode.E) && !string.IsNullOrEmpty(currentTriggerTag))
        {
            TryActivateMiniGame(currentTriggerTag);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var game in miniGames)
        {
            if (collision.CompareTag(game.tag))
            {
                currentTriggerTag = game.tag;
                return;
            }
        }

        if (collision.CompareTag("DeadZone"))
        {
            inDeadZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (var game in miniGames)
        {
            if (collision.CompareTag(game.tag))
            {
                currentTriggerTag = "";
                return;
            }
        }

        if (collision.CompareTag("DeadZone"))
        {
            inDeadZone = false;
        }
    }

    private void TryActivateMiniGame(string tag)
    {
        foreach (var game in miniGames)
        {
            if (game.tag == tag && game.gameObject != null)
            {
                game.gameObject.SetActive(true);
                characterController.canMove = false;
                return;
            }
        }
    }

    IEnumerator Spawn()
    {
        characterController.enabled = false;
        yield return new WaitForSeconds(.3f);

        if (spawnZone != null)
            transform.position = spawnZone.transform.position;

        characterController.enabled = true;
        inDeadZone = false;
    }
}