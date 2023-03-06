using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using PathCreation;
using TMPro;

public class PredatorPathHandler : MonoBehaviour
{
    public int _currentLevel;

    [SerializeField] private Creatures _creaturesPack;

    [SerializeField] private Level[] _levelParams;

    [SerializeField] private bool _isItBeginersLevel;

    [SerializeField] private float _levelSpeed;
    [SerializeField] private TextMeshProUGUI _currentValueOfMeat;
    [SerializeField] private TextMeshProUGUI _currentOfPrey;

    [SerializeField] private GameObject[] _preyArray;

    private int _meatValue;
    private int _totalNewMeat;
    private int _desiredPreyCount;
    private int _currentPreyCount;

    [SerializeField] private GameObject[] _bosses;
    [SerializeField] private GameObject[] _predators;
    [SerializeField] private PathCreator[] _pathes;
    //[SerializeField] private List<PathesWithOffsets> _pathesForPredators = new List<PathesWithOffsets>();
    //[SerializeField] private List<PathesWithOffsets> _pathesForPrey = new List<PathesWithOffsets>();

    [SerializeField] private PathesWithOffsets[] _pathesForPredators;
    [SerializeField] private PathesWithOffsets[] _pathesForPrey;

    [SerializeField] private PathesWithOffsets _pathForBoss;
    [SerializeField] private PathesWithOffsets[] _pathesForPreyWithBoss;


    [SerializeField] private List<AnimalMovement> _animalsOnTheHunt = new List<AnimalMovement>();
    [SerializeField] private List<AnimalMovement> _animalsPrey = new List<AnimalMovement>();
    [SerializeField] private List<Prey> _animalsForHunting = new List<Prey>();

    [SerializeField] private CameraController _cameraController;

    [SerializeField] private Transform _nearestTransform;

    [SerializeField] private GameObject _finishPopUp;
    [SerializeField] private GameObject _plusMeatPopUp;
    [SerializeField] private UIHalper _uiHalper;

    [SerializeField] private PathesWithOffsets _starterPath;

    [SerializeField] private Camera _camera;

    [SerializeField] private SavesPlatformArray _savesPlatformArrayForStart;

    [SerializeField] private GameObject[] _preyOnTheLevel;
    [SerializeField] private PopUpHandler _popUpHandler;
    [SerializeField] private HealthBar _bossHealth;


    [SerializeField] private GameObject _finger;
    private Level thisLevel;

    private bool _isLevelFinished;
    private AnimalMovement _firstPredator;
    private string _dataPath;
    private bool _firstEnemyWasSpawned;

    private void Awake()
    {
        _currentLevel = PlayerPrefs.GetInt("LevelID");
        SetCreaturesParams();
        SetLevelsParams(_currentLevel);

        _dataPath = Application.persistentDataPath;
        GameEvents.OnFindNewPredator += MoveCameraToNewCreature;
        GameEvents.OnPreyDied += RemovePreyAndGetMeat;
        GameEvents.OnUpdateMeatStatus += UpdateMeatStatus;
        _meatValue = PlayerPrefs.GetInt("MeatValue");
        _currentValueOfMeat.text = _meatValue.ToString();

    }

    private void SetCreaturesParams()
    {
        _bosses = _creaturesPack._bosses;
        _predators = _creaturesPack._predators;
        _preyArray = _creaturesPack._pray;
    }

    private void OnDestroy()
    {
        GameEvents.OnFindNewPredator -= MoveCameraToNewCreature;
        GameEvents.OnPreyDied -= RemovePreyAndGetMeat;
        GameEvents.OnUpdateMeatStatus -= UpdateMeatStatus;
    }

