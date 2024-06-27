using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using GameScene;
using Unity.PlasticSCM.Editor.WebApi;

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

    public void ShurikenInstantiate()
    {
        _player.shurikenSound.Play();
        bulletInst = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.transform.rotation);
        bulletScript = bulletInst.GetComponent<Bullet>();
        bulletScript.SetStraightVelosity(_player.IsFacingRight, bulletInst.GetComponent<Rigidbody2D>());
    }
    private void HandleGunShooting()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !_player.isThrowing && _player.currentShurikenAmount > 0)
        {
            _player.isThrowing = true;
            _player.animator.SetBool("IsThrowing", _player.isThrowing);
            _player.currentShurikenAmount--;

            UpdateInterface();
        }
        else if(_player.currentShurikenAmount == 0)
        {
            FillShurikens();
        }
    }

    private void UpdateInterface()
    {
        for (int i = 0; i < _player.shurikens.Length; i++)
        {
            if (i < Mathf.RoundToInt(_player.currentShurikenAmount))
            {
                _player.shurikens[i].sprite = _player.fullShuriken;
            }
            else
            {
                _player.shurikens[i].sprite = _player.emptyShuriken;
            }

            if (i < _player.maxShurikenAmount)
            {
                _player.shurikens[i].enabled = true;
            }
            else
            {
                _player.shurikens[i].enabled = false;
            }
        }
    }

    public void FillShurikens()
    {
        _player.currentShurikenAmount = _player.maxShurikenAmount;

        for (int i = 0; i < _player.shurikens.Length; i++)
        {
            _player.shurikens[i].enabled = true;
        }
    }
    
    public void AnimationStop()
    {
        _player.isThrowing = false;
        _player.animator.SetBool("IsThrowing", _player.isThrowing);
    }
}
