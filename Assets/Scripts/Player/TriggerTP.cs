using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameScene;

public class TriggerTP : MonoBehaviour
{
    public Player Player;
    public Transform Destination;
    public GameObject Panel;
    public AudioSource Audio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Teleporting());
        }
    }

    IEnumerator Teleporting()
    {
        Panel.SetActive(true);
        yield return new WaitForSeconds(2f);
        Audio.gameObject.SetActive(true);
        Player.transform.position = Destination.position;
        yield return new WaitForSeconds(2f);
        Audio.gameObject.SetActive(false);
        Panel.SetActive(false);
    }
}
