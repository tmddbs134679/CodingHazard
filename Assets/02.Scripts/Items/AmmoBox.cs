using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour, IDetectable, IInteractable
{
    public bool IsLockDetect { get; set; }
    public bool IsLockInteract { get; set; }


    [SerializeField] private int fillAmount;
    
    
    private Outline _outline;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

  
    public void Enter()
    {
        if (!IsLockDetect)
        {
            _outline.enabled = true;
        }
    }

    public void Exit()
    {
        if (_outline != null)
        {
            _outline.enabled = false;
        }
    }

    public void Interact()
    {
        var weaponManager = StageManager.Instance.PlayerController.GetComponent<WeaponManager>();

        if (weaponManager != null)
        {
            if (weaponManager.GetWeapon(WeaponType.Gun) is Gun gun)
            {
                gun.AddSpareAmmo(fillAmount);

                IsLockInteract = true;
                IsLockDetect = true;
                _outline.enabled = false;
            }
        }
    }
}
