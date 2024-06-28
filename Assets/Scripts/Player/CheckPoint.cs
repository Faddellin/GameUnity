using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameScene;

public class CheckPoint : MonoBehaviour
{
    public Transform player;
    public GameObject TriggerBarrier;
    public int Index;

    private void Awake()
    {
        if (CheckpointContainer.CheckpointIndex == Index)
        {
            player.position = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CheckpointContainer.CheckpointIndex = Index;
            TriggerBarrier.SetActive(true);
        }
    }
}