    private void SetLevelsParams(int levelID)
    {
        thisLevel = _levelParams[levelID];
        _preyOnTheLevel = new GameObject[thisLevel.preyOnTheHunt.Length];

        if(thisLevel.isItTutorial)
        {
            _finger.SetActive(true);
        }
        for (int i = 0; i < thisLevel.preyOnTheHunt.Length; i++)
        {
            _preyOnTheLevel[i] = _preyArray[thisLevel.preyOnTheHunt[i]];
        }

        _levelSpeed = thisLevel.levelSpeed;

        if (thisLevel.isLevelWithBoss)
        {
            _pathesForPrey = new PathesWithOffsets[_pathesForPreyWithBoss.Length];
            _pathesForPrey = _pathesForPreyWithBoss;

            for (int i = 0; i < _pathesForPreyWithBoss.Length; i++)
            {
                PathesWithOffsets newPath = new PathesWithOffsets();
                newPath.SetParams(_pathesForPreyWithBoss[i].pathCreator, Mathf.Round(Random.Range(thisLevel.minPreyShift, thisLevel.maxPreyShift) * 1000f) / 1000f);
                _pathesForPrey[i] = newPath;
            }

            _pathForBoss.SetParams(_pathForBoss.pathCreator, Mathf.Round(Random.Range(thisLevel.minBossShift, thisLevel.maxBossShift) * 1000f) / 1000f);
        }
        else
        {
            _pathesForPrey = new PathesWithOffsets[thisLevel.availablePathesForPrey];

            for (int i = 0; i < thisLevel.availablePathesForPrey; i++)
            {
                PathesWithOffsets newPath = new PathesWithOffsets();
                newPath.SetParams(_pathes[i], Mathf.Round(Random.Range(thisLevel.minPreyShift, thisLevel.maxPreyShift) * 1000f) / 1000f);
                _pathesForPrey[i] = newPath;
            }
        }


        _pathesForPredators = new PathesWithOffsets[thisLevel.availablePathesForPredators];

        for (int i = 0; i < thisLevel.availablePathesForPredators; i++)
        {
            PathesWithOffsets newPath = new PathesWithOffsets();
            newPath.SetParams(_pathes[i], Mathf.Round(Random.Range(thisLevel.minPredatorsShift, thisLevel.maxPredatorsShift) * 1000f) / 1000f);
            _pathesForPredators[i] = newPath;
        }
    }

    private void RemovePreyAndGetMeat(GameObject prey, int reward)
    {
        _currentPreyCount += 1;
        _currentOfPrey.text = _currentPreyCount.ToString() + "/" + _desiredPreyCount.ToString();
        _animalsForHunting.Remove(prey.GetComponent<Prey>());

        /*
        _meatValue += reward;
        _totalNewMeat += reward;
        _currentValueOfMeat.text = _meatValue.ToString();
        PlayerPrefs.SetInt("MeatValue", _meatValue);
        _plusMeatPopUp.SetActive(true);
        _plusMeatPopUp.GetComponent<UIHalper>().SetText("+" + reward.ToString());
        */

        
    }
    public string GetCurrentLevelType()
    {
        return _levelParams[_currentLevel].sceneType;
    }
    public string GetNextLevelType()
    {
        if (_currentLevel + 1 < 100)
        {
            return _levelParams[_currentLevel + 1].sceneType;
        }
        else
        {
            return _levelParams[0].sceneType;
        }
    }
    private void UpdateMeatStatus(int reward)
    {
        _meatValue += reward;
        _totalNewMeat += reward;
        _currentValueOfMeat.text = _meatValue.ToString();
        PlayerPrefs.SetInt("MeatValue", _meatValue);

        if (_currentPreyCount == _desiredPreyCount)
        {
            _popUpHandler.SetFinishMenuStatusForPopUp(_desiredPreyCount, _currentPreyCount, _totalNewMeat, true);
            _isLevelFinished = true;
            //GameEvents.CallOnLevelFinish();
        }     
    }
    void Start()
    {
        
        InstantiatePrey();

        _desiredPreyCount = _animalsForHunting.Count;
        _currentOfPrey.text = "0/" + _desiredPreyCount.ToString();

        //_uiHalper.StartSavesAndEvents();
        _popUpHandler.IniLevel(_currentLevel);
        SpawnPredatorsFromJson();
        //var nextPredator = _animalsOnTheHunt[Random.Range(0, _animalsOnTheHunt.Count - 1)];
        _cameraController.StartMoveToThePredator(_firstPredator);
        _animalsOnTheHunt.Remove(_firstPredator);
    }

