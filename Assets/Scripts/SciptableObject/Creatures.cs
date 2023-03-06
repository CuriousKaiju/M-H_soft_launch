using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Creatures Pack", menuName = "Creatures")]
[System.Serializable]

public class Creatures : ScriptableObject
{
    public GameObject[] _pray;
    public GameObject[] _bosses;
    public GameObject[] _predators;

}
