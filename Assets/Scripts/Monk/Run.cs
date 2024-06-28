using GameScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : MonoBehaviour
{
    public Transform Point;
    private Rigidbody2D rb;
    private Animator animator;

    public float Speed;

    private void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       animator = GetComponent<Animator>();
    }
    public void StartRunning()
    {
        while (Point.position.x < transform.position.x)
        {
            rb.velocity = new Vector2(-Speed, rb.velocity.y);
            animator.SetFloat("Speed", Speed);
        }
    }
}
