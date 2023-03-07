using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPopUp : MonoBehaviour
{
    [SerializeField] private Animator _popUpAnimator;

    public void ClosePopUp()
    {
        _popUpAnimator.SetTrigger("Close");
    }
}
