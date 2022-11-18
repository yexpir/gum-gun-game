using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using SalvadorNovo.Assets.Scripts.CharacterRelated;
using SalvadorNovo.CharacterRelated;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IKillable
{
    public GameObject enemyBullet;
    public Transform enemyBulletSpawner;
    public float enemyBulletSpeed;
    public float destroyTime = 5.0f;


    public bool targetPlayer = true;
    
    public float enemyRange;
    public float coolDown;
    private float _timeCount;

    private Transform _enemyTransform;
    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = PlayerMove.Player.transform;
    }

    private void Awake()
    {
        _enemyTransform = transform;
    }

    private void FixedUpdate()
    {
        InRange();
    }

    private void InRange()
    {
        var enemyPos = _enemyTransform.position;
        var playerPos = _playerTransform.position;
        
        if (!(Vector3.Distance(enemyPos, playerPos) < enemyRange)) return;
        
        if(targetPlayer)
        {
            var target = new Vector3(playerPos.x, enemyPos.y, playerPos.z);
            _enemyTransform.forward = (target - enemyPos);
        }
        
        _timeCount += Time.deltaTime;

        if (!(_timeCount > coolDown)) return;
        
        Shoot();
        _timeCount = 0;
    }

    private void Shoot()
    {
        var tempBullet = Instantiate(enemyBullet, enemyBulletSpawner.position, enemyBulletSpawner.rotation) as GameObject;
        var rb = tempBullet.GetComponent<Rigidbody>();
        rb.AddForce(enemyBulletSpawner.forward * enemyBulletSpeed);
        Destroy(tempBullet, destroyTime);
    }

    
    public void Die()
    {
        Destroy(gameObject);
    }
}
