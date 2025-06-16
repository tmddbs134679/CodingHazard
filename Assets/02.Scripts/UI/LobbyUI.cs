using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    public Stack<LobbyUISelectMenu> EnabledMenus { get; private set; } = new();

    [field: SerializeField] public Color PointerEnterColor { get; private set; }
    [field: SerializeField] public Color PointerExitColor { get; private set; }
    
    [field: SerializeField] public RectTransform MenuShowPoint { get; private set; }
    [field: SerializeField] public RectTransform MenuHidePoint { get; private set; }

    [SerializeField] private List<LobbyUISelectMenu> selectMenus;
    

    private void Awake()
    {
        foreach (var menu in selectMenus)
        {
            menu.Init(this);
        }
    }

    private void Start()
    {
        for (int i = 0; i < selectMenus.Count; i++)
        {
            if (i == 0)
            {
                selectMenus[i].Show();
            }
            else
            {
                selectMenus[i].Hide();
            }
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EnabledMenus.Count > 1)
            {
                EnabledMenus.Pop().Hide();
                
                EnabledMenus.Peek().Show();
            }
        }
    }
}
