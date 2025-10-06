using UnityEngine;

public class OpenDoor: MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetOpen(bool isOpen)
    {
        if (animator != null)
        {
            animator.SetBool("IsOpen", isOpen);
        }
    }
}
