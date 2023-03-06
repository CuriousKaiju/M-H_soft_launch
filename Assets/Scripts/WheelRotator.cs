using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Vector3 _rotationDirection;

    void Update()
    {
        transform.Rotate(_rotationDirection * _rotationSpeed * Time.deltaTime);
    }
}
