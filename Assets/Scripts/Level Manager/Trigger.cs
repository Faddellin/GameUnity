using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameScene;
using System;

public class Trigger : MonoBehaviour
{
    public GameObject Panel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CheckpointContainer.CheckpointIndex = -1;
            Panel.SetActive(true);
        }
    }
}
