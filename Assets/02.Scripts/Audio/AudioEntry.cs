using UnityEngine;


[System.Serializable]
public class AudioEntry
{
    public AudioID id;
    
    public AudioType audioType;

    public AudioClip clip;
    
    public bool isLoop;
}
