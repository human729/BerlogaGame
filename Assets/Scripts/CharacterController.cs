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
    private bool isOnElevator;
    public bool canMove = true;

    void Update()
    {
        if (!canMove)
        {
            animator.SetBool("MovingRight", false);
            return;
        }

        inputX = Input.GetAxisRaw("Horizontal");

        if (inputX != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (inputX < 0 ? -1 : 1);
            transform.localScale = scale;
        }

        transform.position += new Vector3(inputX * Speed * Time.deltaTime, 0f);

        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, rayCastOffset), Vector2.down, 0.1f);
        Debug.DrawRay(transform.position - new Vector3(0, rayCastOffset), Vector2.down * 0.1f, Color.red);

        isOnElevator = hit.collider != null && hit.collider.CompareTag("Elevator");
        isGrounded = hit.collider != null && (hit.collider.CompareTag("Ground") || isOnElevator);

        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("MovingRight", inputX != 0 && isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            animator.SetBool("JumpingRight", true);
        }

        if ((!isGrounded && rb.linearVelocity.y < 0) || (isGrounded && rb.linearVelocity.y <= 0 && !isOnElevator))
        {
            animator.SetBool("JumpingRight", false);
        }

        if (hit.collider != null)
        {
            if (isOnElevator)
            {
                var localScaleX = transform.localScale.x;
                transform.parent = hit.transform;
            }
            else
            {
                transform.parent = null;
            }
        }
    }
}