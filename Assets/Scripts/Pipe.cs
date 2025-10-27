using UnityEngine;

public class Pipe : MonoBehaviour
{
    public bool up;
    public bool down;
    public bool left;
    public bool right;
    public PipeType type;

    private bool baseUp, baseDown, baseLeft, baseRight;

    public enum PipeType
    {
        Straight,
        Corner,
        TShape,
        Cross,
        Start,
        Finish
    }

    void Start()
    {
        baseUp = up;
        baseDown = down;
        baseLeft = left;
        baseRight = right;
        UpdateConnections();
    }

    void OnMouseDown()
    {
        if (type != PipeType.Start && type != PipeType.Finish)
        {
            transform.Rotate(0, 0, -90);
            UpdateConnections();
        }
    }

    public void UpdateConnections()
    {
        up = down = left = right = false;

        int rot = Mathf.RoundToInt(transform.eulerAngles.z) % 360;

        if (rot == 0)
        {
            up = baseUp; down = baseDown; left = baseLeft; right = baseRight;
        }
        else if (rot == 90)
        {
            up = baseRight; down = baseLeft; left = baseUp; right = baseDown;
        }
        else if (rot == 180)
        {
            up = baseDown; down = baseUp; left = baseRight; right = baseLeft;
        }
        else if (rot == 270)
        {
            up = baseLeft; down = baseRight; left = baseDown; right = baseUp;
        }
    }

    public void InitializeDirections(bool u, bool d, bool l, bool r)
    {
        baseUp = u;
        baseDown = d;
        baseLeft = l;
        baseRight = r;
        UpdateConnections();
    }
}
