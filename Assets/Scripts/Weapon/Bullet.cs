using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float normalBulletSpeed = 15f;
    [SerializeField] private float destroyTime = 3f;
    [SerializeField] private LayerMask desturctionLayer;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetStraightVelosity();
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

    private void SetStraightVelosity()
    {
        rb.velocity = transform.right * normalBulletSpeed;
    }

    private void DestroyBullet()
    {
        Destroy(gameObject,destroyTime);
    }
}
