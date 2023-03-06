using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class SphereDrawer : MonoBehaviour
{
    public PathCreator _pathCreator;

    public void SetPosition(float totalDistanceForLine)
    {
        transform.position = _pathCreator.path.GetPointAtDistance(totalDistanceForLine);
    }
}
