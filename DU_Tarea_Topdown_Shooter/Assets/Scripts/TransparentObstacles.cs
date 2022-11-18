using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObstacles : MonoBehaviour
{
    [SerializeField] private Material[] _materials = new Material[2];
    [SerializeField] private float _minimumOpacity;
    [SerializeField] private float _fadingSpeed;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Platforms")) return;
        
        var meshRenderer = other.GetComponent<MeshRenderer>();
        meshRenderer.material = _materials[1];
        //meshRenderer.material.SetFloat("_Opacity", 0.0f);
    }
    /*private void OnTriggerStay(Collider other)
    {
        if(!other.gameObject.CompareTag("Platforms")) return;

        var meshRenderer = other.GetComponent<MeshRenderer>();
        var opacity = Vector3.Distance(other.transform.position, transform.position)/22.0f;
        meshRenderer.material.SetFloat("_Opacity", opacity);
        print(opacity);

    }*/

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Platforms")) return;
        
        var meshRenderer = other.GetComponent<MeshRenderer>();
        meshRenderer.material = _materials[0];
    }
}
