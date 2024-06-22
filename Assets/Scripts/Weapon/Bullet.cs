using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameScene;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float normalBulletSpeed = 15f;
    [SerializeField] private float destroyTime = 3f;
    [SerializeField] private LayerMask desturctionLayer;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        DestroyBullet();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((desturctionLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            IDamageable IDamageable = collision.gameObject.GetComponent<IDamageable>();
            if (IDamageable != null)
            {
                IDamageable.Damage(1f);
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

    private void DestroyBullet()
    {
        Destroy(gameObject,destroyTime);
    }
}