    private void InstantiatePrey()
    {
        foreach (GameObject prey in _preyOnTheLevel)
        {
            GameObject newPrey = Instantiate(prey);
            Prey preyScript = newPrey.GetComponent<Prey>();

            if (preyScript._isItTransport)
            {
                preyScript.SetCameraForHealthBar(_camera);
                _animalsPrey.Add(newPrey.gameObject.GetComponent<AnimalMovement>());

                Transport _transport = newPrey.GetComponent<Transport>();
                _transport._camera = _camera;

                foreach (Prey prayInside in _transport._creaturesInside)
                {
                    _animalsForHunting.Add(prayInside);
                    prayInside.SetCameraForHealthBar(_camera);
                    _animalsPrey.Add(prayInside.gameObject.GetComponent<AnimalMovement>());
                }
            }
            else
            {
                _animalsForHunting.Add(preyScript);
                preyScript.SetCameraForHealthBar(_camera);
                _animalsPrey.Add(newPrey.gameObject.GetComponent<AnimalMovement>());
            }
        }

        if (thisLevel.isLevelWithBoss)
        {
            GameObject newBoss = Instantiate(_bosses[thisLevel.boss]);
            Prey preyScript = newBoss.GetComponent<Prey>();
            preyScript._healthBar = _bossHealth;
            _bossHealth.gameObject.SetActive(true);
            _bossHealth.SetStartHp(preyScript.GetStartHealth());
            _animalsForHunting.Add(preyScript);
            preyScript.SetCameraForHealthBar(_camera);
            newBoss.gameObject.GetComponent<AnimalMovement>()._isItBoss = true;
            _animalsPrey.Add(newBoss.gameObject.GetComponent<AnimalMovement>());
        }
    }

