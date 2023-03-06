using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScenesController : MonoBehaviour
{
    [SerializeField] private GameObject[] _mergeScreen;
    [SerializeField] private GameObject[] _mapScreen;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MenuStatus"))
        {
            switch (PlayerPrefs.GetString("MenuStatus"))
            {
                case "Merge":
                    OpenMergeMenu();
                    break;

                case "Map":
                    OpenMapMenu();
                    break;
            }
        }
        else
        {
            OpenMapMenu();
        }
    }

    public void OpenMergeMenu()
    {
        SetMenuStatus(_mergeScreen, true);
        SetMenuStatus(_mapScreen, false);
    }
    public void OpenMapMenu()
    {
        SetMenuStatus(_mergeScreen, false);
        SetMenuStatus(_mapScreen, true);
    }

    private void SetMenuStatus(GameObject[] menuComponents, bool status)
    {
        foreach(GameObject go in menuComponents)
        {
            go.SetActive(status);
        }
    }
}
