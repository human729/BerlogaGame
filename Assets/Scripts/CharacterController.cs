using UnityEngine;
public class CharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float Speed = 5f;
    public float JumpPower = 10f;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public AudioSource source;

    [Header("Raycast")]
    public float rayCastOffset = 0.5f;

    private float inputX;
    private bool isGrounded;

    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        Vector3 scale = transform.localScale;
        scale.x = Input.GetAxis("Horizontal") < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
        transform.position += new Vector3(inputX * Speed * Time.deltaTime, 0f);

        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, rayCastOffset), Vector2.down, 0.1f);
        Debug.DrawRay(transform.position - new Vector3(0, rayCastOffset), Vector2.down * 0.1f, Color.red);

        isGrounded = hit.collider != null && hit.collider.CompareTag("Ground");
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("MovingRight", inputX != 0 && isGrounded);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            animator.SetBool("JumpingRight", true);
        }
        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            animator.SetBool("JumpingRight", false);
        }
        if (isGrounded && rb.linearVelocity.y <= 0)
        {
            animator.SetBool("JumpingRight", false);
        }

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Elevator"))
            {
                transform.parent = hit.transform;
            }
            else
            {
                transform.parent = null;
            }
        }
    }
}