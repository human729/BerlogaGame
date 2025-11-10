using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 6;
    public int height = 6;
    public float cellSize = 80f;

    [Header("UI Parent")]
    public RectTransform uiParent;

    [Header("Prefabs")]
    public GameObject startPrefab;
    public GameObject finishPrefab;
    public GameObject straightPrefab;
    public GameObject cornerPrefab;
    public GameObject obstaclePrefab;

    [HideInInspector] public Pipe[,] grid;
    [HideInInspector] public List<Vector2Int> path;

    void OnEnable()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        bool valid = false;
        int maxAttempts = 100;
        int attempt = 0;

        while (!valid && attempt < maxAttempts)
        {
            attempt++;
            ClearOldPipes();
            grid = new Pipe[width, height];
            path = GeneratePath();

            if (path == null || path.Count < 2)
                continue;

            FillPathPipes();
            FillNonPathCells();
            CenterGrid();

            FlowManager fm = FindObjectOfType<FlowManager>();
            if (fm != null)
            {
                fm.grid = this;
                fm.startPos = path[0];
                fm.endPos = path[path.Count - 1];
                valid = fm.CheckFlowSimulate();
            }
            else
            {
                valid = true; // если FlowManager отсутствует Ч считаем путь валидным
            }
        }

        if (!valid)
            Debug.LogError("Ќе удалось сгенерировать проходимый уровень после 100 попыток!");
    }

    void ClearOldPipes()
    {
        if (uiParent == null) return;
        for (int i = uiParent.childCount - 1; i >= 0; i--)
            DestroyImmediate(uiParent.GetChild(i).gameObject);
    }

    List<Vector2Int> GeneratePath()
    {
        List<Vector2Int> result = new List<Vector2Int>();
        Vector2Int cur = new Vector2Int(0, Random.Range(0, height));
        result.Add(cur);

        while (cur.x < width - 1)
        {
            List<Vector2Int> next = new List<Vector2Int>();
            Vector2Int[] dirs = { Vector2Int.right, Vector2Int.up, Vector2Int.down };

            foreach (var d in dirs)
            {
                Vector2Int nx = cur + d;
                if (nx.x >= 0 && nx.x < width && nx.y >= 0 && nx.y < height && !result.Contains(nx))
                    next.Add(nx);
            }

            if (next.Count == 0) return null;

            // ѕриоритет вправо
            if (next.Contains(cur + Vector2Int.right) && Random.value < 0.8f)
                cur = cur + Vector2Int.right;
            else
                cur = next[Random.Range(0, next.Count)];

            result.Add(cur);
        }

        return result;
    }

    void SpawnPipe(GameObject prefab, Vector2Int pos, float rot, bool up, bool down, bool left, bool right)
    {
        GameObject obj = Instantiate(prefab, uiParent);
        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(cellSize, cellSize);
        rt.anchoredPosition = new Vector2(pos.x * cellSize, -pos.y * cellSize);
        rt.localRotation = Quaternion.Euler(0, 0, rot);

        Pipe pipe = obj.GetComponent<Pipe>();
        if (pipe != null)
            pipe.InitializeDirections(up, down, left, right);

        grid[pos.x, pos.y] = pipe;
    }

    void FillPathPipes()
    {
        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int cur = path[i];

            if (i == 0)
            {
                Vector2Int dir = path[i + 1] - cur;
                SpawnPipe(startPrefab, cur, DirectionToRotation(dir), dir == Vector2Int.up, dir == Vector2Int.down, dir == Vector2Int.left, dir == Vector2Int.right);
                continue;
            }

            if (i == path.Count - 1)
            {
                Vector2Int dir = path[i - 1] - cur;
                SpawnPipe(finishPrefab, cur, DirectionToRotation(-dir), -dir == Vector2Int.up, -dir == Vector2Int.down, -dir == Vector2Int.left, -dir == Vector2Int.right);
                continue;
            }

            Vector2Int prev = path[i - 1];
            Vector2Int next = path[i + 1];
            Vector2Int inDir = cur - prev;
            Vector2Int outDir = next - cur;

            if (inDir == outDir || inDir == -outDir)
            {
                // straight
                bool vertical = inDir.y != 0;
                SpawnPipe(straightPrefab, cur, vertical ? 0 : 90, vertical, vertical, !vertical, !vertical);
            }
            else
            {
                bool up = inDir == Vector2Int.up || outDir == Vector2Int.up;
                bool down = inDir == Vector2Int.down || outDir == Vector2Int.down;
                bool left = inDir == Vector2Int.left || outDir == Vector2Int.left;
                bool right = inDir == Vector2Int.right || outDir == Vector2Int.right;
                float rot = CornerRotation(up, down, left, right);
                SpawnPipe(cornerPrefab, cur, rot, up, down, left, right);
            }
        }
    }

    void FillNonPathCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (path.Contains(pos)) continue;

                if (Random.value < 0.3f && obstaclePrefab != null)
                {
                    SpawnPipe(obstaclePrefab, pos, 0, false, false, false, false);
                    continue;
                }

                bool up = Random.value > 0.5f;
                bool down = Random.value > 0.5f;
                bool left = Random.value > 0.5f;
                bool right = Random.value > 0.5f;
                GameObject prefab = Random.value < 0.5f ? straightPrefab : cornerPrefab;
                float rot = Random.Range(0, 4) * 90;
                SpawnPipe(prefab, pos, rot, up, down, left, right);
            }
        }
    }

    void CenterGrid()
    {
        if (uiParent == null) return;
        float totalW = width * cellSize;
        float totalH = height * cellSize;
        uiParent.anchoredPosition = new Vector2(-totalW / 2 + cellSize / 2, totalH / 2 - cellSize / 2);
    }

    float DirectionToRotation(Vector2Int dir)
    {
        if (dir == Vector2Int.up) return 0;
        if (dir == Vector2Int.right) return 90;
        if (dir == Vector2Int.down) return 180;
        if (dir == Vector2Int.left) return 270;
        return 0;
    }

    float CornerRotation(bool up, bool down, bool left, bool right)
    {
        if (up && right) return 0;
        if (right && down) return 90;
        if (down && left) return 180;
        return 270; // left + up
    }

    public Pipe GetPipe(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height) return null;
        return grid[x, y];
    }
}
