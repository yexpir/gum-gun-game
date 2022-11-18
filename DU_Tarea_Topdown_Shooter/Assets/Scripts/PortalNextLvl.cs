using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SalvadorNovo
{
    public class PortalNextLvl : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            SceneLoader.LoadLvl("BedLevel");
        }
    }
}
