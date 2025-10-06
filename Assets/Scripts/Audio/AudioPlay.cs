using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {

    }

    public void PlaySound()
    {
        audioSource.Play();
    }
}
