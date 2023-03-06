using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transport : MonoBehaviour
{
    public Prey[] _creaturesInside;
    private int _creaturesCount;
    public Camera _camera;

    private void Awake()
    {
        _creaturesCount = _creaturesInside.Length;

        foreach(Prey prey in _creaturesInside)
        {
            prey.SetCameraForHealthBar(_camera);
            prey._transport = this;
        }
    }
    public void MinusCreature()
    {
        _creaturesCount -= 1;
        if(_creaturesCount == 0)
        {
            TransportDestry();
        }
    }
    private void TransportDestry()
    {
        Debug.Log("Destroy");
    }
}
