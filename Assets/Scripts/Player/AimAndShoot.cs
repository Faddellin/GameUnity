using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using GameScene;

public class AimAndShoot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject Player;

    private Player _player;
    private GameObject bulletInst;
    private Bullet bulletScript;

    private void Awake()
    {
        _player = Player.GetComponent<Player>();
    }

    private void Update()
    {
        HandleGunShooting();
    }


    private void HandleGunShooting()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            bulletInst = Instantiate(bullet,bulletSpawnPoint.position,bulletSpawnPoint.transform.rotation);
            bulletScript = bulletInst.GetComponent<Bullet>();
            bulletScript.SetStraightVelosity(_player.IsFacingRight,bulletInst.GetComponent<Rigidbody2D>());
        }
    }
}
