using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeLocationHandler : MonoBehaviour
{

    [SerializeField] private GameObject _plains;
    [SerializeField] private GameObject _winter;
    [SerializeField] private GameObject _sawanna;
    [SerializeField] private GameObject _desert;
    [SerializeField] private GameObject _ocean;
    private GameObject _currenLocation;

    public void SetLocation(string locationType)
    {
        switch (locationType)
        {
            case "Plains":
                SetActiveLocation(_plains);
                break;

            case "WinterForest":
                SetActiveLocation(_winter);
                break;

            case "Savanna":
                SetActiveLocation(_sawanna);
                break;

            case "Desert":
                SetActiveLocation(_desert);
                break;

            case "Ocean":
                SetActiveLocation(_ocean);
                break;
        }
    }

    private void SetActiveLocation(GameObject location)
    {
        if (_currenLocation)
        {
            _currenLocation.SetActive(false);
        }
        location.SetActive(true);
        _currenLocation = location;
    }
}
