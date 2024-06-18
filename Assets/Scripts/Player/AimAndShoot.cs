using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AimAndShoot : MonoBehaviour
{
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;

    private GameObject bulletInst;

    private Vector2 worldPosition;
    private Vector2 direction;
    private float angle;

    private void Update()
    {
        HandleGunRotation();
        HandleGunShooting();
    }

    private void HandleGunRotation()
    {
        worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (worldPosition - (Vector2)gun.transform.position).normalized;
        gun.transform.right = direction;

        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
        Vector3 localScale = new Vector3(1f,1f,1f);
        if(angle > 90 || angle < -90)
        {
            localScale.y = 1f;
        }
        else
        {
            localScale.y = 1f;
        }

        gun.transform.localScale = localScale;
    }

    private void HandleGunShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bulletInst = Instantiate(bullet,bulletSpawnPoint.position,gun.transform.rotation);
        }
    }
}
