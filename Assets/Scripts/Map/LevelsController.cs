using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class LevelsController : MonoBehaviour
{
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private MapPoint[] _points;

    public int _currentLevel;
    [SerializeField] private float _distanceBetweenPoints;

    [SerializeField] private Camera _camera;
    [SerializeField] private Vector3 _cameraOffset;

    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private int _smoothFactor;
    [SerializeField] private SphereDrawer _sphere;

    private void Start()
    {
        InstantiateLevels();
    }
 
    private void InstantiateLevels()
    {
        float totalDistance = 0;
        float totalDistanceForLine = 0;
        _lineRenderer.positionCount = _points.Length * _smoothFactor;
        float distanceBetweenLinePoints = _distanceBetweenPoints / _smoothFactor;
        int linePointID = 0;
        for (int i = 0; i < _points.Length; i++)
        {
            if (_currentLevel == i)
            {
                _points[i].SetPointParams(i + 1, _pathCreator, totalDistance, transform, true);
                totalDistance += _distanceBetweenPoints;
                _camera.transform.position = _points[i].transform.position + _cameraOffset;

                for(int j = 0; j < _smoothFactor; j++)
                {
                    _sphere.SetPosition(totalDistanceForLine);
                    _lineRenderer.SetPosition(linePointID, _sphere.transform.position);
                    linePointID += 1;
                    totalDistanceForLine += distanceBetweenLinePoints;
                }
            }
            else
            {
                _points[i].SetPointParams(i + 1, _pathCreator, totalDistance, transform);
                totalDistance += _distanceBetweenPoints;

                for(int j = 0; j < _smoothFactor; j++)
                {
                    _sphere.SetPosition(totalDistanceForLine);
                    _lineRenderer.SetPosition(linePointID, _sphere.transform.position);
                    linePointID += 1;
                    totalDistanceForLine += distanceBetweenLinePoints;
                }
            }
        }
    }
}
