using System;
using System.Collections;
using System.Collections.Generic;
using SalvadorNovo.CharacterRelated;
using UnityEngine;

namespace SalvadorNovo
{
    public class BearPickUp : MonoBehaviour, IPickupable
    {
        public GameObject endBed;
        
        private void OnTriggerEnter(Collider other)
        {
            PickUp();
        }

        public void PickUp()
        {
            endBed.SetActive(true);
            PlayerMove.Player.PickUpBear();
            Destroy(gameObject);
        }
    }
}
