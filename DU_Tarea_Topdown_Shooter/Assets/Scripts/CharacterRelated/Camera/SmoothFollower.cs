using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollower : MonoBehaviour
{
    public Transform target;
    public float _smoothTime = 0.1f;
    
    private Vector3 _velocity = Vector3.zero;
    private Transform Transform => transform;
    private void Update() => Transform.position = Vector3.SmoothDamp(Transform.position, target.position, ref _velocity, _smoothTime);

}
