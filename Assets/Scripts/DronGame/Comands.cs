using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comands : MonoBehaviour
{
    GameObject dron;
    GameObject player;
    [SerializeField] GameObject interf;
    public List<string> move = new List<string>();
    private Coroutine currentExecution;
    [SerializeField] float speed;
    public LayerMask obstacleLayer;

    private bool isGameStart = false;
    private GameObject phantomDron;
    private SpriteRenderer phantomRenderer;
    private bool isPathDirty = true;

    private void Start()
    {
        dron = GameObject.Find("Dron");
        player = GameObject.Find("Player");
        CreatePhantomDron();
        UpdatePhantomPosition();
    }

    void Update()
    {
        if (!isGameStart)
        {
            if (Input.GetKeyDown(KeyCode.Q) && currentExecution == null)
            {
                isGameStart = true;
                Destroy(phantomDron);
                player.GetComponent<CharacterController>().enabled = true;
                currentExecution = StartCoroutine(ExecuteMoves());
                if (transform.name == "Dron")
                {
                    interf.GetComponent<InterfaceController>().isGameStart = true;
                }
            }
        }
        if (isPathDirty)
        {
            UpdatePhantomPosition();
            isPathDirty = false;
        }
    }

    void CreatePhantomDron()
    {
        phantomDron = new GameObject("PhantomDron");
        phantomDron.transform.SetParent(transform);

        SpriteRenderer originalRenderer = dron.GetComponent<SpriteRenderer>();
        if (originalRenderer != null)
        {
            phantomRenderer = phantomDron.AddComponent<SpriteRenderer>();
            phantomRenderer.sprite = originalRenderer.sprite;
            phantomRenderer.color = new Color(0.5f, 0.5f, 1f, 0.7f);
            phantomRenderer.sortingOrder = originalRenderer.sortingOrder - 1;
        }

        Collider2D originalCollider = dron.GetComponent<Collider2D>();
        if (originalCollider != null)
        {
            if (originalCollider is BoxCollider2D)
            {
                BoxCollider2D phantomCollider = phantomDron.AddComponent<BoxCollider2D>();
                phantomCollider.size = ((BoxCollider2D)originalCollider).size;
            }
            else if (originalCollider is CircleCollider2D)
            {
                CircleCollider2D phantomCollider = phantomDron.AddComponent<CircleCollider2D>();
                phantomCollider.radius = ((CircleCollider2D)originalCollider).radius;
            }
            phantomDron.GetComponent<Collider2D>().isTrigger = true;
        }
    }

    void UpdatePhantomPosition()
    {
        if (phantomDron == null) return;


        Vector3 simulationPosition = dron.transform.position;


        foreach (string command in move)
        {
            Vector3 direction = Vector3.zero;
            float moveDistance = 1f;

            switch (command)
            {
                case "ÂÂÅÐÕ":
                    direction = Vector3.up;
                    break;
                case "ÂÍÈÇ":
                    direction = Vector3.down;
                    break;
                case "ÂÏÐÀÂÎ":
                    direction = Vector3.right;
                    break;
                case "ÂËÅÂÎ":
                    direction = Vector3.left;
                    break;
                case "ÏÀÓÇÀ":
                    continue;
            }

            if (direction != Vector3.zero)
            {
                if (!CheckForObstacle(simulationPosition, direction, moveDistance))
                {
                    simulationPosition += direction * moveDistance;
                }
                else
                {
                    float safeDistance = GetSafeDistance(simulationPosition, direction, moveDistance);
                    simulationPosition += direction * safeDistance;
                }
            }
        }

        phantomDron.transform.position = simulationPosition;


        phantomRenderer.enabled = move.Count > 0;
    }


    bool CheckForObstacle(Vector3 fromPosition, Vector3 direction, float distance)
    {
        Collider2D dronCollider = dron.GetComponent<Collider2D>();
        if (dronCollider == null) return false;

        RaycastHit2D hit = Physics2D.BoxCast(
            fromPosition,
            dronCollider.bounds.size * 0.9f,
            0f,
            direction,
            distance,
            obstacleLayer
        );

        return hit.collider != null;
    }

    float GetSafeDistance(Vector3 fromPosition, Vector3 direction, float maxDistance)
    {
        Collider2D dronCollider = dron.GetComponent<Collider2D>();
        if (dronCollider == null) return maxDistance;

        RaycastHit2D hit = Physics2D.BoxCast(
            fromPosition,
            dronCollider.bounds.size * 0.9f,
            0f,
            direction,
            maxDistance,
            obstacleLayer
        );

        if (hit.collider != null)
        {
            return Mathf.Max(0, hit.distance - 0.05f);
        }

        return maxDistance;
    }

    public void MarkPathDirty()
    {
        isPathDirty = true;
    }

    IEnumerator ExecuteMoves()
    {
        if (phantomRenderer != null)
            phantomRenderer.enabled = false;

        for (int i = 0; i < move.Count; i++)
        {
            Vector2 targetPosition = dron.transform.position;
            Vector2 direction = Vector2.zero;
            float moveDistance = 1f;

            if (move[i] == "ÂÂÅÐÕ")
            {
                targetPosition = new Vector2(dron.transform.position.x, dron.transform.position.y + moveDistance);
                direction = Vector2.up;
            }
            else if (move[i] == "ÂÍÈÇ")
            {
                targetPosition = new Vector2(dron.transform.position.x, dron.transform.position.y - moveDistance);
                direction = Vector2.down;
            }
            else if (move[i] == "ÂÏÐÀÂÎ")
            {
                targetPosition = new Vector2(dron.transform.position.x + moveDistance, dron.transform.position.y);
                direction = Vector2.right;
            }
            else if (move[i] == "ÂËÅÂÎ")
            {
                targetPosition = new Vector2(dron.transform.position.x - moveDistance, dron.transform.position.y);
                direction = Vector2.left;
            }
            else if (move[i] == "ÏÀÓÇÀ")
            {
                yield return new WaitForSeconds(1f);
                continue;
            }

            if (direction != Vector2.zero)
            {
                Collider2D dronCollider = dron.GetComponent<Collider2D>();

                if (dronCollider != null)
                {
                    if (move[i] == "ÂÍÈÇ")
                    {
                        float raycastDistance = moveDistance + dronCollider.bounds.extents.y;
                        Vector2 rayStart = (Vector2)dron.transform.position - new Vector2(0, dronCollider.bounds.extents.y);

                        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, raycastDistance, obstacleLayer);

                        if (hit.collider != null)
                        {
                            float newY = hit.point.y + dronCollider.bounds.extents.y + 0.01f;
                            targetPosition = new Vector2(targetPosition.x, newY);
                        }
                    }
                    else
                    {
                        RaycastHit2D hit = Physics2D.BoxCast(
                            dron.transform.position,
                            dronCollider.bounds.size * 0.9f,
                            0f,
                            direction,
                            moveDistance,
                            obstacleLayer
                        );

                        if (hit.collider != null)
                        {
                            float safeDistance = hit.distance - 0.05f;
                            targetPosition = (Vector2)dron.transform.position + direction * safeDistance;
                        }
                    }
                }
            }

            float journey = 0f;
            Vector2 startPosition = dron.transform.position;

            while (journey <= 1f)
            {
                journey += Time.deltaTime * speed;
                dron.transform.position = Vector2.Lerp(startPosition, targetPosition, journey);
                yield return null;
            }
        }

        move.Clear();
        currentExecution = null;

        MarkPathDirty();
    }
}