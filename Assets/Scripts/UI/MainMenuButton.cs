using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private LayoutElement _layoutElement;
    [SerializeField] private GameObject _additionalColor;
    [SerializeField] private bool _isItPlayMap;

    private bool _scalePhase;
    [SerializeField] private float _desiredScaleTime;
    private float _elapsedScaleTime;
    private float _basicPreferredWidth;
    private float _selectedPreferredWidth;

    void Start()
    {
        _basicPreferredWidth = _layoutElement.preferredWidth;
        _selectedPreferredWidth = _basicPreferredWidth * 1.18f;

        if (_isItPlayMap)
        {
            SuperSize();
        } 
    }

    private void Update()
    {
        if(_scalePhase)
        {
            _elapsedScaleTime += Time.deltaTime;
            float percentage = _elapsedScaleTime / _desiredScaleTime;
            float scale;
            scale = Mathf.Lerp(_basicPreferredWidth, _selectedPreferredWidth, percentage);
            _layoutElement.preferredWidth = scale;
            if(percentage >= 1)
            {
                _scalePhase = false;
                _elapsedScaleTime = 0;
            }
        }
    }

    public void SelectButton()
    {
        _elapsedScaleTime = 0;
        _scalePhase = true;
        _layoutElement.preferredWidth = _basicPreferredWidth;
        _additionalColor.SetActive(true);
        _button.enabled = false;
    }
    public void OffButton()
    {
        _layoutElement.preferredWidth = _basicPreferredWidth;
        _additionalColor.SetActive(false);
        _button.enabled = true;
    }
    public void SuperSize()
    {
        _elapsedScaleTime = 0;
        _scalePhase = true;
        _layoutElement.preferredWidth = _basicPreferredWidth;
    }
}
