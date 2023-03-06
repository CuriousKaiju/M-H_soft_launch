using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PathCreation;

public class MapPoint : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] private TextMeshPro _levelText;
    [SerializeField] private GameObject _particles;
    [SerializeField] private MeshRenderer[] _meshRenderers;
    public PathCreator _pathCreator;

    [Header("VARIABLES")]
    public int _level;
    [SerializeField] private Vector3 _basicScale;
    [SerializeField] private Vector3 _selectedScale;
    [SerializeField] private Material[] _basicMaterials;
    [SerializeField] private Material[] _offMaterials;



    public void SetPointParams(int levelID, PathCreator pathCreator, float distanceTraveled, Transform parent,  bool isItCurrentLevel)
    {
        _pathCreator = pathCreator;
        transform.parent = parent;

        _levelText.text = levelID.ToString();
        transform.position = _pathCreator.path.GetPointAtDistance(distanceTraveled);
        SetActiveStatus();

    }
    public void SetPointParams(int levelID, PathCreator pathCreator, float distanceTraveled, Transform parent)
    {
        _pathCreator = pathCreator;
        transform.parent = parent;

        _levelText.text = levelID.ToString();
        transform.position = _pathCreator.path.GetPointAtDistance(distanceTraveled);
    }

    private void SetActiveStatus()
    {
        transform.localScale = _selectedScale;
        _meshRenderers[0].material = _basicMaterials[0];
        _meshRenderers[1].material = _basicMaterials[1];
        _particles.SetActive(true);
    }
    private void SetInactiveStatus()
    {
        transform.localScale = _basicScale;
        _meshRenderers[0].material = _offMaterials[0];
        _meshRenderers[1].material = _offMaterials[1];
        _particles.SetActive(false);
    }

}
