using System.Collections.Generic;
using UnityEngine;

public class FlowManager : MonoBehaviour
{
    public GridManager grid;
    public Vector2Int startPos;
    public Vector2Int endPos;

    private HashSet<Pipe> visited = new HashSet<Pipe>();

    public event System.Action OnFlowSuccess;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckFlow();
        }
    }

    public void CheckFlow()
    {
        if (grid == null)
        {
            Debug.LogWarning("Grid not assigned!");
            return;
        }

        visited.Clear();
        bool success = FlowFrom(startPos.x, startPos.y, null);

        if (success)
        {
            Debug.Log("Flow reached finish!");
            OnFlowSuccess?.Invoke();
        }
        else
        {
            Debug.Log("Connection broken.");
        }
    }

    bool FlowFrom(int x, int y, Pipe fromPipe)
    {
        Pipe current = grid.GetPipe(x, y);
        if (current == null || visited.Contains(current))
            return false;

        visited.Add(current);

        if (x == endPos.x && y == endPos.y)
            return true;
        if (current.up && CanFlowTo(x, y + 1, "down")) return true;
        if (current.down && CanFlowTo(x, y - 1, "up")) return true;
        if (current.left && CanFlowTo(x - 1, y, "right")) return true;
        if (current.right && CanFlowTo(x + 1, y, "left")) return true;

        return false;
    }

    bool CanFlowTo(int x, int y, string requiredDirection)
    {
        Pipe neighbor = grid.GetPipe(x, y);
        if (neighbor == null) return false;

        return requiredDirection switch
        {
            "up" => neighbor.up,
            "down" => neighbor.down,
            "left" => neighbor.left,
            "right" => neighbor.right,
            _ => false
        };
    }
}