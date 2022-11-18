using System;
using System.Collections;
using System.Collections.Generic;
using SalvadorNovo.CharacterRelated;
using UnityEngine;

namespace SalvadorNovo
{
    public class GunPickUp : MonoBehaviour, IPickupable
    {
        private void OnTriggerEnter(Collider other)
        {
            PickUp();
        }

        public void PickUp()
        {
            PlayerMove.Player.PickUpGun();
            Destroy(gameObject);
        }
    }
}
