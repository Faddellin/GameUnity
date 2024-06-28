using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameScene;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float normalBulletSpeed = 15f;
    [SerializeField] private LayerMask desturctionLayer;
    private Rigidbody2D rb;
    private bool directionOfShuriken;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((desturctionLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            IDamageable IDamageable = collision.gameObject.GetComponent<IDamageable>();
            if (IDamageable != null)
            {
                if(rb.velocity.x <= 0)
                {
                    directionOfShuriken = false;
                }
                else
                {
                    directionOfShuriken = true;
                }
                IDamageable.Damage(1f,directionOfShuriken, null);
            }
            Destroy(gameObject);
        }
    }

    public void SetStraightVelosity(bool IsRight,Rigidbody2D rb)
    {
        if (IsRight)
        {
            rb.velocity = transform.right * normalBulletSpeed;
        }
        else
        {
            rb.velocity = -transform.right * normalBulletSpeed;
        }
    }
}
