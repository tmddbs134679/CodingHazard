using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instacne => _instacne;

    private static LobbyManager _instacne;
    
    
    private void Awake()
    {
        if (_instacne == null)
        {
            _instacne = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void StartStage(int level)
    {
        SceneLoadManager.Instance.LoadScene(Constants.StageSceneName + level);
    }

    public void QuitGame()
    {
        Application.Quit();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
