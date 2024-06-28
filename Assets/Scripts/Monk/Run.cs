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
    public bool IsTriggered = false;

    private void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (IsTriggered)
        {
            animator.SetBool("IsRight", false);
            StartRunning();
        }
    }

    public void StartRunning()
    {
        if (Point.position.x < transform.position.x)
        {
            rb.velocity = new Vector2(-Speed, rb.velocity.y);
            animator.SetFloat("Speed", Speed);
        }

        if (Mathf.Abs(Point.position.x - transform.position.x) < 2f)
        {
            gameObject.SetActive(false);
        }
    }
}
