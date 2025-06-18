using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimationHandler : MonoBehaviour
{
    private Gun _gun;

    private PlayerStateMachine _playerStateMachine;
    
    private void Awake()
    {
        _gun = GetComponentInChildren<Gun>();
    }

    private void Start()
    {
        _playerStateMachine = StageManager.Instance.PlayerController.stateMachine;
    }

    public void Reload()
    {
        _gun.Reload();
        PlayerEvent.OnUpdateBullet?.Invoke(_gun.SpareAmmo, _gun.MaxAmmo);

    }

    public void OnFinishReload()
    {
        _playerStateMachine.ChangeState(new PlayerIdleState(_playerStateMachine));
    }
}
