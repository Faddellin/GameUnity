using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    public AudioSource FirstAudio;
    public AudioSource LastAudio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FirstAudio.gameObject.SetActive(false);
            LastAudio.gameObject.SetActive(true);
        }
    }
}
