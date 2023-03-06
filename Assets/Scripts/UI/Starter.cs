using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Starter : MonoBehaviour
{
    [SerializeField] private SavesPlatformArray _savesPlatformArrayForStart;

    private string _dataPath;

    private void Awake()
    {
        _dataPath = Application.persistentDataPath;
    }

    void Start()
    {
        PlayerPrefs.SetString("MenuStatus", "Map");
        SceneManager.LoadScene("MainScene");
    }
}
