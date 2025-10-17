using UnityEngine;

public class CheckUIPosition : MonoBehaviour
{
    [SerializeField] string tag;
    public bool isPositionRight = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == tag)
        {
            isPositionRight = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == tag)
        {
            isPositionRight = false;
        }
    }
}
