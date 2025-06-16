using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/GameSettings")]
public class GameSettings : ScriptableObject
{
    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;
    public float fov;
    public float mouseSensitivity;
    
    public AntialiasingMode antialiasing;
}
