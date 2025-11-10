using UnityEngine;
using UnityEngine.EventSystems;

public class Pipe : MonoBehaviour, IPointerClickHandler
{
    public bool up;
    public bool down;
    public bool left;
    public bool right;

    [SerializeField] bool baseUp, baseDown, baseLeft, baseRight;

    RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void InitializeDirections(bool u, bool d, bool l, bool r)
    {
        baseUp = u; baseDown = d; baseLeft = l; baseRight = r;
        transform.localRotation = Quaternion.identity;
        UpdateConnections();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.Rotate(0, 0, -90);
        UpdateConnections();
    }

    public void UpdateConnections()
    {
        up = baseUp; down = baseDown; left = baseLeft; right = baseRight;

        int rot = Mathf.RoundToInt(transform.eulerAngles.z) % 360;
        if (rot < 0) rot += 360;

        if (rot == 90)
        {
            bool t = up; up = left; left = down; down = right; right = t;
        }
        else if (rot == 180)
        {
            bool t = up; up = down; down = t;
            t = left; left = right; right = t;
        }
        else if (rot == 270)
        {
            bool t = up; up = right; right = down; down = left; left = t;
        }
    }

    public bool Has(Vector2Int dir)
    {
        if (dir == Vector2Int.up) return up;
        if (dir == Vector2Int.down) return down;
        if (dir == Vector2Int.left) return left;
        if (dir == Vector2Int.right) return right;
        return false;
    }
}
