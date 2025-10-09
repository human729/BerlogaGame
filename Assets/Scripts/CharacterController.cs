 using UnityEngine;
using UnityEngine.Audio;

public class CharacterController : MonoBehaviour
{
    [SerializeField] public float Speed;
    [SerializeField] public float JumpPower;
    private float InputX;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public float rayCastOffset;
    public AudioSource source;
  
    void Update()
    {
        InputX = Input.GetAxis("Horizontal");


        //if (Input.GetKeyDown(KeyCode.A) && transform.rotation.y != -180f)
        //{
        //    transform.RotateAround(transform.position, new Vector3(0, transform.position.y, 0), -180f);
        //} else if (Input.GetKeyDown(KeyCode.D) && transform.rotation.y != 0f)
        //{
        //    transform.RotateAround(transform.position, new Vector3(0, transform.position.y, 0), 180f);
        //}

        transform.position += new Vector3(InputX * Time.deltaTime * Speed, 0f);

        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, rayCastOffset), new Vector3(0, -0.1f), .1f);
        Debug.DrawRay(transform.position - new Vector3(0, rayCastOffset), new Vector3(0, -0.1f));
        //if (InputX == 0 && hit.collider != null)
        //{
        //    source.Play();
        //}

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Elevator"))
            {
                transform.parent = hit.transform;
            } else
            {
                transform.parent = null;
            }
            if (Input.GetKeyDown(KeyCode.Space) && hit.collider.CompareTag("Ground"))
            {

                rb.AddForce(Vector3.up * JumpPower, ForceMode2D.Impulse);
            }
        }
    }
}
