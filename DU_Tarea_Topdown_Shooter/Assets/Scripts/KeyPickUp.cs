using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using SalvadorNovo.CharacterRelated;
using UnityEngine;

namespace SalvadorNovo
{
    public class KeyPickUp : MonoBehaviour, IPickupable
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
                PickUp();
        }


        public void PickUp()
        {
            PlayerMove.Player.PickUpKey();
            Destroy(gameObject);
        }
    }
}
