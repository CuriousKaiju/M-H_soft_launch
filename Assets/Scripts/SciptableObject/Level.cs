using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
[System.Serializable]
public class Level : ScriptableObject
{
    public bool isItTutorial;
    public int levelID;
    public int[] preyOnTheHunt;
    public bool isLevelWithBoss;
    public int boss;
    public float levelSpeed;

    public float minPreyShift;
    public float maxPreyShift;

    public float minPredatorsShift;
    public float maxPredatorsShift;

    public float minBossShift;
    public float maxBossShift;

    public string sceneType;


    [Range(0, 12)]
    public int availablePathesForPredators;

    [Range(0, 12)]
    public int availablePathesForPrey;




}
