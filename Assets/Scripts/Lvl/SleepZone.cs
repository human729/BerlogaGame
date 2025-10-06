using Unity.VisualScripting;
using UnityEngine;

public class SleepZone : MonoBehaviour
{
    [SerializeField] Reload re;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            re.RestartCurrentScene();
        }
    }
}
