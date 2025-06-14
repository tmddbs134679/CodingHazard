using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [field: SerializeField] public GameSettingManager GraphicsSettingManager { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        
        GraphicsSettingManager.Init();

        GraphicsSettingManager.SetBaseState();
    }
}
