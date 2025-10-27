using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 6;
    public int height = 6;
    [Range(0f, 1f)]
    public float obstacleChance = 0.2f;
    public GameObject startPrefab;
    public GameObject finishPrefab;
    public GameObject straightPrefab;
    public GameObject cornerPrefab;
    public GameObject tShapePrefab;
    public GameObject crossPrefab;
    public GameObject obstaclePrefab;
    public int minPathLength = 8;
    public int maxExtraTurns = 5;
    public bool allowBacktracking = true;
    public bool addAlternativePaths = true;

    [HideInInspector] public Pipe[,] grid;
    private List<Vector2Int> path;

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        grid = new Pipe[width, height];
        int maxAttempts = 20;
        int attempts = 0;

        do
        {
            path = GeneratePath();
            attempts++;
        } while ((path == null || path.Count < minPathLength || !IsPathValid()) && attempts < maxAttempts);

        if (path == null)
        {
            Debug.LogError("Failed to generate valid path!");
            return;
        }

        Debug.Log($"Generated path with {path.Count} segments and {CountTurns(path)} turns");
        FillPathPipes();

        FillNonPathCellsWithDistractions();

        SetupFlowManager();
    }

    List<Vector2Int> GeneratePath()
    {
        List<Vector2Int> path = new List<Vector2Int>();

        Vector2Int start = new Vector2Int(0, Random.Range(1, height - 1));
        path.Add(start);

        Vector2Int current = start;
        int totalAttempts = 0;
        int maxTotalAttempts = width * height * 4;
        int turnsMade = 0;
        int maxTurns = maxExtraTurns + (width / 2);

        while ((current.x < width - 1 || path.Count < minPathLength) && totalAttempts < maxTotalAttempts)
        {
            totalAttempts++;

            List<Vector2Int> possibleMoves = GetSmartMoves(current, path, turnsMade, maxTurns);

            if (possibleMoves.Count == 0)
            {
                if (path.Count > 3)
                {
                    int backSteps = Random.Range(1, Mathf.Min(4, path.Count - 1));
                    for (int i = 0; i < backSteps; i++)
                    {
                        path.RemoveAt(path.Count - 1);
                    }
                    current = path[^1];
                    turnsMade = Mathf.Max(0, turnsMade - backSteps);
                    continue;
                }
                else
                {
                    return null;
                }
            }

            Vector2Int nextMove = ChooseMove(current, possibleMoves, path, turnsMade, maxTurns);

            if (path.Count >= 2)
            {
                Vector2Int prevDir = current - path[^2];
                Vector2Int nextDir = nextMove - current;
                if (prevDir != nextDir)
                {
                    turnsMade++;
                }
            }

            current = nextMove;
            path.Add(current);

            if (allowBacktracking && path.Count > 5 && Random.value < 0.15f)
            {
                if (TryAddSmallLoop(path, ref current))
                {
                    turnsMade += 2;
                }
            }
        }

        if (path[^1].x != width - 1)
        {
            while (current.x < width - 1 && totalAttempts < maxTotalAttempts * 2)
            {
                totalAttempts++;
                Vector2Int rightMove = new Vector2Int(current.x + 1, current.y);
                if (IsPositionValid(rightMove, path))
                {
                    current = rightMove;
                    path.Add(current);
                }
                else
                {
                    break;
                }
            }
        }

        return path.Count >= minPathLength ? path : null;
    }

    List<Vector2Int> GetSmartMoves(Vector2Int current, List<Vector2Int> path, int turnsMade, int maxTurns)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        Vector2Int[] allDirections = {
            Vector2Int.right, Vector2Int.up, Vector2Int.down, Vector2Int.left
        };

        foreach (var dir in allDirections)
        {
            Vector2Int next = current + dir;
            if (IsPositionValid(next, path))
            {
                if (!WouldCreateDeadEnd(next, path))
                {
                    moves.Add(next);
                }
            }
        }

        return moves;
    }

    bool WouldCreateDeadEnd(Vector2Int pos, List<Vector2Int> path)
    {
        List<Vector2Int> tempPath = new List<Vector2Int>(path) { pos };
        int possibleExits = 0;
        Vector2Int[] directions = { Vector2Int.right, Vector2Int.up, Vector2Int.down, Vector2Int.left };

        foreach (var dir in directions)
        {
            Vector2Int next = pos + dir;
            if (IsPositionValid(next, tempPath))
            {
                possibleExits++;
            }
        }

        return possibleExits == 0;
    }

    Vector2Int ChooseMove(Vector2Int current, List<Vector2Int> possibleMoves, List<Vector2Int> path, int turnsMade, int maxTurns)
    {
        float progress = (float)current.x / (width - 1);
        bool needMoreTurns = turnsMade < maxTurns * 0.7f;
        bool nearEnd = progress > 0.7f;

        List<Vector2Int> preferredMoves = new List<Vector2Int>();
        List<Vector2Int> goodMoves = new List<Vector2Int>();
        List<Vector2Int> acceptableMoves = new List<Vector2Int>();

        foreach (var move in possibleMoves)
        {
            Vector2Int direction = move - current;
            bool isTurn = path.Count >= 2 && (current - path[^2]) != direction;

            if (nearEnd && direction == Vector2Int.right)
            {
                preferredMoves.Add(move); 
            }
            else if (needMoreTurns && isTurn)
            {
                preferredMoves.Add(move); 
            }
            else if (direction == Vector2Int.right)
            {
                goodMoves.Add(move);
            }
            else if (Mathf.Abs(direction.y) == 1)
            {
                acceptableMoves.Add(move);
            }
            else
            {
                acceptableMoves.Add(move);
            }
        }

        if (preferredMoves.Count > 0) return preferredMoves[Random.Range(0, preferredMoves.Count)];
        if (goodMoves.Count > 0) return goodMoves[Random.Range(0, goodMoves.Count)];
        return acceptableMoves[Random.Range(0, acceptableMoves.Count)];
    }

    bool TryAddSmallLoop(List<Vector2Int> path, ref Vector2Int current)
    {
        if (path.Count < 3) return false;
        Vector2Int[] loopPatterns = {
            Vector2Int.down, Vector2Int.right, Vector2Int.up,
            Vector2Int.up, Vector2Int.right, Vector2Int.down
        };

        for (int i = 0; i < loopPatterns.Length; i += 3)
        {
            Vector2Int step1 = current + loopPatterns[i];
            Vector2Int step2 = step1 + loopPatterns[i + 1];
            Vector2Int step3 = step2 + loopPatterns[i + 2];

            if (IsPositionValid(step1, path) &&
                IsPositionValid(step2, path) &&
                IsPositionValid(step3, path))
            {
                path.Add(step1);
                path.Add(step2);
                path.Add(step3);
                current = step3;
                return true;
            }
        }

        return false;
    }

    int CountTurns(List<Vector2Int> path)
    {
        int turns = 0;
        for (int i = 2; i < path.Count; i++)
        {
            Vector2Int dir1 = path[i - 1] - path[i - 2];
            Vector2Int dir2 = path[i] - path[i - 1];
            if (dir1 != dir2) turns++;
        }
        return turns;
    }

    bool IsPositionValid(Vector2Int pos, List<Vector2Int> path)
    {
        return pos.x >= 0 && pos.x < width &&
               pos.y >= 0 && pos.y < height &&
               !path.Contains(pos);
    }

    bool IsPathValid()
    {
        if (path[0].x != 0 || path[^1].x != width - 1)
            return false;
        int turns = CountTurns(path);
        if (turns < 3) return false;

        return true;
    }

    void FillPathPipes()
    {
        for (int i = 0; i < path.Count; i++)
        {
            Vector2Int current = path[i];
            Vector3 pos = new Vector3(current.x, current.y, 0);

            if (i == 0) 
            {
                GameObject start = Instantiate(startPrefab, pos, Quaternion.identity);
                Pipe startPipe = start.GetComponent<Pipe>();
                startPipe.type = Pipe.PipeType.Start;

                Vector2Int firstStep = path[1] - path[0];
                bool right = firstStep == Vector2Int.right;
                bool up = firstStep == Vector2Int.up;
                bool down = firstStep == Vector2Int.down;
                bool left = firstStep == Vector2Int.left;

                startPipe.InitializeDirections(up, down, left, right);
                grid[current.x, current.y] = startPipe;
            }
            else if (i == path.Count - 1) 
            {
                GameObject finish = Instantiate(finishPrefab, pos, Quaternion.identity);
                Pipe finishPipe = finish.GetComponent<Pipe>();
                finishPipe.type = Pipe.PipeType.Finish;

                Vector2Int lastStep = path[^1] - path[^2];
                bool right = lastStep == Vector2Int.left;
                bool up = lastStep == Vector2Int.down;
                bool down = lastStep == Vector2Int.up;
                bool left = lastStep == Vector2Int.right;

                finishPipe.InitializeDirections(up, down, left, right);
                grid[current.x, current.y] = finishPipe;
            }
            else 
            {
                GameObject prefab = GetPathPipePrefab(current, out float rotation);
                GameObject obj = Instantiate(prefab, pos, Quaternion.Euler(0, 0, rotation));
                Pipe pipe = obj.GetComponent<Pipe>();
                grid[current.x, current.y] = pipe;
            }
        }
    }

    void FillNonPathCellsWithDistractions()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int current = new Vector2Int(x, y);
                Vector3 pos = new Vector3(x, y, 0);

                if (!path.Contains(current))
                {
                    if (Random.value > obstacleChance)
                    {
                        GameObject prefab = GetStrategicPipePrefab(x, y, out float rotation);
                        GameObject obj = Instantiate(prefab, pos, Quaternion.Euler(0, 0, rotation));
                        Pipe pipe = obj.GetComponent<Pipe>();
                        grid[x, y] = pipe;
                    }
                    else
                    {
                        Instantiate(obstaclePrefab, pos, Quaternion.identity);
                        grid[x, y] = null;
                    }
                }
            }
        }
    }

    GameObject GetStrategicPipePrefab(int x, int y, out float rotation)
    {
        bool nearPath = IsNearPath(x, y);
        bool nearStartOrEnd = IsNearStartOrEnd(x, y);

        float rand = Random.value;
        rotation = Random.Range(0, 4) * 90f;

        if (nearStartOrEnd)
        {
            return straightPrefab;
        }
        else if (nearPath)
        {
            if (rand < 0.3f) return tShapePrefab;
            else if (rand < 0.6f) return cornerPrefab;
            else return straightPrefab;
        }
        else
        {
            if (rand < 0.4f) return tShapePrefab;
            else if (rand < 0.6f) return crossPrefab;
            else if (rand < 0.8f) return cornerPrefab;
            else return straightPrefab;
        }
    }

    bool IsNearPath(int x, int y)
    {
        foreach (var pathCell in path)
        {
            if (Mathf.Abs(x - pathCell.x) <= 1 && Mathf.Abs(y - pathCell.y) <= 1)
                return true;
        }
        return false;
    }

    bool IsNearStartOrEnd(int x, int y)
    {
        return (Mathf.Abs(x - path[0].x) <= 1 && Mathf.Abs(y - path[0].y) <= 1) ||
               (Mathf.Abs(x - path[^1].x) <= 1 && Mathf.Abs(y - path[^1].y) <= 1);
    }

    GameObject GetPathPipePrefab(Vector2Int current, out float rotation)
    {
        int index = path.IndexOf(current);
        Vector2Int prev = path[index - 1];
        Vector2Int next = path[index + 1];

        Vector2Int inDir = current - prev;
        Vector2Int outDir = next - current;

        rotation = 0f;
        if (inDir == outDir || inDir == -outDir)
        {
            rotation = (inDir.x != 0) ? 90f : 0f;
            return straightPrefab;
        }
        else
        {
            if ((inDir == Vector2Int.up && outDir == Vector2Int.right) || (inDir == Vector2Int.right && outDir == Vector2Int.up))
                rotation = 0;
            else if ((inDir == Vector2Int.right && outDir == Vector2Int.down) || (inDir == Vector2Int.down && outDir == Vector2Int.right))
                rotation = 270;
            else if ((inDir == Vector2Int.down && outDir == Vector2Int.left) || (inDir == Vector2Int.left && outDir == Vector2Int.down))
                rotation = 180;
            else
                rotation = 90;

            return cornerPrefab;
        }
    }

    void SetupFlowManager()
    {
        FlowManager flow = FindObjectOfType<FlowManager>();
        if (flow != null)
        {
            flow.grid = this;
            flow.startPos = path[0];
            flow.endPos = path[^1];
        }
    }

    public Pipe GetPipe(int x, int y)
    {
        if (x < 0 || y < 0 || x >= width || y >= height)
            return null;
        return grid[x, y];
    }
}