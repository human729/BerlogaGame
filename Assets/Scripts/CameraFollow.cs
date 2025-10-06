using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject Target;
    public float CameraSpeed;

    public float minX;
    public float maxX;

    public float minY;
    public float maxY;
    private Vector3 clampedPosition;
    void Update()
    {
        clampedPosition = new Vector3(Mathf.Clamp(Target.transform.position.x, minX, maxX), Mathf.Clamp(Target.transform.position.y, minY, maxY), transform.position.z);
        transform.position = clampedPosition;
    }
}