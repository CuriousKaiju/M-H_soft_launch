using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BottomTabs : MonoBehaviour
{
    [SerializeField] private MainMenuButton _mergeButton;
    [SerializeField] private MainMenuButton _mapButton;
    [SerializeField] private MainMenuButton _playButton;
    [SerializeField] private MainMenuButton _talantesButton;

    [SerializeField] private GameObject[] _mergeComponents;
    [SerializeField] private GameObject[] _mapComponents;


    private void Start()
    {
        SetStartMenu();
    }
    private void SetStartMenu()
    {
        if (PlayerPrefs.HasKey("MenuStatus"))
        {
            switch (PlayerPrefs.GetString("MenuStatus"))
            {
                case "Merge":
                    MergeMenu();
                    Debug.Log("Merge");
                    break;

                case "Map":
                    MapMenu();
                    Debug.Log("Map");
                    break;
            }
        }
        else
        {
            MergeMenu();
        }
    }

    public void MergeMenu()
    {
        _playButton.gameObject.SetActive(false);
        _mapButton.gameObject.SetActive(true);
        //_talantesButton.OffButton();
        _mergeButton.SelectButton();
        SetGOArrayStatus(_mergeComponents, true);
        SetGOArrayStatus(_mapComponents, false);
    }

    public void TalantesMenu()
    {
        DOTween.KillAll();
        _playButton.gameObject.SetActive(false);
        _mapButton.gameObject.SetActive(true);
        //_talantesButton.SelectButton();
        _mergeButton.OffButton();
        SetGOArrayStatus(_mergeComponents, false);
        SetGOArrayStatus(_mapComponents, true);
    }

    public void MapMenu()
    {
        DOTween.KillAll();
        _playButton.gameObject.SetActive(true);
        _mapButton.gameObject.SetActive(false);
        _playButton.SuperSize();
        //_talantesButton.OffButton();
        _mergeButton.OffButton();
        SetGOArrayStatus(_mergeComponents, false);
        SetGOArrayStatus(_mapComponents, true);
    }

    private void SetGOArrayStatus(GameObject[] array, bool status)
    {
        foreach(GameObject go in array)
        {
            go.SetActive(status);
        }
    }
}