    private void MoveCameraToNewCreature(Transform prey)
    {
        if (_animalsOnTheHunt.Count > 0)
        {
            var nextPredator = _animalsOnTheHunt[Random.Range(0, _animalsOnTheHunt.Count - 1)];
            _cameraController.StartMoveToThePredator(nextPredator, prey);
            _animalsOnTheHunt.Remove(nextPredator);
        }
        else
        {
            foreach (AnimalMovement predator in _animalsOnTheHunt)
            {
                predator.gameObject.GetComponent<DragAndShoot>()._canControl = false;
            }
            _popUpHandler.SetFinishMenuStatusForPopUp(_desiredPreyCount, _currentPreyCount, _totalNewMeat, false);
            _cameraController.Fix(prey);
        }
    }
    private void SpawnPredatorsFromJson()
    {
        if (File.Exists(_dataPath + "/WorldThings.txt"))
        {
            string outputJson = File.ReadAllText(_dataPath + "/WorldThings.txt");
            SavesPlatformArray arrayOfPlatforms = JsonUtility.FromJson<SavesPlatformArray>(outputJson);
            SpawnPrey();

            foreach (SavesPlatforms savesPlatforms in arrayOfPlatforms._savesPlatforms)
            {
                if (!savesPlatforms._isPlatformFree && !_firstEnemyWasSpawned && savesPlatforms._isPlatformForHunt)
                {
                    var newPredator = Instantiate(_predators[savesPlatforms._levelOfCreature]).GetComponent<AnimalMovement>();
                    if (_isItBeginersLevel)
                    {
                        newPredator.SetAimForBeginers();
                    }
                    _firstPredator = newPredator;
                    _firstPredator._isItForstCreature = true;
                    _firstPredator._levelSpeed = _levelSpeed;


                    newPredator.SetPathForAnimal(_starterPath.pathCreator, 0, _starterPath.cameraPos);
                    _animalsOnTheHunt.Add(newPredator);
                    _firstEnemyWasSpawned = true;

                }
                else if (!savesPlatforms._isPlatformFree && _firstEnemyWasSpawned && savesPlatforms._isPlatformForHunt)
                {

                    var newPredator = Instantiate(_predators[savesPlatforms._levelOfCreature]).GetComponent<AnimalMovement>();
                    if (_isItBeginersLevel)
                    {
                        newPredator.SetAimForBeginers();
                    }
                    int randomPathID = Random.Range(0, _pathesForPredators.Length - 1);
                    newPredator.SetPathForAnimal(_pathesForPredators[randomPathID].pathCreator, _pathesForPredators[randomPathID].pathOffset, _pathesForPredators[randomPathID].cameraPos);
                    RemoveElement(ref _pathesForPredators, randomPathID);
                    //_pathesForPredators.RemoveAt(randomPathID);
                    _animalsOnTheHunt.Add(newPredator);
                }
            }
        }
        
    }
    private void SpawnPrey()
    {
        foreach (AnimalMovement animalMovement in _animalsPrey)
        {
            if (_isItBeginersLevel)
            {
                int randomPathID = Random.Range(0, _pathesForPrey.Length - 1);
                animalMovement.SetPathForAnimal(_pathesForPrey[randomPathID].pathCreator, _pathesForPrey[randomPathID].pathOffset, _pathesForPrey[randomPathID].cameraPos, true, _camera);
                //_pathesForPrey.RemoveAt(randomPathID);
                RemoveElement(ref _pathesForPrey, randomPathID);
            }
            else
            {
                int randomPathID = Random.Range(0, _pathesForPrey.Length - 1);
                if(animalMovement._isItBoss)
                {
                    animalMovement.SetPathForAnimal(_pathForBoss.pathCreator, _pathForBoss.pathOffset, _pathForBoss.cameraPos);
                }
                else
                {
                    animalMovement.SetPathForAnimal(_pathesForPrey[randomPathID].pathCreator, _pathesForPrey[randomPathID].pathOffset, _pathesForPrey[randomPathID].cameraPos);
                }

                RemoveElement(ref _pathesForPrey, randomPathID);
            }
        }

        
    }
    public void RemoveElement<T>(ref T[] arr, int index)
    {
        for (int i = index; i < arr.Length - 1; i++)
        {
            arr[i] = arr[i + 1];
        }

        System.Array.Resize(ref arr, arr.Length - 1);
    }

    public Transform ReturnNearestPreyLookPoint(Transform predatorPosition)
    {
        if (_animalsForHunting.Count > 0)
        {
            float distanceToPrey = Mathf.Infinity;
            AnimalMovement _nearestPrey = _animalsForHunting[Random.Range(0, _animalsForHunting.Count - 1)].gameObject.GetComponent<AnimalMovement>();

            for (int i = 0; i < _animalsForHunting.Count; i++)
            {

                if((_animalsForHunting[i].transform.position - predatorPosition.position).magnitude < distanceToPrey)
                {
                    distanceToPrey = (_animalsForHunting[i].transform.position - predatorPosition.position).magnitude;
                    _nearestPrey = _animalsForHunting[i].GetComponent<AnimalMovement>();
                }
            }

            _nearestTransform = _nearestPrey.ReturnLookPoint();

            return _nearestTransform;
        }
        else
        {
            return transform;
        }

    }
}


[System.Serializable]
public class PathesWithOffsets
{
    public PathCreator pathCreator;
    public float pathOffset;
    public bool cameraPos;

    public PathesWithOffsets()
    {
        pathCreator = null;
        pathOffset = 0;
        cameraPos = false;
    }

    public void SetParams(PathCreator path, float offset)
    {
        pathCreator = path;
        pathOffset = offset;
        cameraPos = false;
    }

}
