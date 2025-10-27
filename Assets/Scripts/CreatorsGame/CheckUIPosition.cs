using UnityEngine;

public class CheckUIPosition : MonoBehaviour
{
    [SerializeField] string tag;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == tag)
        {
           collision.gameObject.GetComponent<DragSprite>().isRightPosition = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == tag)
        {
            collision.gameObject.GetComponent<DragSprite>().isRightPosition = false;
        }
    }
}
