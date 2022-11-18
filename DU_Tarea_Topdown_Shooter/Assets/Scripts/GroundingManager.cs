using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GroundingManager : MonoBehaviour
{
    private Ray _ray;
    private RaycastHit hit;
    [SerializeField] private Transform _aim;
    private Transform _transform;


    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        GetFallingDistance(_transform.position);
    }

    private void GetFallingDistance(Vector3 position)
    {
        var dir = _aim.position - position;
        _ray = new Ray(position, dir);

        if (Physics.Raycast(_ray, out hit))
        {
            var distance = Vector3.Distance(hit.point, position);
            Debug.DrawRay(_ray.origin, _ray.direction.normalized * distance, Color.yellow);
            FallingDistance = distance;
        }
        //Debug.DrawRay(_ray.origin, _ray.direction, Color.yellow);
        FallingDistance = Vector3.Distance(dir, position);
    }

    public static float FallingDistance { get; private set; }
}
