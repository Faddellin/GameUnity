using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameScene;

public class Trigger : MonoBehaviour
{
    public GameObject Panel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Panel.SetActive(true);
        }
    }
}
