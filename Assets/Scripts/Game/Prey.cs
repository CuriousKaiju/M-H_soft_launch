using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lofelt.NiceVibrations;

public class Prey : MonoBehaviour
{
    [SerializeField] private int _totalHealth;
    [SerializeField] private int _reward;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody[] _ragDoll;
    [SerializeField] private Rigidbody _preyRagDoll;
    public HealthBar _healthBar;
    [SerializeField] private Vector3 _punchSkale;
    [SerializeField] private GameObject _bloodParticles;
    [SerializeField] private Transform _animalTransform;
    [SerializeField] private Color _impactColor;
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [SerializeField] private float _impactTime;
    [SerializeField] private AnimalMovement _animalMovement;
    [SerializeField] private HapticSource _haptic;
    [SerializeField] private GameObject _warningParticles;
    [SerializeField] private Collider _magnitCollider;

    public Transport _transport;

    public bool _isItTransport;
    public Transform _huntPoint;
    private bool _isAnimalAlive = true;
    public Rigidbody _pointForConnection;

    private void Awake()
    {
        if(_ragDoll.Length > 0)
        {
            _pointForConnection = _ragDoll[0];
        }

        _healthBar.SetStartHp(_totalHealth);
        RagDollStatus(true);
        AddScripts();
        GameEvents.OnStartHunt += StartMovementHunt;
        GameEvents.OnStartAllGhost += InitMovementOfGhost;
    }
    private void OnDestroy()
    {
        GameEvents.OnStartHunt -= StartMovementHunt;
        GameEvents.OnStartAllGhost -= InitMovementOfGhost;
    }
    public int GetStartHealth()
    {
        return _totalHealth;
    }
    public void GetDamage(int damage, out bool isItDie, out int reward)
    {
        bool creatureStatus = false;

        if (_isAnimalAlive)
        {
            if(_bloodParticles)
            {
                _bloodParticles.SetActive(true);
                _bloodParticles.GetComponent<ParticleSystem>().Play();
            }

            transform.parent = null;
            //transform.DOPunchScale(_punchSkale, _impactTime);
            _animalTransform.DOPunchScale(_punchSkale, _impactTime);
            if(_meshRenderer)
            {
                _meshRenderer.material.DOColor(_impactColor, _impactTime / 2).OnComplete(() =>
                {
                    _meshRenderer.material.DOColor(Color.white, _impactTime / 2);
                });
            }

            _totalHealth -= damage;

            if (_totalHealth > 0)
            {
                _healthBar.MinusHealth(_totalHealth);
                GameEvents.CallOnStartAllGhost();
            }
            else
            {
                if(_transport)
                {
                    _transport.MinusCreature();
                }

                _isAnimalAlive = false;
                GameEvents.CallOnStartAllGhost();
                _magnitCollider.enabled = false;
                creatureStatus = true;
                GameEvents.CallOnPreyDied(gameObject, _reward);
                _healthBar.gameObject.SetActive(false);
                _animalMovement.enabled = false;
                RagDollStatus(false);

                _animalMovement.CloseGhost();
            }

            _haptic.Play();
        }

        reward = _reward;
        isItDie = creatureStatus;
    }

    private IEnumerator SetVarningStatus(float levelSpeed)
    {
        _animalMovement.StartGhost(levelSpeed);

        if(_warningParticles)
        {
            _warningParticles.SetActive(true);
        }
        _animalMovement._speed = levelSpeed;
        _animator.speed = levelSpeed * 10;
        _animator.SetTrigger("Run");
        yield return new WaitForSeconds(1f);

        if (_warningParticles)
        {
            _warningParticles?.SetActive(false);
        }
    }

    private void InitMovementOfGhost()
    {
        if(_isAnimalAlive)
        {
            _animalMovement.StartGhost();
        }
    }

    private void StartMovementHunt(float levelSpeed)
    {
        StartCoroutine(SetVarningStatus(levelSpeed));

        if(_isItTransport)
        {

        }
    }

    private void RagDollStatus(bool status)
    {
        _animator.enabled = status;

        foreach (Rigidbody rb in _ragDoll)
        {
            //rb.GetComponent<Collider>().enabled = status;
            rb.isKinematic = status;
            rb.GetComponent<Collider>().enabled = status;
        }
    }
    private void AddScripts()
    {
        foreach (Rigidbody rb in _ragDoll)
        {
            var preyPart = rb.transform.gameObject.AddComponent<PreyPart>();
            preyPart._rootTransform = transform;
        }
    }

    public void SetCameraForHealthBar(Camera camera)
    {
        _healthBar.SetCamera(camera);
    }
}
