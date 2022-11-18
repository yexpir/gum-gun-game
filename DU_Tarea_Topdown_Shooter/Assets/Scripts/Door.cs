using System;
using System.Collections;
using System.Collections.Generic;
using SalvadorNovo.CharacterRelated;
using UnityEngine;

namespace SalvadorNovo
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private GameObject _door;
        private bool _doOnce;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (PlayerMove.Player.hasKey && !_doOnce)
            {
                print("abrir puerta");
                PlayerMove.Player.UseKey();
                _doOnce = true;
                _door.SetActive(false);
            }
        }
    }
}
