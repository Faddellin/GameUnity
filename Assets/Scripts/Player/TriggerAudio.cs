using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    public AudioSource FirstAudio;
    public AudioSource LastAudio;
    public GameObject Boss;
    public bool IsBoss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FirstAudio.gameObject.SetActive(false);
            LastAudio.gameObject.SetActive(true);
            if (IsBoss)
            {
                Boss.SetActive(true);
            }
        }
    }
}
