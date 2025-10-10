using UnityEngine;

public class JumpElevator : MonoBehaviour
{
    public bool onElevator = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            onElevator=true;
        }
        
    }
    

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onElevator = false;
        }
    }
}
