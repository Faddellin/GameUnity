using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameScene;
using static UnityEditor.Experimental.GraphView.GraphView;

public class holdAndThrow : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    bool isFacingRight;
    public bool hold;
    public float distance = 1f;
    public GameObject touchedObject;
    public Transform holdPoint;
    public float throwObject = 1f;
    private float playerSpeed;
    private float jumpForce;
    Player _player;

    private void Awake()
    {
        _player = _playerTransform.gameObject.GetComponent<Player>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && touchedObject!= null)
        {
            if (!hold)
            {
                touchedObject.transform.position = holdPoint.transform.position;
                touchedObject.GetComponent<Collider2D>().enabled = false;
                touchedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                hold = true;
            }
            else if (hold)
            {
                isFacingRight = _player.IsFacingRight;
                playerSpeed = Input.GetAxis("Horizontal")* _player.speed;
                jumpForce = _player.rb.velocity.y;

                touchedObject.GetComponent<Collider2D>().enabled = true;
                touchedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                Debug.Log(playerSpeed);
                touchedObject.GetComponent<Rigidbody2D>().AddForce(new Vector2((Convert.ToInt32(isFacingRight) * 2 - 1) * throwObject * 0.5f+playerSpeed*60, 1 * throwObject+jumpForce*60), ForceMode2D.Impulse);
                
                touchedObject = null;
                hold = false;
            }
        }

        if (hold)
        {

            touchedObject.transform.position = holdPoint.transform.position;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Portable")
        {
            touchedObject = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == touchedObject && touchedObject.GetComponent<Collider2D>().enabled)
        {
            touchedObject = null;
        }
    }
}
