using System.Collections;
using System.Collections.Generic;
using SalvadorNovo.Assets.Scripts.CharacterRelated;
using SalvadorNovo.CharacterRelated;
using UnityEngine;

public class PlayerFollowObject : MonoBehaviour
{
    private Vector3 _pos;
    private float _yValue;

    private void Update()
    {
        if (PlayerMove.Player.IsGrounded) _yValue = PlayerMove.Player.PlayerPos.y;
        if (PlayerMove.Player.PlayerPos.y < _yValue) _yValue = PlayerMove.Player.PlayerPos.y;

        _pos.x = PlayerMove.Player.PlayerPos.x;
        _pos.y = _yValue;
        _pos.z = PlayerMove.Player.PlayerPos.z;

        transform.position = _pos;
    }
}
