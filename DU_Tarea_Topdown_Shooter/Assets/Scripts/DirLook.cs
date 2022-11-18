using System;
using System.Collections;
using System.Collections.Generic;
using SalvadorNovo.Assets.Scripts.CharacterRelated;
using SalvadorNovo.CharacterRelated;
using UnityEngine;

public class DirLook : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    
    private Quaternion lookRotation;
    private Vector3 _currentDirection;
    private float _rotationTimer;

    private void Update() => LookInDirection();

    private void LookInDirection()
    {
        var direction = PlayerMove.Player.PlayerMovementDir;
        lookRotation = Quaternion.LookRotation(PlayerMove.Player.PlayerAim.forward * direction.z + PlayerMove.Player.PlayerAim.right * direction.x);
        if (direction != _currentDirection)
        {
            _rotationTimer = 0.0f;
            _currentDirection = direction;
        }
        print(_rotationTimer);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _rotationTimer);
        _rotationTimer += _rotationSpeed * Time.deltaTime;
    }
}
