using System.Collections.Generic;
using UnityEngine;

public class FlowManager : MonoBehaviour
{
    public GridManager grid;
    public Vector2Int startPos;
    public Vector2Int endPos;

    private HashSet<Pipe> visited = new HashSet<Pipe>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool success = CheckFlow();
            Debug.Log(success ? "<color=green Поток дошёл до цели!</color>" : "<color=red> Поток не завершён!</color>");
        }
    }

    public bool CheckFlow()
    {
        visited.Clear();
        return Flow(startPos.x, startPos.y, Vector2Int.zero);
    }

    public bool CheckFlowSimulate()
    {
        visited.Clear();
        return Flow(startPos.x, startPos.y, Vector2Int.zero);
    }

    bool Flow(int x, int y, Vector2Int fromDir)
    {
        Pipe p = grid.GetPipe(x, y);
        if (p == null || visited.Contains(p)) return false;

        visited.Add(p);

        if (x == endPos.x && y == endPos.y) return true;

        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        foreach (var d in dirs)
        {
            if (d == -fromDir) continue;

            Pipe next = grid.GetPipe(x + d.x, y + d.y);
            if (next == null) continue;

            if (p.Has(d) && next.Has(-d))
            {
                if (Flow(x + d.x, y + d.y, d)) return true;
            }
        }

        return false;
    }
}
