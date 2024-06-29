using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameScene;

public class TriggerAudio : MonoBehaviour
{
    public AudioSource FirstAudio;
    public AudioSource LastAudio;
    public GameObject Boss;
    public Run Monk;
    public bool IsBoss;
    public bool IsMonk;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FirstAudio.gameObject.SetActive(false);
            LastAudio.gameObject.SetActive(true);
            if (IsBoss)
            {
                Boss.SetActive(true);
                Monk.IsTriggered = false;
                Monk.gameObject.SetActive(true);
                Monk.transform.position = new Vector2(-425f, 5f);
                Monk.animator.SetBool("IsRight", true);
            }
            if (IsMonk)
            {
                Monk.IsTriggered = true;
            }
        }
    }
}
