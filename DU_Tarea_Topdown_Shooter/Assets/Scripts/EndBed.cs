using System;
using System.Collections;
using System.Collections.Generic;
using SalvadorNovo.CharacterRelated;
using UnityEngine;

namespace SalvadorNovo
{
    public class EndBed : MonoBehaviour
    {
        private void OnTriggerStay(Collider other)
        {
            if(PlayerMove.Player.IsGrounded)
                Application.Quit();
        }
    }
}
