using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PopUpHandler : MonoBehaviour
{
    [SerializeField] private UIHalper _loseMenu;
    [SerializeField] private UIHalper _winMenu;
    [SerializeField] private PredatorPathHandler _pathHandler;
   

    private bool _twoPopUpsHaveFalseStatus = true;
    private int _currentLevel;



    public void LoadMap(string LeveType)
    {
        DOTween.KillAll();
        PlayerPrefs.SetString("MenuStatus", LeveType);
        SceneManager.LoadScene("MainScene");
    }
    public void LoadMapLose()
    {
        DOTween.KillAll();
        PlayerPrefs.SetString("MenuStatus", "Map");
        TinySauce.OnGameFinished(false, 0, _currentLevel.ToString());
        Debug.Log("Lose:" + _currentLevel);
        SceneManager.LoadScene("MainScene");
    } 
    public void ThisLevelAgain()
    {
        SceneManager.LoadScene(_pathHandler.GetCurrentLevelType());
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(_pathHandler.GetNextLevelType());
    }
    public void IniLevel(int levelID)
    {
        _currentLevel = levelID;
        TinySauce.OnGameStarted(_currentLevel.ToString());
        Debug.Log("start: " + (levelID).ToString());
    }

    public void UpdatePopUpStatuses(int desiredPreyCount, int playersPreyCount, int newMeat)
    {
        _winMenu.SetFinishMenuStatus(desiredPreyCount, playersPreyCount, newMeat);
        _loseMenu.SetFinishMenuStatus(desiredPreyCount, playersPreyCount, newMeat);
    }

    public void SetFinishMenuStatusForPopUp(int desiredPreyCount, int playersPreyCount, int newMeat, bool finishStatus)
    {
        if (_twoPopUpsHaveFalseStatus)
        {
            if (finishStatus)
            {
                TinySauce.OnGameFinished(true, newMeat, _currentLevel.ToString());
                Debug.Log("Win:" + _currentLevel);
                _winMenu.gameObject.SetActive(true);
                _winMenu.SetFinishMenuStatus(desiredPreyCount, playersPreyCount, newMeat);
                PlayerPrefs.SetInt("LevelID", _currentLevel + 1);
            }
            else
            {
                TinySauce.OnGameFinished(false, newMeat, _currentLevel.ToString());
                Debug.Log("Lose:" + _currentLevel);
                _loseMenu.gameObject.SetActive(true);
                _loseMenu.SetFinishMenuStatus(desiredPreyCount, playersPreyCount, newMeat);
            }

            _twoPopUpsHaveFalseStatus = false;
        }
    }
}
